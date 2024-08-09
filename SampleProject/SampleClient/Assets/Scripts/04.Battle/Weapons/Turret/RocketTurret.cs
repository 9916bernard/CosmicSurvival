using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTurret : Turret
{
    private float detectionRadius = 10f;
    private float nextFireTime = 0f;
    private Base baseStation;
    private BattleField field; // Assign the BattleField object in the Inspector
    [SerializeField] private Transform rocketSpawnPoints; // Assign the rocket spawn points in the Inspector
    private PlayerController player = null; 
    


    override public void Init(UTBEquipment_Record data, Base baseStation)
    {
        base.Init(data, baseStation);
        this.baseStation = baseStation;
        StartCoroutine(FireRockets());
    }

    public void StopFiring()
    {
        StopAllCoroutines();
    }

    void Update()
    {
        DetectEnemies();
    }

    private void DetectEnemies()
    {

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy") || hit.CompareTag("Boss"))
            {
                if (Time.time >= nextFireTime)
                {
                    RotateTowardsTarget(hit.transform);
                    nextFireTime = Time.time + 1f / _stat.FireRate;
                }
                break;
            }
        }
    }

    private void RotateTowardsTarget(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle- 270));
    }


    private IEnumerator FireRockets()
    {
        while (true)
        {
            for (int i = 0; i < _stat.ProjectileNum; i++)
            {
                Rocket rocket = baseStation.field.GetPooledRocket(_stat.Damage, _stat.ProjectileSpeed);
                if (rocket != null)
                {
                    rocket.transform.position = rocketSpawnPoints.position;
                    rocket.transform.rotation = rocketSpawnPoints.rotation;
                    rocket.gameObject.SetActive(true);
                }
            }

            // Wait for the specified fire rate before firing the next rocket
            yield return new WaitForSeconds(_stat.FireRate);
        }
    }

}

