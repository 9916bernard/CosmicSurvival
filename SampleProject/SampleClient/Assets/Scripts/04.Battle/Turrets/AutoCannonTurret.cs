using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCannonTurret : Turret
{
    public float detectionRadius = 10f;
    [SerializeField] private Transform bulletLocationLeft;
    [SerializeField] private Transform bulletLocationRight;
    private Base baseStation;
    [SerializeField] private GameObject bulletPrefab;
    


    private float nextFireTime = 0f;
    //change to initialize when prefab
    override public void Init(UTBEquipment_Record data, Base baseStation)
    {
        base.Init(data, baseStation);
        this.baseStation = baseStation;
    }

    void Update()
    {
        DetectAndShootEnemies();
    }

    private void DetectAndShootEnemies()
    {
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy") || hit.CompareTag("Boss"))
            {
                if (Time.time >= nextFireTime)
                {
                    RotateTowardsTarget(hit.transform);
                    Shoot();
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
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 270));
    }

    private void Shoot()
    {
        Bullet bulletLeft = baseStation.field.GetPooledBullet(_stat.Damage, _stat.ProjectileSpeed, _stat.Penetration);
        if (bulletLeft != null)
        {
            bulletLeft.transform.position = bulletLocationLeft.position;
            bulletLeft.transform.rotation = bulletLocationLeft.rotation;
            bulletLeft.gameObject.SetActive(true);
        }

        Bullet bulletRight = baseStation.field.GetPooledBullet(_stat.Damage, _stat.ProjectileSpeed, _stat.Penetration);
        if (bulletRight != null)
        {
            bulletRight.transform.position = bulletLocationRight.position;
            bulletRight.transform.rotation = bulletLocationRight.rotation;
            bulletRight.gameObject.SetActive(true);
        }

        //SOUND.Sfx(EUI_SFX.HIT_TEST_2);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
