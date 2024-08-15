using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLauncher : Weapon
{
    private BattleField field; // Assign the Field object in the Inspector
    //public WeaponData weaponData; // Assign the WeaponData object in the Inspector
    private Transform[] bulletSpawnPoints; // Assign the bullet spawn points in the Inspector
    private BattleManager battleManager = null; // Reference to the BattleManager script

    private Coroutine fireCoroutine;

    override public void Init(UTBEquipment_Record data, PlayerController player)
    {
        base.Init(data, player);
        battleManager = player.battleManager;
        field = battleManager._Field;
        fireCoroutine = StartCoroutine(FireBullets());
        
    }

    public override void Upgrade(int weaponID)
    {
        base.Upgrade(weaponID);
        // Restart the coroutine to apply the new stats
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
        }
        fireCoroutine = StartCoroutine(FireBullets());
    }


    public void StopFiring()
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }
    }

    private IEnumerator FireBullets()
    {
        if (_stat.FireRate < 0.1f)
        {
            _stat.Damage += 5;
            _stat.Penetration += 2;
            _stat.FireRate = 0.1f;

        }
        while (true)
        {
            
            Bullet bullet = field.GetPooledBullet(_stat.Damage, _stat.ProjectileSpeed, _stat.Penetration);
            if (bullet != null)
            {
                bullet.transform.position = battleManager._Field._Player.leftWingSpawn.position;
                bullet.transform.rotation = battleManager._Field._Player.leftWingSpawn.rotation;
                bullet.gameObject.SetActive(true);
            }

            Bullet bullet2 = field.GetPooledBullet(_stat.Damage, _stat.ProjectileSpeed, _stat.Penetration);
            if (bullet2 != null)
            {
                bullet2.transform.position = battleManager._Field._Player.rightWingSpawn.position;
                bullet2.transform.rotation = battleManager._Field._Player.rightWingSpawn.rotation;
                bullet2.gameObject.SetActive(true);
            }

            //SOUND.Sfx(EUI_SFX.HIT_TEST_2);
            

            // Wait for the specified fire rate before firing the next bullet
            yield return new WaitForSeconds(_stat.FireRate);
        }
    }
}
