using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class LaserCannonTurret : Turret
{
    [HideInInspector] public float detectionRadius = 10f;
    [SerializeField] private Transform laserStartPoint;
    [SerializeField] private GameObject laserPrefab;
    private List<GameObject> enemiesInRange = new List<GameObject>();
    private Base baseStation;
    private float nextFireTime = 0f;

    override public void Init(UTBEquipment_Record data, Base baseStation)
    {
        base.Init(data, baseStation);
        this.baseStation = baseStation;
    }

    void Update()
    {
        if (enemiesInRange.Count > 0 && Time.time >= nextFireTime)
        {
            List<GameObject> enemiesToTarget = new List<GameObject>(enemiesInRange);
            for (int i = 0; i < _stat.ProjectileNum; i++)
            {
                if (enemiesToTarget.Count == 0)
                    break;

                int randomIndex = Random.Range(0, enemiesToTarget.Count);
                GameObject targetEnemy = enemiesToTarget[randomIndex];
                enemiesToTarget.RemoveAt(randomIndex);

                FireLaser(targetEnemy);
            }

            nextFireTime = Time.time + _stat.FireRate;
        }

    }

    private void RotateTowardsTarget(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 270));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
        {
            if (!enemiesInRange.Contains(collision.gameObject))
            {
                enemiesInRange.Add(collision.gameObject);
                
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
        {
            if (enemiesInRange.Contains(collision.gameObject))
            {
                enemiesInRange.Remove(collision.gameObject);
            }
        }
    }

    private void FireLaser(GameObject targetEnemy)
    {
        if (targetEnemy.activeInHierarchy == true && targetEnemy != null)
        {
            StartCoroutine(LaserRoutine(targetEnemy.transform));
        }
    }

    private IEnumerator LaserRoutine(Transform target)
    {
        RotateTowardsTarget(target.transform);
        // Instantiate the laser sprite
        GameObject laser = Instantiate(laserPrefab, laserStartPoint.position, Quaternion.identity);
        Vector3 direction = (target.position - laserStartPoint.position).normalized;
        float distance = Vector3.Distance(laserStartPoint.position, target.position);

        // Set the laser sprite to stretch from the start point to the target
        laser.transform.position = laserStartPoint.position + direction * distance / 2; // Position the laser halfway
        laser.transform.up = direction; // Rotate the laser to point towards the target
        laser.transform.localScale = new Vector2(laser.transform.localScale.x, distance * 3f); // Stretch the laser

        // Damage the enemy
        EnemyUnit enemyUnit = target.GetComponent<EnemyUnit>();
        if (enemyUnit != null)
        {
            enemyUnit._battleManager.enemyGetDamage(_stat.Damage, enemyUnit);
        }

        yield return new WaitForSeconds(0.5f); // Laser stays for 0.5 seconds

        // Destroy the laser sprite
        Destroy(laser);
    }
}
