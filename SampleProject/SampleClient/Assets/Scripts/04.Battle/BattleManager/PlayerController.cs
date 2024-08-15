using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    [HideInInspector] public BattleManager battleManager = null;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public Collider2D coll;
    [HideInInspector] public FingerStick fingerStick;
    private bool isDashing = false;
    private bool isFingerStickEnabled = true;
    [SerializeField] private GameObject Booster_Effect;

    // New GameObjects for different states
    public GameObject baseEngineIdle;
    public GameObject baseEngineBoost;

    // Bullet prefab and spawn points
    public Transform leftWingSpawn;
    public Transform rightWingSpawn;

    // Player stats
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public int health;
    [HideInInspector] public int maxHealth;

    private const int MaxWeaponCount = 5;
    private int currentWeaponCount = 1; // AutoCannon is initially active
    public bool isInventoryFull => currentWeaponCount >= MaxWeaponCount;

    [HideInInspector] public bool isAutoCannonActive;
    private bool isAutoCannonFirstActive = true;

    [HideInInspector] public bool isLaserCannonActive;
    private bool isLaserCannonFirstActive = true;

    [HideInInspector] public bool isCollideDroneActive;
    private bool isCollideDroneFirstActive = true;

    [HideInInspector] public bool isRocketActive;
    private bool isRocketFirstActive = true;

    [HideInInspector] public bool isCollideShieldActive;
    private bool isColliderShieldFirstActive = true;

    [HideInInspector] public bool isRangedDroneActive;
    private bool isRangedDroneFirstActive = true;

    [HideInInspector] public bool isSlashSwordActive;
    private bool isSlashSwordFirstActive = true;

    private RocketLauncher _rocketSpawner;
    private BulletLauncher _bulletSpawner;

    // Invincibility flag
    private bool isInvincible = false;
    [HideInInspector] public float boosterGage = 100;
    [HideInInspector] public float maxBoosterGage = 100;
    private UTBUpgrade _Rec = null;

    public void Init(BattleManager battleManager, RocketLauncher rocketSpawner, BulletLauncher bulletSpawner)
    {
        rb = GetComponent<Rigidbody2D>();
        baseEngineIdle.SetActive(true);
        baseEngineBoost.SetActive(false);

        _rocketSpawner = rocketSpawner;
        _bulletSpawner = bulletSpawner;
        setInitialActiveState();

        if (battleManager != null)
        {
            this.battleManager = battleManager;
        }
        _Rec = TABLE.upgrade;
        GetPlayerStat();
        this.AddWeapon(1008);



    }

    private void setInitialActiveState()
    {
        isAutoCannonActive = true;
        isLaserCannonActive = false;
        isSlashSwordActive = false;
        isRangedDroneActive = false;
        isCollideDroneActive = false;
        isCollideShieldActive = false;
        isRocketActive = false;

        isAutoCannonFirstActive = true;
        isLaserCannonFirstActive = true;
        isSlashSwordFirstActive = true;
        isRangedDroneFirstActive = true;
        isCollideDroneFirstActive = true;
        isColliderShieldFirstActive = true;
        isRocketFirstActive = true;
    }

    private void GetPlayerStat()
    {
        var charinfo = TABLE.character.Find(1001);

        if (charinfo != null)
        {
            moveSpeed = charinfo.Speed + (_Rec.Find(1002).UpgradeAmount * USER.upgrade.GetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.PLAYER_SPD));
            health = charinfo.Health + (int)(_Rec.Find(1001).UpgradeAmount * USER.upgrade.GetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.PLAYER_HP));
            maxHealth = charinfo.MaxHealth + (int)(_Rec.Find(1001).UpgradeAmount * USER.upgrade.GetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.PLAYER_HP));
        }
    }

    void Update()
    {
        if(boosterGage < maxBoosterGage)
        {
            boosterGage += 0.15f * Time.timeScale;
            battleManager.BoostBar.SetBoost((int)boosterGage);
        }
        

        if (isFingerStickEnabled && fingerStick != null)
        {
            moveInput = fingerStick.GetDirection();
        }

        if (moveInput != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            rb.rotation = angle + 270;
        }
    }

    void FixedUpdate()
    {
        if (isFingerStickEnabled)
        {
            rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isInvincible && (other.CompareTag("Enemy") || other.CompareTag("Boss")))
        {
            if (gameObject.CompareTag("Player"))
            {
                battleManager.getDamage(1);
                SetInvincible();
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!isInvincible && other.CompareTag("Boss"))
        {
            if (gameObject.CompareTag("Player"))
            {
                battleManager.getDamage(1);
                SetInvincible();
            }
        }
    }

    public void SetInvincible()
    {
        StartCoroutine(Invincible());
    }

    public IEnumerator Invincible()
    {
        isInvincible = true;
        // You can add visual feedback for invincibility here (e.g., blinking effect)
        yield return new WaitForSeconds(1.5f);
        isInvincible = false;
    }

    public void activateDash()
    {
        if (!isDashing && boosterGage >= 100)
        {
           
            StartCoroutine(DashCoroutine());
            SetInvincible();
            boosterGage = 0;
            battleManager.BoostBar.SetBoost((int)boosterGage);
        }
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        Booster_Effect.SetActive(true);
        float originalSpeed = moveSpeed;
        moveSpeed *= 2;
        baseEngineIdle.SetActive(false);
        baseEngineBoost.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        moveSpeed = originalSpeed;
        isDashing = false;
        baseEngineIdle.SetActive(true);
        baseEngineBoost.SetActive(false);
        Booster_Effect.SetActive(false);
    }

    public void ResetPlayerStats()
    {
        // Destroy all weapon instances
        var weaponInstances = battleManager.weapons.GetComponentsInChildren<Weapon>();
        foreach (var weapon in weaponInstances)
        {
            Destroy(weapon.gameObject);
        }

        _dicWeapon.Clear();

        currentWeaponCount = 1;

       
    }
}
