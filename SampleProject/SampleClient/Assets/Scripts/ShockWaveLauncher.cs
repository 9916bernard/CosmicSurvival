using UnityEngine;
using System.Collections;

public class ShockWaveLauncher : Weapon
{
    private BattleField field; // Assign the Field object in the Inspector
    private BattleManager battleManager = null; // Reference to the BattleManager script
    private Coroutine fireCoroutine;

    public override void Init(UTBEquipment_Record data, PlayerController player)
    {
        base.Init(data, player);
        battleManager = player.battleManager;
        field = battleManager._Field;
        StartFiring(); // Start firing when initialized
    }

    public void StartFiring()
    {
        if (fireCoroutine == null)
        {
            fireCoroutine = StartCoroutine(FireShockWaves());
        }
    }

    public void StopFiring()
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }
    }

    private IEnumerator FireShockWaves()
    {
        while (true)
        {
            // Create a new shockwave and set its properties
            Vector3 direction = battleManager._Field._Player.transform.up;
            ShockWave shockWave = field.GetPooledShockWave(_stat.Damage, _stat.ProjectileSpeed, 5f, 1f, direction);

            // Set the position and rotation of the shockwave
            shockWave.transform.position = battleManager._Field._Player.transform.position;
            shockWave.transform.rotation = battleManager._Field._Player.transform.rotation;

            // Log when a shockwave is fired for debugging purposes
            //Debug.Log($"Shockwave fired at {Time.time} with FireRate: {_stat.FireRate}");

            // Wait for the specified fire rate before firing the next shockwave
            yield return new WaitForSeconds(_stat.FireRate);
        }
    }
}
