using Cysharp.Threading.Tasks.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public partial class BattleManager : MonoBehaviour
{
    [HideInInspector] public BattleField _Field = null;
    private BulletLauncher _BulletSpawner = null;
    public EnemySpawner _EnemySpawner = null;
    [SerializeField] private PickUpSpawner PickUpSpawner = null;
    [SerializeField] private ExpSpawner _ExpSpawner = null;
    [SerializeField] private RocketLauncher _rocketSpawner = null;
    [SerializeField] private Canvas canvasUi = null;
    public GameObject weapons = null;
    [SerializeField] private BgManager bgManager = null;
    [SerializeField] SceneMain sceneMain = null;
    [SerializeField] private AstroidSpawner astroidSpawner = null;
    [SerializeField] private BgGenerator bgGenerator = null;

    // Experience properties
    [HideInInspector] public ExpBar expBar; // Reference to the ExpBar
    [HideInInspector] public HpBar hpBar; // Reference to the HpBar
    [HideInInspector] public BoostBar BoostBar; // Reference to the BoostBar
    [HideInInspector] public LevelUp levelUpUi; // Reference to the LevelUpCanvas
    [HideInInspector] public NavigationArrow navigationArrow;
    private float expToLevelUp = 2;
    private float experience = 0;

    //public AllyCarrierCollider AllyCarrierCollider;
    [HideInInspector] public float MetalColleced = 0;
    private float baseMetalToLevelUp = 2;
    [SerializeField] private GameObject bulletPrefab = null;

    [HideInInspector] public bool isRocketSpawnerActive = false;
    public Base baseStation = null;

    private float defaultKnockbackDistance = 0.3f; // Default knockback distance
    private float knockbackDuration = 0.2f; // Duration of the knockback effect
    private float blinkDuration = 0.1f; // Duration of each blink
    private int blinkCount = 3; // Number of blinks

    private Vector3 damagePoistionOffset = new Vector3(-0.2f, 0.3f, 0);

    private Vector3 MetalSpawnLocation = new Vector3(0.3f, 0, 0); // Metal spawn location
    private Vector3 HpSpawnLocation = new Vector3(-0.3f, 0, 0); // Hp spawn location

    MetalCount metalCount;

    private float baseGainedMetal = 0;

    private UIBattleBase _uiBattle = null;

    private Action _onBattleEndAction = null;

    public bool gameStarted = false;

    private float startTime;

    public bool isTimeFast = false;

    [HideInInspector] public long gold;
    [HideInInspector] public int pearl;
    [HideInInspector] public int Ruby;
    [HideInInspector] public int Amber;
    [HideInInspector] public int Sapphire;
    [HideInInspector] public int Emerald;
    [HideInInspector] public int Black;

    [HideInInspector] public float battleTime;

    private void Update()
    {
        if (gameStarted)
        {
            battleTime = Time.time - startTime;
            //metalCount.MetalText.text = MetalColleced.ToString() +"/"+ baseMetalToLevelUp.ToString();
        }


    }

    public void Init(UIBattleBase InBattleUI, Action InOnBattleEndAction)
    {
        _uiBattle = InBattleUI;
        _onBattleEndAction = InOnBattleEndAction;
    }

    public void SetLobby()
    {
        _Field.PlayerInst();
        _Field.Init();
        bgGenerator.Init();
        Debug.Log("bggenerator init");
        bgManager.Init();
    }

    public void StartGame()
    {
        ResumeGame();
        SpawnerPlayerActive();
        gameStarted = true;
        ShowBattleUI();
        hpBar.SetInitialHp(_Field._Player.maxHealth);
        BoostBar.SetMaxBoost((int)_Field._Player.maxBoosterGage);
        BoostBar.SetBoost((int)_Field._Player.boosterGage);
        navigationArrow.Init(baseStation);
        startTime = Time.time;
        SetReviveUsed(false);
        PickUpSpawner.Init();


        if (!USER.player.IsTutorialCompleted("BATTLE_START_MODE_MAIN"))
        {
            StartCoroutine(HandleTutorialOverlay("ui_tutorial", ETB_TUTORIAL.BATTLE_START_MODE_MAIN));
        }
        else if (!USER.player.IsTutorialCompleted("BATTLE_MOVE_FAR"))
        {
            StartCoroutine(HandleTutorialOverlay("ui_tutorial", ETB_TUTORIAL.BATTLE_MOVE_FAR));
        }
        else if (!USER.player.IsTutorialCompleted("BATTLE_TIME_PASS"))
        {
            StartCoroutine(HandleTutorialOverlay("ui_tutorial", ETB_TUTORIAL.BATTLE_TIME_PASS));
        }

        SOUND.Sfx(EUI_SFX.GAME_START);

    }

    public IEnumerator HandleTutorialOverlay(string overlayName, ETB_TUTORIAL tutorialType)
    {
        PauseGame();
        UIM.ShowOverlay(overlayName, EUI_LoadType.COMMON, new() { { "TutorialType", tutorialType } });

        // Wait until the overlay is closed
        while (IsOverlayActive(overlayName))
        {
            yield return null; // Wait for the next frame
        }

        if (tutorialType == ETB_TUTORIAL.BATTLE_START_MODE_MAIN || tutorialType == ETB_TUTORIAL.BATTLE_DROP_EXP || tutorialType == ETB_TUTORIAL.BATTLE_MOVE_FAR || tutorialType == ETB_TUTORIAL.BATTLE_TIME_PASS || tutorialType == ETB_TUTORIAL.BATTLE_DROP_METAL || tutorialType == ETB_TUTORIAL.BATTLE_HIT)
        {
            ResumeGame();
        }
        _Field.playerObject.SetActive(true);
        //ResumeGame();
        USER.player.SetTutorialCompleted(tutorialType.ToString());
    }

    private bool IsOverlayActive(string overlayName)
    {
        // Implement logic to check if the overlay is still active
        // This can be done by checking the active state of the overlay GameObject
        // or any other condition that signifies the overlay is closed

        // Example (assuming you have a way to get the overlay GameObject by name):
        GameObject overlay = GameObject.Find(overlayName);
        return overlay != null && overlay.activeSelf;
    }

    public void ShowBattleUI()
    {
        if (_uiBattle != null)
        {
            AssignBattleUIComponents(_uiBattle, _Field._Player.GetComponent<PlayerController>());
        }
        else
        {
            Debug.Log("Battle UI component not found.");
        }


    }

    public void SpawnerPlayerActive()
    {
        _EnemySpawner.Init();
        astroidSpawner.Init(_Field._Player);
        _Field._Player.Init(this, _rocketSpawner, _BulletSpawner);
    }

    public void PauseGame()
    {
        if (Time.timeScale != 0f)
        {
            Time.timeScale = 0f;
            canvasUi.gameObject.SetActive(true);
            //_Field.playerObject.SetActive(false);
        }

    }

    public void ResumeGame()
    {
        if (Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
            //_Field.playerObject.SetActive(true);
        }

    }

    private void AssignBattleUIComponents(UIBattleBase battleUI, PlayerController playerController)
    {
        expBar = battleUI.expBar;
        hpBar = battleUI.hpBar;
        BoostBar = battleUI.boostBar;
        metalCount = battleUI.metalCount;
        metalCount.Init();
        playerController.fingerStick = battleUI.fingerStick;
        navigationArrow = battleUI.navigationArrow;

    }

    public void GainMetal(int amount)
    {
        MetalColleced += amount;
        metalCount.MetalText.text = MetalColleced.ToString() + "/" + baseMetalToLevelUp.ToString(); // Update the UI with the current metal status
        SOUND.Sfx(EUI_SFX.METAL_PICKUP);
    }


    public void BaseGainMetal()
    {

        if (MetalColleced >= baseMetalToLevelUp)
        {
            MetalColleced -= baseMetalToLevelUp;
            BaseLevelUp(); // Level-up the base
            metalCount.MetalText.text = MetalColleced.ToString() + "/" + baseMetalToLevelUp.ToString(); // Update the UI with the current metal status
        }


    }


    public void GainExperience(int amount)
    {
        experience += amount;
        expBar.SetExperience((int)experience);

        if (experience >= expToLevelUp)
        {
            experience -= expToLevelUp;
            LevelUp();
        }
        SOUND.Sfx(EUI_SFX.EXP_PICKUP);
    }

    public void GainHealth(int amount)
    {
        _Field._Player.health += amount;
        if (_Field._Player.health > _Field._Player.maxHealth)
        {
            _Field._Player.health = _Field._Player.maxHealth;
        }
        hpBar.SetHp(_Field._Player.health);
        SOUND.Sfx(EUI_SFX.HP_PICKUP);
    }

    public void GainBlackHole(BlackHolePickUp blackhole)
    {
        StartCoroutine(BlackHoleEffect(blackhole));
    }

    private IEnumerator BlackHoleEffect(BlackHolePickUp blackHole)
    {
        float pullDuration = 5f;
        float pullStrength = 5f;

        // Remove the black hole GameObject immediately
        Destroy(blackHole.gameObject);

        // Find all active exps and metals
        List<Exp> activeExps = new List<Exp>(_Field.GetComponentsInChildren<Exp>());
        List<Metal> activeMetals = new List<Metal>(_Field.GetComponentsInChildren<Metal>());

        float elapsedTime = 0f;

        while (elapsedTime < pullDuration)
        {
            elapsedTime += Time.deltaTime;

            // Pull active exps towards the player
            foreach (var exp in activeExps)
            {
                if (exp.gameObject.activeInHierarchy)
                {
                    Vector3 direction = (_Field._Player.transform.position - exp.transform.position).normalized;
                    exp.transform.position += direction * pullStrength * Time.deltaTime;
                }
            }

            // Pull active metals towards the player
            foreach (var metal in activeMetals)
            {
                if (metal.gameObject.activeInHierarchy)
                {
                    Vector3 direction = (_Field._Player.transform.position - metal.transform.position).normalized;
                    metal.transform.position += direction * pullStrength * Time.deltaTime;
                }
            }

            yield return null;
        }
    }


    public void LevelUp()
    {

        if (!USER.player.IsTutorialCompleted("BATTLE_UPGRADE_UNIT"))
        {
            StartCoroutine(HandleTutorialOverlay("ui_tutorial", ETB_TUTORIAL.BATTLE_UPGRADE_UNIT));
        }
        Debug.Log("Level Up!");
        expToLevelUp += 1.5f; // test 0
        expBar.SetMaxExperience((int)expToLevelUp);
        expBar.SetExperience((int)experience);

        UIBase upgrade = UIM.ShowPopup("ui_battle_upgrade", EUI_LoadType.BATTLE, new()
        {
            { "BattleManager", this },
            { "Player", _Field._Player.GetComponent<PlayerController>() },
            { "Type", "player"}
        });

        SOUND.Sfx(EUI_SFX.LEVEL_UP);
    }

    public void BaseLevelUp()
    {
        if (!USER.player.IsTutorialCompleted("BATTLE_UPGRADE_BASE"))
        {
            StartCoroutine(HandleTutorialOverlay("ui_tutorial", ETB_TUTORIAL.BATTLE_UPGRADE_BASE));
        }

        Debug.Log("Base Level Up!");
        baseMetalToLevelUp += 0.5f;

        UIBase upgrade = UIM.ShowPopup("ui_battle_base_upgrade", EUI_LoadType.BATTLE, new()
        {
            { "BattleManager", this },
            { "Player", _Field._Player.GetComponent<PlayerController>() },
            { "Type", "base"}
        });
        SOUND.Sfx(EUI_SFX.LEVEL_UP);
    }


    public void DropExperience(EnemyUnit enemy)
    {
        ExpSpawner expSpawner = FindObjectOfType<ExpSpawner>();
        expSpawner.SpawnExp(enemy.transform.position);

        if (!USER.player.IsTutorialCompleted("BATTLE_DROP_EXP"))
        {
            StartCoroutine(HandleTutorialOverlay("ui_tutorial", ETB_TUTORIAL.BATTLE_DROP_EXP));
        }

    }

    public void DropMetal(EnemyUnit enemy)
    {


        float dropChance = 0.3f;
        if (UnityEngine.Random.value < dropChance)
        {
            if (!USER.player.IsTutorialCompleted("BATTLE_DROP_METAL"))
            {
                StartCoroutine(HandleTutorialOverlay("ui_tutorial", ETB_TUTORIAL.BATTLE_DROP_METAL));
            }
            MetalSpawner metalSpawner = FindObjectOfType<MetalSpawner>();
            metalSpawner.SpawnMetal(enemy.transform.position + MetalSpawnLocation);
        }
    }

    public void DropHp(EnemyUnit enemy)
    {
        float dropChange = 0.05f;
        if (UnityEngine.Random.value < dropChange)
        {
            PickUpSpawner pickUpSpawner = FindObjectOfType<PickUpSpawner>();
            PickUpSpawner.SpawnHp(enemy.transform.position + HpSpawnLocation);
        }


    }

    public void DropBlackHole(EnemyUnit enemy)
    {
        float dropChance = 0.01f;
        if (UnityEngine.Random.value < dropChance)
        {
            PickUpSpawner pickUpSpawner = FindObjectOfType<PickUpSpawner>();
            PickUpSpawner.SpawnBlackHole(enemy.transform.position);
        }
    }
}