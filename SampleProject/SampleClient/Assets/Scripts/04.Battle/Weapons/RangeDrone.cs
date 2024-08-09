using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDrone : Weapon
{
    public GameObject damageArea; // The child object that deals damage to enemies
    [HideInInspector] public float shootDistance = 2f; // Distance the drone flies forward
    [HideInInspector] public float shootSpeed = 5f; // Speed at which the drone shoots forward
    [SerializeField] private BattleManager battleManager = null;

    private Vector3 initialPosition;
    private bool isShooting = false;
    private float stateTimer = 0f;

    private enum DroneState { Inactive, Shooting, Active, Returning }
    private DroneState currentState;

    private List<Collider2D> enemiesInRange = new List<Collider2D>(); // List to keep track of enemies in range
    private List<Collider2D> enemiesToRemove = new List<Collider2D>(); // List to keep track of enemies to remove

    private float floatingSpeed = 2f; // Speed of the floating motion
    private float floatingAmplitude = 0.1f; // Amplitude of the floating motion

    override public void Init(UTBEquipment_Record data, PlayerController player)
    {
        base.Init(data, player);
        initialPosition = transform.localPosition;
        damageArea.SetActive(false);
        SetState(DroneState.Inactive);
    }

    void Update()
    {
        stateTimer -= Time.deltaTime;

        if (currentState == DroneState.Inactive)
        {
            float scaleOffset = Mathf.Sin(Time.time * floatingSpeed) * floatingAmplitude;
            transform.localScale = Vector3.one * (0.5f + scaleOffset);
        }
        else
        {
            transform.localScale = Vector3.one * 0.5f;
        }

        switch (currentState)
        {
            case DroneState.Inactive:
                if (stateTimer <= 0f)
                {
                    SetState(DroneState.Shooting);
                }
                else
                {
                    transform.position = _player.transform.position + initialPosition;
                }
                break;
            case DroneState.Shooting:
                MoveTowards(_player.transform.position + _player.transform.up * shootDistance, () =>
                {
                    SetState(DroneState.Active);
                });
                break;
            case DroneState.Active:
                if (stateTimer <= 0f)
                {
                    SetState(DroneState.Returning);
                }
                break;
            case DroneState.Returning:
                MoveTowards(_player.transform.position + initialPosition, () =>
                {
                    SetState(DroneState.Inactive);
                });
                break;
        }
    }

    private void SetState(DroneState newState)
    {
        currentState = newState;
        switch (newState)
        {
            case DroneState.Inactive:
                isShooting = false;
                stateTimer = _stat.InActiveDuration;
                damageArea.SetActive(false);
                enemiesInRange.Clear(); // Clear the list of enemies when inactive
                break;
            case DroneState.Shooting:
                isShooting = true;
                break;
            case DroneState.Active:
                isShooting = false;
                stateTimer = _stat.ActiveDuration;
                damageArea.SetActive(true);
                break;
            case DroneState.Returning:
                isShooting = true;
                damageArea.SetActive(false);
                break;
        }
    }

    private void MoveTowards(Vector3 targetPosition, System.Action onComplete)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, shootSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            transform.position = targetPosition;
            onComplete?.Invoke();
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
                StartCoroutine(StayEnemyDamage(enemyUnit));
            }
            else
            {
                enemyUnit._battleManager.enemyGetDamage(_stat.Damage, enemyUnit);
            }
        }
    }

    private IEnumerator StayEnemyDamage(EnemyUnit enemyUnit)
    {
        while (enemyUnit != null && enemiesInRange.Contains(enemyUnit.GetComponent<Collider2D>()))
        {
            enemyUnit._battleManager.enemyGetDamage(_stat.Damage, enemyUnit);
            yield return new WaitForSeconds(1f); // Damage interval
        }
    }
}
