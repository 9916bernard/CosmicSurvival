using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class UTBTutorial_Record
{
    public ETB_TUTORIAL TutorialType;
    public Vector2 Position;
    public int Desc;


    public void Read(BinaryReader fs, byte[] buffer)
    {
        ushort val_TutorialType = fs.ReadUInt16(); TutorialType = (ETB_TUTORIAL)val_TutorialType;
        Position.x = fs.ReadSingle(); Position.y = fs.ReadSingle();
        Desc = fs.ReadInt32();
    }
};

public class UTBTutorial
{
    public Dictionary<ETB_TUTORIAL, List<UTBTutorial_Record>> mapTable = new Dictionary<ETB_TUTORIAL, List<UTBTutorial_Record>>();

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
            UTBTutorial_Record rec = new UTBTutorial_Record();

            rec.Read(fs, buffer);

            List<UTBTutorial_Record> list = null;
            if (mapTable.TryGetValue(rec.TutorialType, out list) == false)
            {
                list = new List<UTBTutorial_Record>();
                mapTable.Add(rec.TutorialType, list);
            }

            list.Add(rec);
        }

        return true;
    }

    public bool LoadFromResources(byte[] buffer)
    {
        TextAsset textAsset = Resources.Load("Table/Tutorial") as TextAsset;
        if (textAsset == null) { return false; }
        bool ret = LoadFromStream(new BinaryReader(new MemoryStream(textAsset.bytes)), buffer);
        Resources.UnloadAsset(textAsset);
        return ret;
    }

    public bool LoadFromFile(byte[] buffer)
    {
        string filePath = Application.temporaryCachePath + "/Table/Tutorial.txt";
        if (File.Exists(filePath) == false) { return false; }
        return LoadFromStream(new BinaryReader(new FileStream(filePath, FileMode.Open)), buffer);
    }

    public int GetLastVersion(out int OutStorageType)
    {
        OutStorageType = 0;

        int verResources = 0;
        int verDownload = 0;

        TextAsset textAsset = Resources.Load("Table/Tutorial") as TextAsset;
        if (textAsset != null) { verResources = BitConverter.ToInt32(textAsset.bytes, 0); }
        Resources.UnloadAsset(textAsset);

        string filePath = Application.temporaryCachePath + "/Table/Tutorial.txt";
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

    public List<UTBTutorial_Record> Find(ETB_TUTORIAL key1)
    {
        List<UTBTutorial_Record> list = null;
        if (mapTable.TryGetValue(key1, out list) == false)
        {
            return null;
        }
        return list;
    }
};
