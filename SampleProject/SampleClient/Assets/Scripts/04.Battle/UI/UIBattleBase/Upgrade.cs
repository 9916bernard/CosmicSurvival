using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class Upgrade : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private BattleField field;
    [SerializeField] private BulletLauncher bulletSpawner;
    [SerializeField] private BattleManager battleManager;

    public enum UpgradeType
    {
        Damage,
        MovementSpeed,
        BulletPenetration,
        FireRate,
        RestoreHealth,
        MaxHealth
    }

    public UpgradeType upgradeType;

    public void OnClick()
    {
        LevelUp levelUp = GetComponentInParent<LevelUp>(); // Get LevelUp from the parent or the root
        if (levelUp != null)
        {
            switch (upgradeType)
            {
                case UpgradeType.Damage:
                    Debug.Log("Damage increased!");
                    field.Damage += 1;
                    break;
                case UpgradeType.MovementSpeed:
                    Debug.Log("Movement speed increased!");
                    playerController.moveSpeed += 0.01f;
                    break;
                case UpgradeType.BulletPenetration:
                    Debug.Log("Bullet penetration increased!");
                    field.BulletHitCount += 1;
                    break;
                case UpgradeType.FireRate:
                    Debug.Log("Fire rate increased!");
                    bulletSpawner.fireRate -= 0.05f; // Assuming lower fire rate means faster shooting
                    break;
                case UpgradeType.RestoreHealth:
                    Debug.Log("Health restored!");
                    playerController.health = Mathf.Min(playerController.health + 1, playerController.maxHealth);
                    break;
                case UpgradeType.MaxHealth:
                    Debug.Log("Max health increased!");
                    playerController.maxHealth += 1;
                    
                    break;
                default:
                    Debug.LogWarning("Unknown upgrade type.");
                    break;
            }
            levelUp.Hide();
        }
        else
        {
            Debug.LogWarning("LevelUp component not found in parent or ancestors.");
        }
    }
}
