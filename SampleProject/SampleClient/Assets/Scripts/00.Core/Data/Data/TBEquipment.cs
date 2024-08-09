using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class UTBEquipment_Record
{
    public int EquipID;
    public string EquipName;
    public int Name;
    public int Desc;
    public int ResID;
    public FTB_WeaponStat WeaponStat;
    public FTB_WeaponUpgrade[] WeaponUpgrade = new FTB_WeaponUpgrade[0];
    public string PrefabName;


    public void Read(BinaryReader fs, byte[] buffer)
    {
        EquipID = fs.ReadInt32();
        ushort cnt = 0;
        cnt = fs.ReadUInt16();
        if (cnt > 0) { fs.Read(buffer, 0, cnt * 2); EquipName = System.Text.Encoding.Unicode.GetString(buffer, 0, cnt * 2); }
        Name = fs.ReadInt32();
        Desc = fs.ReadInt32();
        ResID = fs.ReadInt32();
        WeaponStat.Damage = fs.ReadInt32();
        WeaponStat.FireRate = fs.ReadSingle();
        WeaponStat.Penetration = fs.ReadInt32();
        WeaponStat.ProjectileNum = fs.ReadInt32();
        WeaponStat.ProjectileSpeed = fs.ReadSingle();
        WeaponStat.SplashRadius = fs.ReadSingle();
        WeaponStat.RotationSpeed = fs.ReadSingle();
        WeaponStat.RotationRadius = fs.ReadSingle();
        WeaponStat.ActiveDuration = fs.ReadSingle();
        WeaponStat.InActiveDuration = fs.ReadSingle();
        WeaponStat.LifeDrain = fs.ReadInt32();
        cnt = fs.ReadUInt16();
        if (cnt > 0) { WeaponUpgrade = new FTB_WeaponUpgrade[cnt]; for (int i = 0; i < cnt; ++i) {
            WeaponUpgrade[i].Damage = fs.ReadInt32();
            WeaponUpgrade[i].FireRate = fs.ReadSingle();
            WeaponUpgrade[i].Penetration = fs.ReadInt32();
            WeaponUpgrade[i].ProjectileNum = fs.ReadInt32();
            WeaponUpgrade[i].ProjectileSpeed = fs.ReadSingle();
            WeaponUpgrade[i].SplashRadius = fs.ReadSingle();
            WeaponUpgrade[i].RotationSpeed = fs.ReadSingle();
            WeaponUpgrade[i].RotationRadius = fs.ReadSingle();
            WeaponUpgrade[i].ActiveDuration = fs.ReadSingle();
            WeaponUpgrade[i].InActiveDuration = fs.ReadSingle();
            WeaponUpgrade[i].LifeDrain = fs.ReadInt32();
        }}
        cnt = fs.ReadUInt16();
        if (cnt > 0) { fs.Read(buffer, 0, cnt * 2); PrefabName = System.Text.Encoding.Unicode.GetString(buffer, 0, cnt * 2); }
    }
};

public class UTBEquipment
{
    public Dictionary<int, UTBEquipment_Record> mapTable = new Dictionary<int, UTBEquipment_Record>();

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
            UTBEquipment_Record rec = new UTBEquipment_Record();

            rec.Read(fs, buffer);

            mapTable.Add(rec.EquipID, rec);
        }

        return true;
    }

    public bool LoadFromResources(byte[] buffer)
    {
        TextAsset textAsset = Resources.Load("Table/Equipment") as TextAsset;
        if (textAsset == null) { return false; }
        bool ret = LoadFromStream(new BinaryReader(new MemoryStream(textAsset.bytes)), buffer);
        Resources.UnloadAsset(textAsset);
        return ret;
    }

    public bool LoadFromFile(byte[] buffer)
    {
        string filePath = Application.temporaryCachePath + "/Table/Equipment.txt";
        if (File.Exists(filePath) == false) { return false; }
        return LoadFromStream(new BinaryReader(new FileStream(filePath, FileMode.Open)), buffer);
    }

    public int GetLastVersion(out int OutStorageType)
    {
        OutStorageType = 0;

        int verResources = 0;
        int verDownload = 0;

        TextAsset textAsset = Resources.Load("Table/Equipment") as TextAsset;
        if (textAsset != null) { verResources = BitConverter.ToInt32(textAsset.bytes, 0); }
        Resources.UnloadAsset(textAsset);

        string filePath = Application.temporaryCachePath + "/Table/Equipment.txt";
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

    public UTBEquipment_Record Find(int key1)
    {
        UTBEquipment_Record rec = null;
        if (mapTable.TryGetValue(key1, out rec) == false)
        {
            return null;
        }
        return rec;
    }
};
