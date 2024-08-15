using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class UTBUpgrade_Record
{
    public int ID;
    public ETB_UPGRADE_EFFECT UpgradeType;
    public string Name;
    public short Order;
    public float UpgradeAmount;
    public int UpgradeCost;
    public int UpgradeMaxLevel;


    public void Read(BinaryReader fs, byte[] buffer)
    {
        ID = fs.ReadInt32();
        ushort val_UpgradeType = fs.ReadUInt16(); UpgradeType = (ETB_UPGRADE_EFFECT)val_UpgradeType;
        ushort cnt = 0;
        cnt = fs.ReadUInt16();
        if (cnt > 0) { fs.Read(buffer, 0, cnt * 2); Name = System.Text.Encoding.Unicode.GetString(buffer, 0, cnt * 2); }
        Order = fs.ReadInt16();
        UpgradeAmount = fs.ReadSingle();
        UpgradeCost = fs.ReadInt32();
        UpgradeMaxLevel = fs.ReadInt32();
    }
};

public class UTBUpgrade
{
    public Dictionary<int, UTBUpgrade_Record> mapTable = new Dictionary<int, UTBUpgrade_Record>();

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
            UTBUpgrade_Record rec = new UTBUpgrade_Record();

            rec.Read(fs, buffer);

            mapTable.Add(rec.ID, rec);
        }

        return true;
    }

    public bool LoadFromResources(byte[] buffer)
    {
        TextAsset textAsset = Resources.Load("Table/Upgrade") as TextAsset;
        if (textAsset == null) { return false; }
        bool ret = LoadFromStream(new BinaryReader(new MemoryStream(textAsset.bytes)), buffer);
        Resources.UnloadAsset(textAsset);
        return ret;
    }

    public bool LoadFromFile(byte[] buffer)
    {
        string filePath = Application.temporaryCachePath + "/Table/Upgrade.txt";
        if (File.Exists(filePath) == false) { return false; }
        return LoadFromStream(new BinaryReader(new FileStream(filePath, FileMode.Open)), buffer);
    }

    public int GetLastVersion(out int OutStorageType)
    {
        OutStorageType = 0;

        int verResources = 0;
        int verDownload = 0;

        TextAsset textAsset = Resources.Load("Table/Upgrade") as TextAsset;
        if (textAsset != null) { verResources = BitConverter.ToInt32(textAsset.bytes, 0); }
        Resources.UnloadAsset(textAsset);

        string filePath = Application.temporaryCachePath + "/Table/Upgrade.txt";
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

    public UTBUpgrade_Record Find(int key1)
    {
        UTBUpgrade_Record rec = null;
        if (mapTable.TryGetValue(key1, out rec) == false)
        {
            return null;
        }
        return rec;
    }
};
