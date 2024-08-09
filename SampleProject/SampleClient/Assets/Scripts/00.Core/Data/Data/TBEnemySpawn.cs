using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class UTBEnemySpawn_Record
{
    public int ID;
    public string Grade;
    public int SpawnRate;
    public int IncreaseRate;
    public float MaxSpawnRate;


    public void Read(BinaryReader fs, byte[] buffer)
    {
        ID = fs.ReadInt32();
        ushort cnt = 0;
        cnt = fs.ReadUInt16();
        if (cnt > 0) { fs.Read(buffer, 0, cnt * 2); Grade = System.Text.Encoding.Unicode.GetString(buffer, 0, cnt * 2); }
        SpawnRate = fs.ReadInt32();
        IncreaseRate = fs.ReadInt32();
        MaxSpawnRate = fs.ReadSingle();
    }
};

public class UTBEnemySpawn
{
    public Dictionary<int, UTBEnemySpawn_Record> mapTable = new Dictionary<int, UTBEnemySpawn_Record>();

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
            UTBEnemySpawn_Record rec = new UTBEnemySpawn_Record();

            rec.Read(fs, buffer);

            mapTable.Add(rec.ID, rec);
        }

        return true;
    }

    public bool LoadFromResources(byte[] buffer)
    {
        TextAsset textAsset = Resources.Load("Table/EnemySpawn") as TextAsset;
        if (textAsset == null) { return false; }
        bool ret = LoadFromStream(new BinaryReader(new MemoryStream(textAsset.bytes)), buffer);
        Resources.UnloadAsset(textAsset);
        return ret;
    }

    public bool LoadFromFile(byte[] buffer)
    {
        string filePath = Application.temporaryCachePath + "/Table/EnemySpawn.txt";
        if (File.Exists(filePath) == false) { return false; }
        return LoadFromStream(new BinaryReader(new FileStream(filePath, FileMode.Open)), buffer);
    }

    public int GetLastVersion(out int OutStorageType)
    {
        OutStorageType = 0;

        int verResources = 0;
        int verDownload = 0;

        TextAsset textAsset = Resources.Load("Table/EnemySpawn") as TextAsset;
        if (textAsset != null) { verResources = BitConverter.ToInt32(textAsset.bytes, 0); }
        Resources.UnloadAsset(textAsset);

        string filePath = Application.temporaryCachePath + "/Table/EnemySpawn.txt";
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

    public UTBEnemySpawn_Record Find(int key1)
    {
        UTBEnemySpawn_Record rec = null;
        if (mapTable.TryGetValue(key1, out rec) == false)
        {
            return null;
        }
        return rec;
    }
};
