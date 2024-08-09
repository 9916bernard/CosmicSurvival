using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlashSword_Wp : Weapon
{
    public Animator animator; // Reference to the Animator component
    private float offsetDistance = 1f; // Distance ahead of the player where the slash occurs
    private float slashDuration = 0.2f; // Duration of the slash animation
    [SerializeField] GameObject slashEffect;

    private float slashTimer = 0f;
    private bool isSlashing = false;
    private float slashActiveTimer = 0f;
    private int SlashlifeDrain;


    override public void Init(UTBEquipment_Record data, PlayerController player)
    {
        base.Init(data, player);

        slashTimer = fireRate;

        slashEffect.SetActive(false); // Initially disable the collider
    }

    void Update()
    {
        slashTimer -= Time.deltaTime;

        // Update the position to be slightly ahead of the player
        Vector3 playerDirection = _player.transform.up;
        transform.position = _player.transform.position + playerDirection * offsetDistance;
        transform.right = playerDirection;

        if (slashTimer <= 0f)
        {
            PerformSlash();
            slashTimer = _stat.FireRate; // Reset the timer
        }

        if (isSlashing)
        {
            slashActiveTimer -= Time.deltaTime;
            if (slashActiveTimer <= 0f)
            {
                slashEffect.SetActive(false);
                isSlashing = false;
            }
        }
    }

    private void PerformSlash()
    {
        isSlashing = true;
        slashEffect.SetActive(true);
        slashActiveTimer = slashDuration; // Set the active duration of the slash

        // Play the slash animation
        if (animator != null)
        {
            animator.SetTrigger("Slash");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (isSlashing && (collision.CompareTag("Enemy") || collision.CompareTag("Boss")))
        {
            

            var enemy = collision.GetComponent<EnemyUnit>();
            if (enemy != null)
            {
                enemy._battleManager.enemyGetDamage(_stat.Damage, enemy);
            }
            if (_stat.LifeDrain > 0)
            {
                
                _player.battleManager.GainHealth(_stat.LifeDrain);
            }

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isSlashing && (collision.CompareTag("Enemy") || collision.CompareTag("Boss")))
        {


            var enemy = collision.GetComponent<EnemyUnit>();
            if (enemy != null)
            {
                if(enemy.tag == "Boss")
                {
                    StayEnemyDamage(enemy);
                }
                else
                {
                    enemy._battleManager.enemyGetDamage(_stat.Damage, enemy);
                }
                
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
