using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{


    [HideInInspector] public Dictionary<int, Weapon> _dicWeapon = new();

    public void AddWeapon(int InWeaponID)
    {
        var data = TABLE.equipment.Find(InWeaponID);
 
        _dicWeapon.TryGetValue(InWeaponID, out Weapon weapon);
        

        if (weapon != null)
        {
            weapon.Upgrade(InWeaponID);
            return;
        }

       

        if (data == null)
        {
            Debug.LogError("Weapon Data is Null");
            return;
        }

        var _obj = Resources.Load($"Battle/Weapon/{data.PrefabName}");

        switch (InWeaponID)
        {
            case 1001: weapon = Instantiate(_obj, battleManager.weapons.transform).GetComponent<BulletLauncher>(); break;
            case 1002: weapon = Instantiate(_obj, battleManager.weapons.transform).GetComponent<LaserCannon>(); break;
            case 1003: weapon = Instantiate(_obj, battleManager.weapons.transform).GetComponent<CollideDrone>(); break;
            case 1004: weapon = Instantiate(_obj, battleManager.weapons.transform).GetComponent<RocketLauncher>(); break;
            case 1005: weapon = Instantiate(_obj, battleManager.weapons.transform).GetComponent<ColliderShield>(); break;
            case 1006: weapon = Instantiate(_obj, battleManager.weapons.transform).GetComponent<RangeDrone>(); break;
            case 1007: weapon = Instantiate(_obj, battleManager.weapons.transform).GetComponent<SlashSword_Wp>(); break;
            case 1008: weapon = Instantiate(_obj, battleManager.weapons.transform).GetComponent<ShockWaveLauncher>(); break;
        }

        if (weapon == null)
        {
            Debug.LogError("Weapon is Null");
            return;
        }

        weapon.Init(data, this);

        _dicWeapon.Add(InWeaponID, weapon);
    }

    public bool InventoryMaxed()
    {
       
        if(_dicWeapon.Count > 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool AllInventoryWeaponsMaxed()
    {

        foreach(var weapon in _dicWeapon) {
            if (weapon.Value._level < 10)
            {
                return false;
            }
        }
        return true;
    }


    public List<int> GetUpgradableWeaponsWhenFull()
    {
        List<int> upgradableWeapons = new List<int>();

        
            foreach (var weapon in _dicWeapon)
            {
                if (weapon.Value._level < 10)
                {
                    upgradableWeapons.Add(weapon.Key);
                }
            }
        
        

        return upgradableWeapons;
    }
}
