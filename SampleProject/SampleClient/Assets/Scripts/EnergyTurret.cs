using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyTurret : Turret
{
    [SerializeField] private SpriteRenderer shieldRenderer; // Reference to the SpriteRenderer for the shield
    private Base baseStation;
    private Coroutine blinkCoroutine;
    private Dictionary<EnemyUnit, float> enemyCooldowns = new Dictionary<EnemyUnit, float>();
    private float cooldownTime = 2f; // Cooldown time for hitting an enemy

    override public void Init(UTBEquipment_Record data, Base baseStation)
    {
        base.Init(data, baseStation);
        this.baseStation = baseStation;
    }

   

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
        {
            var enemy = collision.GetComponent<EnemyUnit>();
            if (enemy != null && CanHitEnemy(enemy))
            {
                HitEnemy(enemy);
            }
        }
    }

    private void HitEnemy(EnemyUnit enemy)
    {
        enemy._battleManager.enemyGetDamage(_stat.Damage, enemy);
        StartBlinking();
        enemyCooldowns[enemy] = Time.time + cooldownTime;
    }

    private bool CanHitEnemy(EnemyUnit enemy)
    {
        return !enemyCooldowns.ContainsKey(enemy) || enemyCooldowns[enemy] <= Time.time;
    }

    private void StartBlinking()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        blinkCoroutine = StartCoroutine(BlinkShield());
    }

    private IEnumerator BlinkShield()
    {
        float duration = 1f; // Duration of the blink
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(74f / 255f, 200f / 255f, elapsedTime / duration);
            SetShieldAlpha(alpha);
            yield return null;
        }

        SetShieldAlpha(74f / 255f); // Reset alpha to the initial value after blinking
    }

    private void SetShieldAlpha(float alpha)
    {
        Color color = shieldRenderer.color;
        color.a = alpha;
        shieldRenderer.color = color;
    }

    
}
