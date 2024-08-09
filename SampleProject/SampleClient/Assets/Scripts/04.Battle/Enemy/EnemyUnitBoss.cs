using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyUnitBoss : EnemyUnit
{
    private float chargeInterval = 7f;
    private float chargeSpeedMultiplier = 2f;
    private float chargeDurationMax = 1.5f;
    private float currentSpeed;
    private Vector3 chargeTargetPosition;
    private bool isCharging = false;
    [SerializeField] private SpriteRenderer chargeAreaIndicator;
    private int maxHealth;
    private GameObject chargeIndicator; // GameObject for the charge indicator
    private bool isMovementStopped = false; // To track if the movement is stopped

    [HideInInspector] public new ObjectPool<int, EnemyUnitBoss> _pool = null;

    private Color originalColor; // To store the original color

    protected override void Awake()
    {
        base.Awake();
        currentSpeed = speed;
        maxHealth = health;
        originalColor = spriteRenderer.color; // Store the original color
    }

    public override void Init(PlayerController target, BattleManager battleManager, float battleTime, int EnemyId)
    {
        base.Init(target, battleManager, battleTime, EnemyId);
        StartCoroutine(ChargeRoutine());

        if (battleTime > 300)
        {
            StartCoroutine(BlinkAlpha());
        }

        if (battleTime > 900)
        {
            transform.localScale = Vector3.one * 3; // Scale the boss to three times its original size
        }
    }

    private IEnumerator ChargeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(chargeInterval - 1f); // Wait before charging
            if (!isDestroyed)
            {
                // Perform the charge once
                yield return Charge();

                // If battle time is over 600, perform a second charge
                if (_battleManager.battleTime > 600)
                {
                    yield return Charge();
                }
            }
        }
    }

    private IEnumerator Charge()
    {
        chargeTargetPosition = _target.transform.position;

        // Stop moving for 1 second before charging
        StopMovement();

        yield return new WaitForSeconds(1f);

        ResumeMovement();

        // Show the charge area indicator
        Vector3 dashDirection = (chargeTargetPosition - transform.position).normalized;
        Vector3 dashTargetPosition = transform.position + dashDirection * 3f; // Dash a distance of 3 units

        isCharging = true;
        float chargeDuration = 0f; // Charge duration in seconds

        while (chargeDuration < chargeDurationMax)
        {
            transform.position += dashDirection * currentSpeed * chargeSpeedMultiplier * Time.deltaTime;
            chargeDuration += Time.deltaTime;

            // Stop if the boss has reached the dash distance
            if (Vector3.Distance(transform.position, dashTargetPosition) < 0.1f)
                break;

            yield return null;
        }

        isCharging = false;
        ResumeMovement();
    }

    private IEnumerator BlinkAlpha()
    {
        float alpha = 0f;
        bool increasing = true;

        while (true)
        {
            if (increasing)
            {
                alpha += Time.deltaTime * 25; // Adjust the speed as needed
                if (alpha >= 255f)
                {
                    alpha = 255f;
                    increasing = false;
                }
            }
            else
            {
                alpha -= Time.deltaTime * 25; // Adjust the speed as needed
                if (alpha <= 0f)
                {
                    alpha = 0f;
                    increasing = true;
                }
            }

            Color color = spriteRenderer.color;
            color.a = alpha / 255f;
            spriteRenderer.color = color;

            yield return null;
        }
    }

    private void StopMovement()
    {
        isMovementStopped = true;
        currentSpeed = 0f;
    }

    private void ResumeMovement()
    {
        isMovementStopped = false;
        currentSpeed = speed;
    }

    protected override void MoveTowardsTarget()
    {
        if (!isCharging && !isMovementStopped)
        {
            base.MoveTowardsTarget();
        }
    }

    void Update()
    {
        if (_target != null && !isDestroyed)
        {
            MoveTowardsTarget();
        }
    }

    public void OnTest()
    {
        Debug.Log("=========== Boss : OnTest");
    }

    public void OnTest2()
    {
        Debug.Log("=========== Boss : OnTest2");
    }

    public override void OnDestroyed()
    {
        StopAllCoroutines();

        for (int i = 0; i < 6; i++)
        {
            _battleManager.DropExperience(this);
            _battleManager.DropMetal(this);
            _battleManager.DropHp(this);
            _battleManager.DropBlackHole(this);
        }

        // Reset the color to the original color
        spriteRenderer.color = originalColor;

        // Ensure the enemy is pushed back to the pool
        if (_pool != null)
        {
            _pool.Push(this);
        }
        else
        {
            Debug.LogError("EnemyUnitBoss pool is null.");
        }

        Debug.Log("=========== Boss : OnDestroyed");
    }

    public void UpdateColorBasedOnHealth()
    {
        float decrementAmount = 0.01f; // Adjust this value to control how quickly it becomes redder

        Color currentColor = spriteRenderer.color;
        float newGreen = Mathf.Max(0f, currentColor.g - decrementAmount);
        float newBlue = Mathf.Max(0f, currentColor.b - decrementAmount);

        Color targetColor = new Color(
            currentColor.r, // Keep the current red value
            newGreen, // Gradually decrease green
            newBlue, // Gradually decrease blue
            currentColor.a // Maintain the current alpha value
        );

        spriteRenderer.color = targetColor;
    }

    // grade º¸°í ½ºÀ§Äª
    public static EnemyUnitBoss MakeFactory(int InKey, ObjectPool<int, EnemyUnitBoss> InPool)
    {
        var _Enemy = Instantiate(Resources.Load($"Battle/Enemy/EnemyUnit_Boss_{InKey:D3}")).GetComponent<EnemyUnitBoss>();
        _Enemy.SetPool(InPool); // Ensure the pool is set correctly here
        return _Enemy;
    }

    public void SetPool(ObjectPool<int, EnemyUnitBoss> pool)
    {
        _pool = pool; // Ensure this sets the correct pool
    }
}
