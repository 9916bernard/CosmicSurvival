using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyUnit : MonoBehaviour
{
    public GameObject engineIdle = null;
    [HideInInspector] public float speed;
    [HideInInspector] public int health;
    private float upgradeSpeed;
    private int upgradeHealth;

    [HideInInspector] public PlayerController _target;
    [HideInInspector] public ObjectPool<int, EnemyUnit> _pool = null;
    private Animator animator;
    [HideInInspector] public bool isDestroyed = false;
    [SerializeField] private Collider2D Collider2D = null;
    public SpriteRenderer spriteRenderer = null; // Reference to the sprite renderer

    [HideInInspector] public Color originalColor; // Original color of the sprite

    private float reinforcementInterval; // Reinforcement interval in seconds

    private int enemyLevel;

    [HideInInspector] public BattleManager _battleManager = null;

    //private UTBEnemy_Record _enemyRec = null;
    private int enemyId = 0;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        originalColor = spriteRenderer.color; // Store the original color
        enemyLevel = 0;
    }

    public virtual void Init(PlayerController target, BattleManager battleManager, float battleTime, int EnemyId)
    {
        var enemyStat = TABLE.enemy.Find(EnemyId);
        enemyLevel = (int)(battleTime / enemyStat.UpgradeInterval);

        enemyId = EnemyId;
        
        health = enemyStat.Health + (enemyStat.UpgradeHealth * enemyLevel);
        speed = enemyStat.Speed + (enemyStat.UpgradeSpeed * enemyLevel);
        if (speed >= 3.0f)
        {
            speed = 3.0f;
        }
        gameObject.SetActive(true);
        Collider2D.enabled = true;
        isDestroyed = false;
        _target = target;
        engineIdle.SetActive(true);
        Debug.Log("Current Enemy: Health = " + health + ", Speed = " + speed);
        
        _battleManager = battleManager;
    }

    void Update()
    {
        if (_target != null && !isDestroyed)
        {
            MoveTowardsTarget();
        }
    }

    protected virtual void MoveTowardsTarget()
    {
        Vector3 direction = _target.transform.position - transform.position;
        direction.Normalize();
        transform.position += direction * speed * Time.deltaTime;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 270));
    }

    public void DestroyEnemy()
    {
        engineIdle.SetActive(false);
        Collider2D.enabled = false;
        isDestroyed = true;
        animator.SetTrigger("DestroyTrigger");
        _battleManager._EnemySpawner.OnEnemyDestroyed(this);

    }

    virtual public void OnDestroyed()
    {
        _battleManager.DropExperience(this);
        _battleManager.DropMetal(this);
        _battleManager.DropHp(this);
        _battleManager.DropBlackHole(this);
        giveGold();
        _pool.Push(this);

        Debug.Log("=========== Enemy : OnDestroyed");
    }

    private void giveGold()
    {
        float dropChance = 0.2f; // 1/5 chance, which is 20%

        if (UnityEngine.Random.value < dropChance)
        {
            _battleManager.gold += 10;
        }
        Debug.Log(_battleManager.gold);
    }

    public void SetPool(ObjectPool<int, EnemyUnit> pool)
    {
        _pool = pool;
    }

    public static EnemyUnit MakeFactory(int InKey, ObjectPool<int, EnemyUnit> InPool)
    {
        var _Enemy = Instantiate(Resources.Load($"Battle/Enemy/EnemyUnit_{InKey:D3}")).GetComponent<EnemyUnit>();
        _Enemy.SetPool(InPool);
        return _Enemy;
    }
}
