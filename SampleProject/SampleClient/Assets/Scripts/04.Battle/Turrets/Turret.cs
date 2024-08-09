using System.Collections;
using System.Collections.Generic;
//using TMPro.EditorUtilities;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class Turret : MonoBehaviour
{
    protected Base _baseStation;

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

    protected FTB_WeaponStat _stat = new();

    virtual public void Init(UTBEquipment_Record data, Base baseStation)
    {
        _level = 1;

        _stat = data.WeaponStat;

        _baseStation = baseStation;

        _stat.Damage += (int)USER.upgrade.GetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.STATION_ATK);
        _stat.FireRate += (USER.upgrade.GetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.STATION_FIRERT) * TABLE.upgrade.Find(2001).UpgradeAmount);
        _stat.ProjectileNum += (int)USER.upgrade.GetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.STATION_PRJNUM);

    }

    virtual public void Upgrade(int turretID)
    {
        var upgrade = TABLE.equipment.Find(turretID).WeaponUpgrade[_level];

        //for (int i = _level; i < TokenImpersonationLevel; ++i)
        _stat.Add(upgrade);

        _level++;


    }
}
