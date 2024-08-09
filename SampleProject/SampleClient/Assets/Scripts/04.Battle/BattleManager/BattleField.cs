using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public partial class BattleField : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager = null;
    [SerializeField] private Camera _Camera = null;
    [SerializeField] private Canvas _Canvas = null;
    [SerializeField] private Transform _RootTrans = null;
    [SerializeField] private GameObject _Unit = null;
    [SerializeField] private RectTransform _Point = null;
    [HideInInspector] public PlayerController _Player = null;
    [HideInInspector] public GameObject playerObject = null;

    private ObjectDictionaryPool<int, EnemyUnit> _Pool_Enemy = new();
    private ObjectDictionaryPool<int, EnemyUnitBoss> _Pool_Enemy_Boss = new();
    private ObjectPoolSimple<Bullet> _Pool_Bullet = new();
    private ObjectPoolSimple<Exp> _Pool_Exp = new();
    public ObjectPoolSimple<Rocket> _Pool_Rocket = new();
    private ObjectPoolSimple<Metal> _Pool_Metal = new();
    private ObjectPoolSimple<DamageText> _Pool_DamageText = new();
    private ObjectDictionaryPool<int, Astroid> _Pool_Astroid = new();
    private ObjectPoolSimple<HealPickUp> _Pool_HealPickUp = new();
    private ObjectPoolSimple<ShockWave> _Pool_ShockWave = new();

    private RectTransform _CanvasRectTrans = null;

    private ObjectPool<int, DamageText> _DamagePool = new();

    [HideInInspector] public int Damage;
    [HideInInspector] public float BulletSpeed;
    [HideInInspector] public int BulletHitCount;

    [HideInInspector] public int RocketDamage;
    [HideInInspector] public float RocketSpeed;

    public Base baseStation = null;
    private AstroidSpawner astroidSpawner;

    void Start()
    {
        if (_CanvasRectTrans == null)
        {
            _CanvasRectTrans = _Canvas.GetComponent<RectTransform>();
        }
        astroidSpawner = GetComponent<AstroidSpawner>();
        
    }

    public void Init()
    {
        _Pool_Enemy_Boss.Init(transform, EnemyUnitBoss.MakeFactory);
        _Pool_Enemy.Init(transform, EnemyUnit.MakeFactory);
        _Pool_Bullet.Init(transform, Bullet.MakeFactory, 10);
        _Pool_Exp.Init(transform, Exp.MakeFactory, 10);
        _Pool_Rocket.Init(transform, Rocket.MakeFactory, 5);
        _Pool_Metal.Init(transform, Metal.MakeFactory, 10);
        _Pool_DamageText.Init(transform, DamageText.MakeFactory);
        _Pool_Astroid.Init(transform, Astroid.MakeFactory);
        _Pool_HealPickUp.Init(transform, HealPickUp.MakeFactory, 10);
        _Pool_ShockWave.Init(transform, ShockWave.MakeFactory, 10);

    }

    public void PlayerInst()
    {
        var _obj = Resources.Load("Battle/Player/Unit_Main_Char");
        _Player = Instantiate(_obj, _RootTrans).GetComponent<PlayerController>();
        playerObject = _Player.gameObject;
    }

    public EnemyUnit GetPooledEnemy(int enemyId)
    {
        var enemy = _Pool_Enemy.Pop(enemyId);
        enemy.Init(_Player, battleManager, battleManager.battleTime, enemyId);
        return enemy;
    }

    public EnemyUnitBoss GetPooledEnemyBoss(int enemyBossId)
    {
        var enemy = _Pool_Enemy_Boss.Pop(enemyBossId);
        enemy.Init(_Player, battleManager, battleManager.battleTime, enemyBossId);
        return enemy;
    }

    public Exp GetPooledExp()
    {
        var exp = _Pool_Exp.Pop();
        exp.Init(battleManager);
        return exp;
    }

    public Bullet GetPooledBullet(int damage, float projectileSpeed, int penetration)
    {
        var bullet = _Pool_Bullet.Pop();
        bullet.Init(damage, projectileSpeed, penetration);
        return bullet;
    }

    public HealPickUp GetPooledHealPickUp()
    {
        var healPickUp = _Pool_HealPickUp.Pop();
        healPickUp.Init(battleManager);
        return healPickUp;
    }

    public ShockWave GetPooledShockWave(int damage, float speed, float rotationSpeed, float duration, Vector3 direction)
    {
        var shockWave = _Pool_ShockWave.Pop();
        shockWave.Init(damage, speed, rotationSpeed, duration, direction);
        return shockWave;
    }

    public void ReturnPooledShockWave(ShockWave shockWave)
    {
        _Pool_ShockWave.Push(shockWave);
    }

    public void ReturnPooledHealPickUp(HealPickUp healPickUp)
    {
        _Pool_HealPickUp.Push(healPickUp);
    }

    public void ReturnPooledBullet(Bullet bullet)
    {
        _Pool_Bullet.Push(bullet);
    }

    public void ReturnPooledExp(Exp exp)
    {
        _Pool_Exp.Push(exp);
    }

    public Rocket GetPooledRocket(int damage, float projectileSpeed)
    {
        var rocket = _Pool_Rocket.Pop();
        rocket.Init(damage, projectileSpeed);
        return rocket;
    }

    public void ReturnPooledRocket(Rocket rocket)
    {
        _Pool_Rocket.Push(rocket);
    }

    public Metal GetPooledMetal()
    {
        var metal = _Pool_Metal.Pop();
        metal.Init(battleManager);
        return metal;
    }

    public void ReturnPooledMetal(Metal metal)
    {
        _Pool_Metal.Push(metal);
    }

    public DamageText GetPooledDamageText()
    {
        var poolUnit = _Pool_DamageText.Pop();
        return poolUnit;
    }

    public Astroid GetPooledAstroid(int astroidId)
    {
        var astroid = _Pool_Astroid.Pop(astroidId);
        astroid.SetSpawner(astroidSpawner);
        return astroid;
    }

    public void ReturnPooledAstroid(int type,Astroid astroid)
    {
        _Pool_Astroid.Push(type,astroid);
        astroidSpawner.RespawnAstroid(astroid);
    }

    public void OnClick_FieldClick()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_CanvasRectTrans, _Unit.transform.position, null, out var canvasPos);
        _Point.anchoredPosition = canvasPos;
    }
}
