using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideDrone : Weapon
{
    [SerializeField] private BattleManager battleManager = null;

    private float angle; // Angle for circular motion


    override public void Init(UTBEquipment_Record data, PlayerController player)
    {
        base.Init(data, player);

        
    }


    void Update()
    {
        if (_player != null)
        {
            RotateAroundPlayer();
        }
    }

    private void RotateAroundPlayer()
    {
        angle += _stat.RotationSpeed * Time.deltaTime; // Update angle based on rotation speed
        float x = Mathf.Cos(angle) * _stat.RotationRadius;
        float y = Mathf.Sin(angle) * _stat.RotationRadius;
        transform.position = new Vector3(_player.transform.position.x + x, _player.transform.position.y + y, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
        {
            var enemy = collision.GetComponent<EnemyUnit>();
            if (enemy != null)
            {
              enemy._battleManager.enemyGetDamage(_stat.Damage, enemy);
            }
        }
    }
}
