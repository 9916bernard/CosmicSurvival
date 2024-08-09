
// [System Struct]


using System;
using System.Runtime.InteropServices;


[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
[Serializable]
public struct FTB_Stat
{
    public long Attack;
    public short AttackRate;
    public long Defense;
    public short DefenseRate;
    public long Life;
    public short LifeRate;
    public short Speed;
    public short CriticalRate;
    public short CriticalDamage;
    public short Continuous;
    public short DrainLife;
    public short Counter;
    public short Stun;
    public short Evade;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
[Serializable]
public struct FTB_ShipInfo
{
    public ETB_SHIP_TYPE ShipType;
    public int Attack;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
[Serializable]
public struct FTB_EnemyUpgrade
{
    public int Attack;
    public float Speed;
    public int Health;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
[Serializable]
public struct FTB_WeaponStat
{
    public int Damage;
    public float FireRate;
    public int Penetration;
    public int ProjectileNum;
    public float ProjectileSpeed;
    public float SplashRadius;
    public float RotationSpeed;
    public float RotationRadius;
    public float ActiveDuration;
    public float InActiveDuration;
    public int LifeDrain;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
[Serializable]
public struct FTB_WeaponUpgrade
{
    public int Damage;
    public float FireRate;
    public int Penetration;
    public int ProjectileNum;
    public float ProjectileSpeed;
    public float SplashRadius;
    public float RotationSpeed;
    public float RotationRadius;
    public float ActiveDuration;
    public float InActiveDuration;
    public int LifeDrain;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
[Serializable]
public struct FTB_UpgradeMineral
{
    public ETB_FUND FundType;
    public int Buy;
    public int Sell;
}

