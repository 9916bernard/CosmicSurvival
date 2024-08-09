using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class Weapon : MonoBehaviour
{
    protected PlayerController _player;

    public int _level = 1;

    [HideInInspector] public int damage;
    [HideInInspector] public float fireRate;
    [HideInInspector] public int penetration;
    [HideInInspector] public int projectileNum;
    [HideInInspector] public float projectileSpeed;
    [HideInInspector] public float splashRadius;
    [HideInInspector] public float rotationSpeed;
    [HideInInspector] public float rotationRadius;
    [HideInInspector] public float activeDutation;
    [HideInInspector] public float inActiveDuration;
    [HideInInspector] public int lifeDrain;

    protected FTB_WeaponStat _stat = new();
    private FTB_WeaponUpgrade upgrade;


    virtual public void Init(UTBEquipment_Record data, PlayerController player)
    {
        var upgrade = TABLE.upgrade;

        _player = player;

        _level = 1;

        _stat = data.WeaponStat;

        _stat.Damage += (int)USER.upgrade.GetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.PLAYER_ATK);
        _stat.FireRate += (USER.upgrade.GetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.PLAYER_FIRERATE) * upgrade.Find(1003).UpgradeAmount);
        _stat.ActiveDuration += (USER.upgrade.GetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.PLAYER_DURATION) * upgrade.Find(1004).UpgradeAmount);
        _stat.ProjectileNum += (int)USER.upgrade.GetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.PLAYER_PRJNUM);

        ////_stat.Add(_stat);

        //FTB_WeaponStat a = new();
        //upgrade = TABLE.equipment.Find(1001).WeaponUpgrade[1];
  
        //_stat.Add(upgrade);

        //data.WeaponStat.Add(upgrade);

    
    }

    virtual public void Upgrade(int weaponID)
    {
        upgrade = TABLE.equipment.Find(weaponID).WeaponUpgrade[_level];
        

        //for (int i = _level; i < TokenImpersonationLevel; ++i)

        _stat.Add(upgrade);

        _level++;
       
     
    }
     

}
