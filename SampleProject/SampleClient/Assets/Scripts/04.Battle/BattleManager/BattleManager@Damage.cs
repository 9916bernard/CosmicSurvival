using Cysharp.Threading.Tasks.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public partial class BattleManager : MonoBehaviour
{
    public void getDamage(int damage)
    {
        _Field._Player.health -= damage;
        UIM.ShowOverlay("ui_battle_damage_alert", EUI_LoadType.BATTLE);
        hpBar.SetHp(_Field._Player.health);

        if (_Field._Player.health <= 0)
        {
            Action exitAction = ExitAction;
            UIM.ShowPopup("ui_battle_game_over", EUI_LoadType.BATTLE, new() { { "ExitAction", exitAction } });
        }
        else
        {
            Time.timeScale = 1.0f;
            isTimeFast = false;
        }

        

        SOUND.Sfx(EUI_SFX.DAMAGE_PLAYER);

    }

    public void RestorePlayerHealth()
    {
        var player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.health = player.maxHealth;
            hpBar.SetHp(player.health);
            Debug.Log("Player health restored to full");
        }
    }

    public void spawnDamageText(Vector3 InPos, long InDamage)
    {
        var damageText = _Field.GetPooledDamageText();
        damageText.Play(InPos + damagePoistionOffset, new(InDamage, 0, false));
    }

    public void enemyGetDamage(int damage, EnemyUnit enemy, float knockbackDistance = -1f)
    {
        if (enemy.health <= 0)
        {
            return;
        }

        spawnDamageText(enemy.transform.position, damage);
        enemy.health -= damage;

        if (enemy.health <= 0 && !enemy.isDestroyed)
        {
            enemy.DestroyEnemy();
        }

        if (enemy.tag == "Boss")
        {
            enemy.GetComponentInChildren<EnemyUnitBoss>().UpdateColorBasedOnHealth();
            return;
        }
        StartCoroutine(BlinkEffect(enemy));

        if (knockbackDistance < 0f)
        {
            knockbackDistance = defaultKnockbackDistance;
        }

        StartCoroutine(Knockback(knockbackDistance, enemy));
        SOUND.Sfx(EUI_SFX.ENEMY_DEAD);
    }

    private IEnumerator BlinkEffect(EnemyUnit enemy)
    {
        for (int i = 0; i < blinkCount; i++)
        {
            enemy.spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(blinkDuration);
            enemy.spriteRenderer.color = enemy.originalColor;
            yield return new WaitForSeconds(blinkDuration);
        }
    }

    private IEnumerator Knockback(float knockbackDistance, EnemyUnit enemy)
    {
        if (enemy._target != null)
        {
            Vector3 knockbackDirection = (enemy.transform.position - enemy._target.transform.position).normalized;
            Vector3 startPosition = enemy.transform.position;
            Vector3 targetPosition = enemy.transform.position + knockbackDirection * knockbackDistance;
            float elapsedTime = 0f;

            while (elapsedTime < knockbackDuration)
            {
                enemy.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / knockbackDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            enemy.transform.position = targetPosition;
        }
    }

}
