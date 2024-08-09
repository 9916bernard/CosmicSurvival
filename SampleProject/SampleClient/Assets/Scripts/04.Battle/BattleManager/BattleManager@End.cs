using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using Cysharp.Threading.Tasks;

public partial class BattleManager : MonoBehaviour
{
    private bool reviveUsed = false; // Flag to track if revive has been used

    private void ExitAction()
    {
        // Reset the revive flag for a new game
        reviveUsed = false;

        // Update the user's gold
        recordFund();
        recordStore();
        expToLevelUp = 2;
        experience = 0;
        expBar.SetExperience((int)experience);
        baseMetalToLevelUp = 2;
        baseGainedMetal = 0;
        MetalColleced = 0;
        GainMetal(0);
        battleTime = 0;
        _EnemySpawner.bossSpawnTimer = 0;

        baseStation.DroneCage.SetActive(false);

        // Reset player position and deactivate
        bgManager.MovePlayerToPosition(new Vector3(15, 15, 0), 0.1f);

        // Reset player stats
        _Field._Player.GetComponent<PlayerController>().ResetPlayerStats();
        baseStation.ResetBaseStats();

        // Push pooled objects back to their pools
        pushPools();
        destroyNotPool();


        // Stop enemy spawning
        _EnemySpawner.stopSpawning();

        gameStarted = false;

        gold = 0;

        // Invoke the on-battle-end action if it's set
        _onBattleEndAction?.Invoke();

        // Save user data locally
        USER.fund.Save();
        USER.player.Save();

        // Save user data to Firestore
        SaveUserDataToFirestore().Forget();

        SOUND.Sfx(EUI_SFX.DEAD_TUTO);
        UIM.ShowOverlay("ui_tutorial", EUI_LoadType.COMMON, new() { { "TutorialType", ETB_TUTORIAL.BATTLE_DIE } });
    }

    private void recordStore()
    {
        if (USER.player.GetRecord() < battleTime)
        {
            USER.player.SetRecord(battleTime);
        }
    }

    private void recordFund()
    {
        USER.fund.AddGold(gold);
        USER.fund.AddPearl(pearl);
        USER.fund.AddRuby(Ruby);
        USER.fund.AddSapphire(Sapphire);
        USER.fund.AddAmber(Amber);
        USER.fund.AddBlack(Black);
        USER.fund.AddEmerald(Emerald);
    }

