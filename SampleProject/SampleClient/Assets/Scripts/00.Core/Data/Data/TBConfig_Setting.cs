using System;
using System.IO;
using UnityEngine;

public class UTBConfig_Setting
{
    public int ServerResetTime;
    public int[] StoryCastleList = new int[0];
    public int DefaultPlayerCharacterID;
    public int DefaultFormationID;
    public float UnitScale;
    public float UnitScaleNext;
    public float UnitScaleLeader;
    public Vector3 UnitRotateLeft;
    public Vector3 UnitRotateRight;
    public Vector3 NextPosLeft;
    public Vector3 NextPostRight;
    public float NextDistanceLeft;
    public float NextDistanceRight;
    public Vector2 StartPostLeft;
    public Vector2 StartPostRight;
    public FTB_UpgradeMineral[] UpgradeMineralList = new FTB_UpgradeMineral[0];

    void Read(BinaryReader fs, byte[] buffer)
    {
        ServerResetTime = fs.ReadInt32();
        ushort cnt = 0;
        cnt = fs.ReadUInt16();
        if (cnt > 0) { StoryCastleList = new int[cnt]; fs.Read(buffer, 0, cnt * 4); Buffer.BlockCopy(buffer, 0, StoryCastleList, 0, cnt * 4); }
        DefaultPlayerCharacterID = fs.ReadInt32();
        DefaultFormationID = fs.ReadInt32();
        UnitScale = fs.ReadSingle();
        UnitScaleNext = fs.ReadSingle();
        UnitScaleLeader = fs.ReadSingle();
        UnitRotateLeft.x = fs.ReadSingle(); UnitRotateLeft.y = fs.ReadSingle(); UnitRotateLeft.z = fs.ReadSingle();
        UnitRotateRight.x = fs.ReadSingle(); UnitRotateRight.y = fs.ReadSingle(); UnitRotateRight.z = fs.ReadSingle();
        NextPosLeft.x = fs.ReadSingle(); NextPosLeft.y = fs.ReadSingle(); NextPosLeft.z = fs.ReadSingle();
        NextPostRight.x = fs.ReadSingle(); NextPostRight.y = fs.ReadSingle(); NextPostRight.z = fs.ReadSingle();
        NextDistanceLeft = fs.ReadSingle();
        NextDistanceRight = fs.ReadSingle();
        StartPostLeft.x = fs.ReadSingle(); StartPostLeft.y = fs.ReadSingle();
        StartPostRight.x = fs.ReadSingle(); StartPostRight.y = fs.ReadSingle();
        cnt = fs.ReadUInt16();
        if (cnt > 0) { UpgradeMineralList = new FTB_UpgradeMineral[cnt]; for (int i = 0; i < cnt; ++i) {
            byte val_UpgradeMineralList_FundType = fs.ReadByte(); UpgradeMineralList[i].FundType = (ETB_FUND)val_UpgradeMineralList_FundType;
            UpgradeMineralList[i].Buy = fs.ReadInt32();
            UpgradeMineralList[i].Sell = fs.ReadInt32();
        }}
    }

    public void Load(byte[] buffer, bool InForceResources)
    {
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

    bool LoadFromStream(BinaryReader fs, byte[] buffer)
    {
        if (fs == null)
        {
            return false;
        }

        int version = fs.ReadInt32();
        int totalRowCount = fs.ReadInt32();
        int maxBufferSize = fs.ReadInt32();

        Read(fs, buffer);

        fs.Close();
        fs.Dispose();

        return true;
    }

    public bool LoadFromResources(byte[] buffer)
    {
        TextAsset textAsset = Resources.Load("Table/Config_Setting") as TextAsset;
        if (textAsset == null) { return false; }
        bool ret = LoadFromStream(new BinaryReader(new MemoryStream(textAsset.bytes)), buffer);
        Resources.UnloadAsset(textAsset);
        return ret;
    }

    public bool LoadFromFile(byte[] buffer)
    {
        string filePath = Application.temporaryCachePath + "/Table/Config_Setting.txt";
        if (File.Exists(filePath) == false) { return false; }
        return LoadFromStream(new BinaryReader(new FileStream(filePath, FileMode.Open)), buffer);
    }

    public int GetLastVersion(out int OutStorageType)
    {
        OutStorageType = 0;

        int verResources = 0;
        int verDownload = 0;

        TextAsset textAsset = Resources.Load("Table/Config_Setting") as TextAsset;
        if (textAsset != null) { verResources = BitConverter.ToInt32(textAsset.bytes, 0); }
        Resources.UnloadAsset(textAsset);

        string filePath = Application.temporaryCachePath + "/Table/Config_Setting.txt";
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
}
