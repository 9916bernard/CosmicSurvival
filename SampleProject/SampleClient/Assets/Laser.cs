using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Boss"))
        {
            if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
            {

                var enemy = other.GetComponent<EnemyUnit>();

                // Reduce the enemy's health
                if (enemy != null)
                {
                    enemy._battleManager.enemyGetDamage(100, enemy);
                }
            }
        }
    }
}
