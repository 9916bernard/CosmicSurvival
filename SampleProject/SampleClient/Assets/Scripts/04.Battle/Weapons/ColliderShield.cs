using System.Collections;
using UnityEngine;

public class ColliderShield : Weapon
{

    [SerializeField] private BattleManager battleManager = null;

    private Vector3[] relativePositions;
    private Quaternion[] rotations;
    private int currentIndex = 0;
    private Collider2D shieldCollider;
    private SpriteRenderer spriteRenderer;

    private enum ShieldState { Active, Inactive }
    private ShieldState currentState;
    private float stateTimer;

    override public void Init(UTBEquipment_Record data, PlayerController player)
    {

        base.Init(data, player);
        relativePositions = new Vector3[]
        {
            new Vector3(0, 0.2f, 0), // Top
            new Vector3(-0.2f, 0, 0), // Left
            new Vector3(0, -0.2f, 0), // Bottom
            new Vector3(0.2f, 0, 0) // Right
        };

        rotations = new Quaternion[]
        {
            Quaternion.Euler(0, 0, 0), // Top
            Quaternion.Euler(0, 0, 90), // Left
            Quaternion.Euler(0, 0, 180), // Bottom
            Quaternion.Euler(0, 0, -90) // Right
        };

        shieldCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        activeDutation = _stat.ActiveDuration;
        inActiveDuration = _stat.InActiveDuration;

        SetState(ShieldState.Active);
    }

    void Update()
    {
        activeDutation = _stat.ActiveDuration;
        inActiveDuration = _stat.InActiveDuration;
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            if (currentState == ShieldState.Active)
            {
                SetState(ShieldState.Inactive);
            }
            else
            {
                currentIndex = (currentIndex + 1) % relativePositions.Length;
                SetState(ShieldState.Active);
            }
        }
        if (_player != null)
        {
            Vector3 targetPosition = _player.transform.position + relativePositions[currentIndex];
            transform.position = targetPosition;
            transform.rotation = rotations[currentIndex];
        }
    }

    private void SetState(ShieldState newState)
    {
        currentState = newState;
        if (newState == ShieldState.Active)
        {
            shieldCollider.enabled = true;
            spriteRenderer.enabled = true;
            stateTimer = activeDutation;
        }
        else
        {
            shieldCollider.enabled = false;
            spriteRenderer.enabled = false;
            stateTimer = inActiveDuration;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
        {
            EnemyUnit enemyUnit = collision.GetComponent<EnemyUnit>();
            if (enemyUnit != null)
            {
                enemyUnit._battleManager.enemyGetDamage(_stat.Damage, enemyUnit);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
        {
            EnemyUnit enemyUnit = collision.GetComponent<EnemyUnit>();
            if (collision.tag == "Boss")
            {
                StayEnemyDamage(enemyUnit);
            }
            else
            {
                enemyUnit._battleManager.enemyGetDamage(_stat.Damage, enemyUnit);
            }

        }
    }

    IEnumerable StayEnemyDamage(EnemyUnit enemyUnit)
    {

        if (enemyUnit != null)
        {
            enemyUnit._battleManager.enemyGetDamage(_stat.Damage, enemyUnit);
        }
        yield return new WaitForSeconds(0.5f);
               
    }
}
