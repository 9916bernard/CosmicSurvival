using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class UTBArtifact_Record
{
    public int ArtifactID;
    public string Grade;
    public string Name;


    public void Read(BinaryReader fs, byte[] buffer)
    {
        ArtifactID = fs.ReadInt32();
        ushort cnt = 0;
        cnt = fs.ReadUInt16();
        if (cnt > 0) { fs.Read(buffer, 0, cnt * 2); Grade = System.Text.Encoding.Unicode.GetString(buffer, 0, cnt * 2); }
        cnt = fs.ReadUInt16();
        if (cnt > 0) { fs.Read(buffer, 0, cnt * 2); Name = System.Text.Encoding.Unicode.GetString(buffer, 0, cnt * 2); }
    }
};

public class UTBArtifact
{
    public Dictionary<int, List<UTBArtifact_Record>> mapTable = new Dictionary<int, List<UTBArtifact_Record>>();

    public void Load(byte[] buffer, bool InForceResources)
    {
        mapTable.Clear();

        if (InForceResources == true)
        {
            LoadFromResources(buffer);
            return;
        }

        int storageType = 0;

        GetLastVersion(out storageType);

        if (storageType == 1) // Resources
        {
            LoadFromResources(buffer);
        }
        else if (storageType == 2) // Download
        {
            LoadFromFile(buffer);
        }
    }

    public bool LoadFromStream(BinaryReader fs, byte[] buffer)
    {
        if (fs == null)
            return false;

        int version = fs.ReadInt32();
        int totalRowCount = fs.ReadInt32();
        int maxBufferSize = fs.ReadInt32();

        for (int i = 0; i < totalRowCount; ++i)
        {
            UTBArtifact_Record rec = new UTBArtifact_Record();

            rec.Read(fs, buffer);

            List<UTBArtifact_Record> list = null;
            if (mapTable.TryGetValue(rec.ArtifactID, out list) == false)
            {
                list = new List<UTBArtifact_Record>();
                mapTable.Add(rec.ArtifactID, list);
            }

            list.Add(rec);
        }

        return true;
    }

    public bool LoadFromResources(byte[] buffer)
    {
        TextAsset textAsset = Resources.Load("Table/Artifact") as TextAsset;
        if (textAsset == null) { return false; }
        bool ret = LoadFromStream(new BinaryReader(new MemoryStream(textAsset.bytes)), buffer);
        Resources.UnloadAsset(textAsset);
        return ret;
    }

    public bool LoadFromFile(byte[] buffer)
    {
        string filePath = Application.temporaryCachePath + "/Table/Artifact.txt";
        if (File.Exists(filePath) == false) { return false; }
        return LoadFromStream(new BinaryReader(new FileStream(filePath, FileMode.Open)), buffer);
    }

    public int GetLastVersion(out int OutStorageType)
    {
        OutStorageType = 0;

        int verResources = 0;
        int verDownload = 0;

        TextAsset textAsset = Resources.Load("Table/Artifact") as TextAsset;
        if (textAsset != null) { verResources = BitConverter.ToInt32(textAsset.bytes, 0); }
        Resources.UnloadAsset(textAsset);

        string filePath = Application.temporaryCachePath + "/Table/Artifact.txt";
        if (File.Exists(filePath) == true)
        {
            using (BinaryReader fs = new BinaryReader(new FileStream(filePath, FileMode.Open)))
            {
                verDownload = fs.ReadInt32();
            }
        }

        if (verResources >= verDownload) { OutStorageType = 1; return verResources; }
        else { OutStorageType = 2; return verDownload; }
    }

    public List<UTBArtifact_Record> Find(int key1)
    {
        List<UTBArtifact_Record> list = null;
        if (mapTable.TryGetValue(key1, out list) == false)
        {
            return null;
        }
        return list;
    }
};
