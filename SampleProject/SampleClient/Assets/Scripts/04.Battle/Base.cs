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

    [HideInInspector] public bool isAutoCannonTurretActive = false;
    private bool isAutoCannonTurretFirstActive = true;

    public bool isInventoryFull => currentWeaponCount >= MaxWeaponCount;
    private const int MaxWeaponCount = 4;
    private int currentWeaponCount = 0; // AutoCannon is initially active

    public GameObject DroneCage;

    public AutoCannonTurret AutoCannonTurret { get; private set; } = new AutoCannonTurret();

    // Define the turret positions
    private readonly Vector3[] turretPositions = new Vector3[]
    {
        new Vector3(-1.15f, 1f, -2f),  // First slot
        new Vector3(-1.15f, -1f, -2f), // Second slot
        new Vector3(1f, 1f, -2f),   // Third slot
        new Vector3(1f, -1f, -2f),  // Fourth slot
    };

    private void Start()
    {
        startPosition = transform.position;
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
            if (turret.Value._level < 9)
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

        // Instantiate the turret and assign it to the correct position
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
    }

    public void ResetBaseStats()
    {
        var turretInstances = GetComponentsInChildren<Turret>(); // Note the plural 'Components'
        foreach (var turret in turretInstances)
        {
            Destroy(turret.gameObject);
        }
        _dicTurret.Clear();

        currentWeaponCount = 0;
    }
}
