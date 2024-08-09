using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Weapon
{
    private BattleField field; // Assign the BattleField object in the Inspector
    private Transform[] rocketSpawnPoints; // Assign the rocket spawn points in the Inspector
    private BattleManager battleManager = null; // Reference to the BattleManager script
    private PlayerController player = null;


    override public void Init(UTBEquipment_Record data, PlayerController player)
    {
        base.Init(data, player);
        
        battleManager = player.battleManager;
        field = battleManager._Field;
        this.player = player;
        StartCoroutine(FireRockets());
    }

    public void StopFiring()
    {
        StopAllCoroutines();
    }



    private IEnumerator FireRockets()
    {
        while (true)
        {
            for(int i = 0; i < _stat.ProjectileNum; i++)
            {
                Rocket rocket = field.GetPooledRocket(_stat.Damage, _stat.ProjectileSpeed);
                if (rocket != null)
                {
                    rocket.transform.position = player.leftWingSpawn.position;
                    rocket.transform.rotation = player.leftWingSpawn.rotation;
                    rocket.gameObject.SetActive(true);

                }
                yield return new WaitForSeconds(0.1f);
            }

            // Wait for the specified fire rate before firing the next rocket
            yield return new WaitForSeconds(_stat.FireRate);
        }
    }
}
