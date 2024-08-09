using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float currentSpeed;
    public int currentDamage;
    public float detectionRadius = 5f; // Radius within which the rocket searches for enemies
    private float lifeTime = 5.0f;

    private Rigidbody2D rb;
    [HideInInspector] public ObjectPoolSimple<Rocket> _pool = null;
    private Transform target;

    private Vector2 currentDirection;

    public void Init(int Damage, float ProjectileSpeed)
    {
        rb = GetComponent<Rigidbody2D>();
        gameObject.SetActive(true);
        lifeTime = 5.0f;
        target = FindNearestEnemy();
        currentSpeed = ProjectileSpeed;
        currentDamage = Damage;
        currentDirection = transform.up;
    }

    void Update()
    {
        if (target != null)
        {
            Vector2 targetDirection = (target.position - transform.position).normalized;
            currentDirection = Vector2.Lerp(currentDirection, targetDirection, Time.deltaTime).normalized;
        }

        ApplyRandomNoise();

        rb.velocity = currentDirection * currentSpeed;
        float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 270));

        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            if (_pool != null)
                _pool.Push(this);
        }
    }

    void ApplyRandomNoise()
    {
        float noiseFactor = 0.2f; // Adjust this to control the amount of noise
        currentDirection += new Vector2(Random.Range(-noiseFactor, noiseFactor), Random.Range(-noiseFactor, noiseFactor));
        currentDirection.Normalize();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            var enemy = other.GetComponent<EnemyUnit>();

            if (enemy != null)
            {
                enemy._battleManager.enemyGetDamage(currentDamage, enemy);
                if (_pool != null)
                    _pool.Push(this);
            }
        }
    }

    private Transform FindNearestEnemy()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        Transform nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(transform.position, hitCollider.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestEnemy = hitCollider.transform;
                }
            }
        }

        return nearestEnemy;
    }

    public void SetPool(ObjectPoolSimple<Rocket> pool)
    {
        _pool = pool;
    }

    public static Rocket MakeFactory(ObjectPoolSimple<Rocket> pool)
    {
        var _obj = Resources.Load("Battle/Bullet/Rocket");
        var _Rocket = Instantiate(_obj).GetComponent<Rocket>();

        if (_Rocket != null)
        {
            _Rocket.SetPool(pool);
        }
        else
        {
            Debug.LogError("Rocket instantiation failed.");
        }

        return _Rocket;
    }
}