    private async UniTaskVoid SaveUserDataToFirestore()
    {
        if (FirebaseManager.Instance.CurrentUser == null)
        {
            Debug.Log("No user is signed in");
            return;
        }

        var userId = FirebaseManager.Instance.CurrentUser.UserId;

        // Record collection within the user's document
        var recordRef = FirebaseManager.Instance.Firestore.Collection("users").Document(userId).Collection("record").Document("data");
        var recordData = new Dictionary<string, object>
        {
            { "userName", USER.account.data.AccountName },
            { "bestRecord", USER.player.GetRecord() }
        };
        await recordRef.SetAsync(recordData).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Record data saved to Firestore successfully.");
            }
            else
            {
                Debug.LogError("Failed to save record data to Firestore: " + task.Exception);
            }
        });

        // Fund collection within the user's document
        var fundRef = FirebaseManager.Instance.Firestore.Collection("users").Document(userId).Collection("fund").Document("data");
        var fundData = new Dictionary<string, object>
        {
            { "gold", USER.fund.GetFund(ETB_FUND.GOLD) },
            { "ruby", USER.fund.GetFund(ETB_FUND.MINERAL_RUBY) },
            { "pearl", USER.fund.GetFund(ETB_FUND.MINERAL_PEARL) },
            { "amber", USER.fund.GetFund(ETB_FUND.MINERAL_AMBER) },
            { "sappaire", USER.fund.GetFund(ETB_FUND.MINERAL_SAPPAIRE) },
            { "black", USER.fund.GetFund(ETB_FUND.MINERAL_BLACK) }
        };
        await fundRef.SetAsync(fundData).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Fund data saved to Firestore successfully.");
            }
            else
            {
                Debug.LogError("Failed to save fund data to Firestore: " + task.Exception);
            }
        });

        // Root level record collection for ranking
        var rootRecordRef = FirebaseManager.Instance.Firestore.Collection("record").Document(userId);
        await rootRecordRef.SetAsync(recordData).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Root record data saved to Firestore successfully.");
            }
            else
            {
                Debug.LogError("Failed to save root record data to Firestore: " + task.Exception);
            }
        });
    }

    private void pushPools()
    {
        pushEnemyBoss();
        pushEnemies();
        pushBullets();
        pushExps();
        pushMetals();
        pushDamageTexts();
        pushAstroids();
        pushHps();
        pushRockets();
        pushWaves();
    }

    private void destroyNotPool()
    {
        // Destroy all BlackHolePickUp objects
        List<BlackHolePickUp> blackHolePickUps = new List<BlackHolePickUp>(_Field.GetComponentsInChildren<BlackHolePickUp>());
        foreach (var blackHolePickUp in blackHolePickUps)
        {
            Destroy(blackHolePickUp.gameObject);
        }

        // Destroy all LaserPickUp objects
        var laserPickUps = new List<LaserPickUp>(_Field.GetComponentsInChildren<LaserPickUp>());
        foreach (var laserPickUp in laserPickUps)
        {
            Destroy(laserPickUp.gameObject);
        }

        // Destroy all objects named "laser(Clone)"
        var lasers = FindObjectsOfType<GameObject>();
        foreach (var laser in lasers)
        {
            if (laser.name == "laser(Clone)")
            {
                Destroy(laser);
            }
        }
    }


    private void pushHps()
    {
        var hps = _Field.GetComponentsInChildren<HealPickUp>(true);

        foreach (var hp in hps)
        {
            if (hp.gameObject.activeInHierarchy)
            {
                hp._pool.Push(hp);
            }
        }
    }

    private void pushEnemies()
    {
        var enemyUnits = _Field.GetComponentsInChildren<EnemyUnit>(true);

        foreach (var enemy in enemyUnits)
        {
            if (enemy.gameObject.activeInHierarchy)
            {
                enemy._pool.Push(enemy);
            }
        }
    }

    private void pushAstroids()
    {
        var astroids = _Field.GetComponentsInChildren<Astroid>(true);

        foreach (var astroid in astroids)
        {
            if (astroid.gameObject.activeInHierarchy)
            {
                astroid._pool.Push(astroid);
            }
        }
    }

    private void pushEnemyBoss()
    {
        var enemyUnitBoss = _Field.GetComponentsInChildren<EnemyUnitBoss>(true);

        foreach (var enemy in enemyUnitBoss)
        {
            if (enemy._pool != null)
            {
                enemy.spriteRenderer.color = enemy.originalColor;
                enemy._pool.Push(enemy);
            }
            else
            {
                Debug.LogError("EnemyUnitBoss pool is null.");
            }
        }
    }

    private void pushBullets()
    {
        var bullets = _Field.GetComponentsInChildren<Bullet>(true);

        foreach (var bullet in bullets)
        {
            if (bullet.gameObject.activeInHierarchy)
            {
                bullet._pool.Push(bullet);
            }
        }
    }

    private void pushWaves()
    {

        var waves = _Field.GetComponentsInChildren<ShockWave>(true);

        foreach (var wave in waves)
        {
            if (wave.gameObject.activeInHierarchy)
            {
                wave._pool.Push(wave);
            }
        }
    }

    private void pushExps()
    {
        var exps = _Field.GetComponentsInChildren<Exp>(true);

        foreach (var exp in exps)
        {
            if (exp.gameObject.activeInHierarchy)
            {
                exp._pool.Push(exp);
            }
        }
    }

    private void pushMetals()
    {
        var metals = _Field.GetComponentsInChildren<Metal>(true);

        foreach (var metal in metals)
        {
            if (metal.gameObject.activeInHierarchy)
            {
                metal._pool.Push(metal);
            }
        }
    }

    private void pushRockets()
    {
        var rockets = _Field.GetComponentsInChildren<Rocket>(true);

        foreach (var rocket in rockets)
        {
            if (rocket.gameObject.activeInHierarchy)
            {
                rocket._pool.Push(rocket);
            }
        }
    }

    private void pushDamageTexts()
    {
        var damageTexts = _Field.GetComponentsInChildren<DamageText>(true);

        foreach (var damageText in damageTexts)
        {
            if (damageText.gameObject.activeInHierarchy)
            {
                damageText._Pool.Push(damageText);
            }
        }
    }

   

    public bool HasUsedRevive()
    {
        return reviveUsed;
    }

    public void SetReviveUsed(bool used)
    {
        reviveUsed = used;
    }
}
