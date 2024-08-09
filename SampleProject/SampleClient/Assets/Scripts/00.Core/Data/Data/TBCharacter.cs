using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class UTBCharacter_Record
{
    public int ID;
    public ETB_CHARACTER Type;
    public string Name;
    public int Health;
    public float Speed;
    public int MaxHealth;


    public void Read(BinaryReader fs, byte[] buffer)
    {
        ID = fs.ReadInt32();
        byte val_Type = fs.ReadByte(); Type = (ETB_CHARACTER)val_Type;
        ushort cnt = 0;
        cnt = fs.ReadUInt16();
        if (cnt > 0) { fs.Read(buffer, 0, cnt * 2); Name = System.Text.Encoding.Unicode.GetString(buffer, 0, cnt * 2); }
        Health = fs.ReadInt32();
        Speed = fs.ReadSingle();
        MaxHealth = fs.ReadInt32();
    }
};

public class UTBCharacter
{
    public Dictionary<int, UTBCharacter_Record> mapTable = new Dictionary<int, UTBCharacter_Record>();

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
            UTBCharacter_Record rec = new UTBCharacter_Record();

            rec.Read(fs, buffer);

            mapTable.Add(rec.ID, rec);
        }

        return true;
    }

    public bool LoadFromResources(byte[] buffer)
    {
        TextAsset textAsset = Resources.Load("Table/Character") as TextAsset;
        if (textAsset == null) { return false; }
        bool ret = LoadFromStream(new BinaryReader(new MemoryStream(textAsset.bytes)), buffer);
        Resources.UnloadAsset(textAsset);
        return ret;
    }

    public bool LoadFromFile(byte[] buffer)
    {
        string filePath = Application.temporaryCachePath + "/Table/Character.txt";
        if (File.Exists(filePath) == false) { return false; }
        return LoadFromStream(new BinaryReader(new FileStream(filePath, FileMode.Open)), buffer);
    }

    public int GetLastVersion(out int OutStorageType)
    {
        OutStorageType = 0;

        int verResources = 0;
        int verDownload = 0;

        TextAsset textAsset = Resources.Load("Table/Character") as TextAsset;
        if (textAsset != null) { verResources = BitConverter.ToInt32(textAsset.bytes, 0); }
        Resources.UnloadAsset(textAsset);

        string filePath = Application.temporaryCachePath + "/Table/Character.txt";
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

    public UTBCharacter_Record Find(int key1)
    {
        UTBCharacter_Record rec = null;
        if (mapTable.TryGetValue(key1, out rec) == false)
        {
            return null;
        }
        return rec;
    }
};
