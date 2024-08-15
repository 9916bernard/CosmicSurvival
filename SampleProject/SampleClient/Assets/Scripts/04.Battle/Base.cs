using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager = null;
    public BattleField field; // Assign the BattleField object in the Inspector
    public Camera MainCamera;
    private float verticalAmplitude = 0.1f;  // The amplitude of the vertical floating movement
    private float verticalFrequency = 0.5f;  // The frequency of the vertical floating movement
    private float horizontalAmplitude = 0.1f;  // The amplitude of the horizontal floating movement
    private float horizontalFrequency = 0.5f;  // The frequency of the horizontal floating movement

    private Vector3 startPosition;

    public bool isBaseExpandFull = false;

    public static int baseStationIndex = 0;  // Track the index for base station expansion

    [HideInInspector] public bool isAutoCannonTurretActive = false;
    private bool isAutoCannonTurretFirstActive = true;

    public bool isInventoryFull => currentWeaponCount >= MaxWeaponCount;

    /// <summary>
    /// /4
    /// </summary>
    private const int MaxWeaponCount = 4;
    [HideInInspector] public int currentWeaponCount = 0;

    public GameObject DroneCage;

    public AutoCannonTurret AutoCannonTurret { get; private set; } = new AutoCannonTurret();

    // Define the turret positions relative to the base station
    private Vector3[] turretPositions;

    private void Start()
    {
        startPosition = transform.position;

        // Initialize turret positions relative to the base station position
        turretPositions = new Vector3[]
        {
            new Vector3(-1.15f, 1f, -2f),  // First slot
            new Vector3(-1.15f, -1f, -2f), // Second slot
            new Vector3(1f, 1f, -2f),   // Third slot
            new Vector3(1f, -1f, -2f),  // Fourth slot
        };
    }

    void Update()
    {
        // Calculate the new position based on sine wave oscillation for both axes
        float yOffset = Mathf.Sin(Time.unscaledTime * verticalFrequency) * verticalAmplitude;
        float xOffset = Mathf.Cos(Time.unscaledTime * horizontalFrequency) * horizontalAmplitude;
        transform.position = startPosition + new Vector3(xOffset, yOffset, 0);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            battleManager.BaseGainMetal();
        }
    }

    public bool AllInventoryTurretMaxed()
    {
        foreach (var turret in _dicTurret)
        {
            if (turret.Value._level < 9)
            {
                return false;
            }
        }
        return true;
    }

    public List<int> GetUpgradableTurretsWhenFull()
    {
        List<int> upgradableTurrets = new List<int>();

        foreach (var turret in _dicTurret)
        {
            if (turret.Value._level < 10)
            {
                upgradableTurrets.Add(turret.Key);
            }
        }

        return upgradableTurrets;
    }

    [HideInInspector] public Dictionary<int, Turret> _dicTurret = new();

    public void AddTurret(int InTurretID)
    {
        var data = TABLE.equipment.Find(InTurretID);

        _dicTurret.TryGetValue(InTurretID, out Turret turret);

        if (turret != null)
        {
            turret.Upgrade(InTurretID);
            return;
        }

        if (data == null)
        {
            Debug.LogError("Turret Data is Null");
            return;
        }

        var _obj = Resources.Load($"Battle/Turret/{data.PrefabName}");

        switch (InTurretID)
        {
            case 2001: turret = Instantiate(_obj, transform).GetComponent<AutoCannonTurret>(); break;
            case 2002: turret = Instantiate(_obj, transform).GetComponent<LaserCannonTurret>(); break;
            case 2003: turret = Instantiate(_obj, transform).GetComponent<RocketTurret>(); break;
            case 2004: turret = Instantiate(_obj, transform).GetComponent<MinerDroneTurret>(); DroneCage.SetActiveEx(true); break;
            case 2005:
                turret = Instantiate(_obj, transform).GetComponent<EnergyTurret>();
                // EnergyTurret does not follow the positional logic
                break;
        }

        if (turret == null)
        {
            Debug.LogError("Turret is Null");
            return;
        }

        // Only assign positions if the turret is not an EnergyTurret
        if (InTurretID != 2005)
        {
            int positionIndex = currentWeaponCount;
            if (positionIndex < turretPositions.Length)
            {
                turret.transform.localPosition = turretPositions[positionIndex];
                if (InTurretID == 2004)
                {
                    DroneCage.transform.localPosition = turretPositions[positionIndex];
                }
            }
        }

        turret.Init(data, this);

        _dicTurret.Add(InTurretID, turret);

        currentWeaponCount++;

        // Check if we need to expand the base station
        if (AllInventoryTurretMaxed() && isInventoryFull && !isBaseExpandFull)
        {
            ExpandBaseStation();
        }
    }

    public void ExpandBaseStation()
    {
        if (AllInventoryTurretMaxed() && isInventoryFull && !isBaseExpandFull)
        {
            if (baseStationIndex < battleManager.baseStationPositions.Length - 1)
            {
                // Increment the base station index to the next location
                baseStationIndex++;

                // Instantiate the next base station
                Vector3 nextBasePosition = battleManager.baseStationPositions[baseStationIndex];
                GameObject newBasePrefab = Resources.Load<GameObject>("Battle/BaseStation"); // Load the prefab

                // Instantiate and initialize the new base station
                GameObject newBaseObject = Instantiate(newBasePrefab, nextBasePosition, Quaternion.identity);
                Base newBase = newBaseObject.GetComponent<Base>();
                newBase.InitializeFromExistingBase(this); // Copy stats and turrets from the current base

                battleManager.bases.Add(newBase); // Add the new base to the list
            }
            else
            {
                isBaseExpandFull = true; // Mark that the base expansion is complete
            }
        }
    }


    public void InitializeFromExistingBase(Base existingBase)
    {
        // Copy necessary fields from the existing base
        battleManager = existingBase.battleManager;
        field = existingBase.field;
        MainCamera = existingBase.MainCamera;
        DroneCage = existingBase.DroneCage;

        // Position of the new base station
        Vector3 newBasePosition = battleManager.baseStationPositions[baseStationIndex];

        // Adjust the turret positions based on the new base position
        turretPositions = new Vector3[]
        {
            newBasePosition + new Vector3(-1.15f, 1f, -2f),  // First slot
            newBasePosition + new Vector3(-1.15f, -1f, -2f), // Second slot
            newBasePosition + new Vector3(1f, 1f, -2f),   // Third slot
            newBasePosition + new Vector3(1f, -1f, -2f),  // Fourth slot
        };

        // Any other necessary initializations
    }

    public void ResetBaseStats()
    {
        // If this is the first base station, only remove the weapons
        if (battleManager.bases[0] == this)
        {
            var turretInstances = GetComponentsInChildren<Turret>(); // Note the plural 'Components'
            foreach (var turret in turretInstances)
            {
                Destroy(turret.gameObject);
            }
            _dicTurret.Clear();
            currentWeaponCount = 0;
        }
        else
        {
            // For other base stations, destroy the entire base along with its turrets
            battleManager.bases.Remove(this); // Remove this base from the BattleManager's list
            Destroy(gameObject); // Destroy the base object
        }
    }

}
