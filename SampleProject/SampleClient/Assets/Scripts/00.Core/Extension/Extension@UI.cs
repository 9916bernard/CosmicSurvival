using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static partial class Extension
{
    //=================================================================================
    // Text
    //=================================================================================
    public static void SetText(this Text InText, string InStr)
    {
        if (InText != null)
        {
            InText.text = InStr;
        }
    }

    public static void SetText(this Text InText, int InID)
    {
        if (InText != null)
        {
            InText.text = UIM.Inst().GetText(InID);
        }
    }

    //=================================================================================
    // UITextMeshProUGUI
    //=================================================================================
    public static void SetText(this UITextMeshProUGUI InText, string InStr)
    {
        if (InText != null)
        {
            InText.text = InStr;
        }
    }

    public static void SetText(this UITextMeshProUGUI InText, int InID)
    {
        if (InText != null)
        {
            InText.text = UIM.Inst().GetText(InID);
        }
    }

    //=================================================================================
    // Image
    //=================================================================================
    public static void SetSprite(this Image InImage, EUI_AtlasType InAtlasType, string InName)
    {
        if (InImage != null)
        {
            InImage.sprite = RESOURCE.Inst().GetSprite(InAtlasType, InName);
        }
    }

    public static void SetSprite(this Image InImage, EUI_AtlasType InAtlasType, int InID)
    {
        if (InImage != null)
        {
            InImage.sprite = RESOURCE.Inst().GetSprite(InAtlasType, InID);
        }
    }

    public static void SetTexture(this Image InImage, EUI_TextureType InTextureType, string InName)
    {
        if (InImage != null)
        {
            InImage.sprite = RESOURCE.Inst().GetTexture(InTextureType, InName);
        }
    }

    public static void SetTexture(this Image InImage, EUI_TextureType InTextureType, int InID)
    {
        if (InImage != null)
        {
            InImage.sprite = RESOURCE.Inst().GetTexture(InTextureType, InID);
        }
    }


    //=================================================================================
    // Struct
    //=================================================================================
    public static void Add(this FTB_WeaponStat InStat, FTB_WeaponStat AddStat)
    {
        InStat.Damage += AddStat.Damage;
        InStat.FireRate += AddStat.FireRate;
        InStat.Penetration += AddStat.Penetration;
        InStat.ProjectileNum += AddStat.ProjectileNum;
        InStat.ProjectileSpeed += AddStat.ProjectileSpeed;
        InStat.SplashRadius += AddStat.SplashRadius;
        InStat.RotationSpeed += AddStat.RotationSpeed;
        InStat.RotationRadius += AddStat.RotationRadius;
        InStat.ActiveDuration += AddStat.ActiveDuration;
        InStat.InActiveDuration += AddStat.InActiveDuration;
        InStat.LifeDrain += AddStat.LifeDrain;
    }

    public static void Add(ref this FTB_WeaponStat InStat, FTB_WeaponUpgrade AddStat)
    {
        InStat.Damage += AddStat.Damage;
        InStat.FireRate += AddStat.FireRate;
        InStat.Penetration += AddStat.Penetration;
        InStat.ProjectileNum += AddStat.ProjectileNum;
        InStat.ProjectileSpeed += AddStat.ProjectileSpeed;
        InStat.SplashRadius += AddStat.SplashRadius;
        InStat.RotationSpeed += AddStat.RotationSpeed;
        InStat.RotationRadius += AddStat.RotationRadius;
        InStat.ActiveDuration += AddStat.ActiveDuration;
        InStat.InActiveDuration += AddStat.InActiveDuration;
        InStat.LifeDrain += AddStat.LifeDrain;


        //int Damage;
        //float FireRate;
        //int Penetration;
        //int ProjectileNum;
        //float ProjectileSpeed;
        //float SplashRadius;
        //float RotationSpeed;
        //float RotationRadius;
        //float ActiveDuration;
        //float InActiveDuration;
        //int LifeDrain;

    }

}