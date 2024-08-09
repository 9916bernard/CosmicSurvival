using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{


    private float lifeTime = 5.0f;
    private int currentHitCount = 0;

    private Rigidbody2D rb;

    [HideInInspector] public ObjectPoolSimple<Bullet> _pool = null;
    private int damage;
    private float projectileSpeed;
    private int penetration;

    public void Init(int damage, float projectileSpeed, int penetration)
    {
        rb = GetComponent<Rigidbody2D>();
        gameObject.SetActive(true);

        lifeTime = 5.0f;
        this.damage = damage;
        this.projectileSpeed = projectileSpeed;
        this.penetration = penetration;
        
    }

    void Update()
    {
        rb.velocity = transform.up * projectileSpeed;

        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            if (_pool != null)
                _pool.Push(this);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            currentHitCount += 1;

            var enemy = other.GetComponent<EnemyUnit>();

            // Reduce the enemy's health
            if (enemy != null)
            {
                enemy._battleManager.enemyGetDamage(damage,enemy);
            }

            if (currentHitCount >= penetration)
            {
                if (_pool != null)
                    _pool.Push(this);
            }
        }
    }

    public void SetPool(ObjectPoolSimple<Bullet> pool)
    {
        _pool = pool;
    }

    public static Bullet MakeFactory(ObjectPoolSimple<Bullet> pool)
    {
        var _obj = Resources.Load("Battle/Bullet/BulletUnit_001");
        var _Bullet = Instantiate(_obj).GetComponent<Bullet>();

        if (_Bullet != null)
        {
            _Bullet.SetPool(pool);
        }
        else
        {
            Debug.LogError("Bullet instantiation failed.");
        }

        return _Bullet;
    }
}
