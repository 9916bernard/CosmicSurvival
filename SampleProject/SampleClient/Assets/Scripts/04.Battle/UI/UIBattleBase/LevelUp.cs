using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    public GameObject fingerStick; // Reference to the FingerStick component
    public Button button1;
    public Button button2;
    public Button button3;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        Hide(); // Ensure the panel is hidden initially
    }

    

    public void Show()
    {
        rect.localScale = Vector3.one;
        if (fingerStick != null)
        {
            fingerStick.SetActive(false);
        }
        AssignRandomUpgrades();
        Time.timeScale = 0f; // Pause the game
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        if (fingerStick != null)
        {
            fingerStick.SetActive(true);
        }
        Time.timeScale = 1f; // Resume the game
    }

    private void AssignRandomUpgrades()
    {
        List<Upgrade.UpgradeType> upgradeTypes = new List<Upgrade.UpgradeType>
        {
            Upgrade.UpgradeType.Damage,
            Upgrade.UpgradeType.MovementSpeed,
            Upgrade.UpgradeType.BulletPenetration,
            Upgrade.UpgradeType.FireRate,
            Upgrade.UpgradeType.RestoreHealth,
            Upgrade.UpgradeType.MaxHealth
            // Add more upgrade types here in the future
        };

        // Shuffle the list to get a random order
        for (int i = 0; i < upgradeTypes.Count; i++)
        {
            Upgrade.UpgradeType temp = upgradeTypes[i];
            int randomIndex = Random.Range(i, upgradeTypes.Count);
            upgradeTypes[i] = upgradeTypes[randomIndex];
            upgradeTypes[randomIndex] = temp;
        }

        // Assign the first three unique upgrades
        AssignUpgrade(button1, upgradeTypes[0]);
        AssignUpgrade(button2, upgradeTypes[1]);
        AssignUpgrade(button3, upgradeTypes[2]);
    }

    private void AssignUpgrade(Button button, Upgrade.UpgradeType upgradeType)
    {
        Upgrade upgradeScript = button.GetComponent<Upgrade>();
        upgradeScript.upgradeType = upgradeType;
        Text buttonText = button.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            buttonText.text = upgradeType.ToString();
        }
        else
        {
            buttonText.text = "Unknown";
        }
    }
}
