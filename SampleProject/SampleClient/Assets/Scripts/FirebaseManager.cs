using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using Cysharp.Threading.Tasks;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance { get; private set; }
    public FirebaseAuth Auth { get; private set; }
    public FirebaseFirestore Firestore { get; private set; }
    public FirebaseUser CurrentUser { get; private set; }

    void Awake()
    {
        // Ensure that there is only one instance of FirebaseManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeFirebase();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Result == DependencyStatus.Available)
            {
                // Firebase is ready to use
                Auth = FirebaseAuth.DefaultInstance;
                Firestore = FirebaseFirestore.DefaultInstance;
                Debug.Log("Firebase initialized successfully.");
                // Automatically restore the current user if logged in previously
                CurrentUser = Auth.CurrentUser;
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });
    }

    public void SetCurrentUser(FirebaseUser user)
    {
        
            CurrentUser = user;
        
    }

    public async UniTask SaveAllDataToFirestore()
    {
        if (CurrentUser == null)
        {
            Debug.Log("No user is signed in");
            return;
        }

        var userId = CurrentUser.UserId;

        var recordData = new Dictionary<string, object>
        {
            { "userName", USER.account.data.AccountName },
            { "bestRecord", USER.player.GetRecord() }
        };

        var fundData = new Dictionary<string, object>
        {
            { "gold", USER.fund.GetFund(ETB_FUND.GOLD) },
            { "ruby", USER.fund.GetFund(ETB_FUND.MINERAL_RUBY) },
            { "pearl", USER.fund.GetFund(ETB_FUND.MINERAL_PEARL) },
            { "amber", USER.fund.GetFund(ETB_FUND.MINERAL_AMBER) },
            { "sappaire", USER.fund.GetFund(ETB_FUND.MINERAL_SAPPAIRE) },
            { "black", USER.fund.GetFund(ETB_FUND.MINERAL_BLACK) }
        };

        var upgradeData = new Dictionary<string, object>
        {
            { "ATK Level", USER.upgrade.GetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.PLAYER_ATK) },
            { "HP Level", USER.upgrade.GetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.PLAYER_HP) },
            { "Spd Level", USER.upgrade.GetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.PLAYER_SPD) },
            { "FireRt Level", USER.upgrade.GetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.PLAYER_FIRERATE) },
            { "Durat Level", USER.upgrade.GetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.PLAYER_DURATION) },
            { "PRJNUM Level", USER.upgrade.GetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.PLAYER_PRJNUM) },
            { "Station ATK Level", USER.upgrade.GetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.STATION_ATK) },
            { "Station FireRt Level", USER.upgrade.GetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.STATION_FIRERT) },
            { "Station Pene Level", USER.upgrade.GetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.STATION_PRJNUM) }
        };

        await SaveDataToFirestore("record", recordData);
        await SaveDataToFirestore("fund", fundData);
        await SaveDataToFirestore("upgrade", upgradeData);
    }

    public async UniTaskVoid SaveBattleDataToFirestore()
    {
        if (CurrentUser == null)
        {
            Debug.LogError("No user is signed in");
            return;
        }

        var userId = CurrentUser.UserId;

        var recordData = new Dictionary<string, object>
        {
            { "userName", USER.account.data.AccountName },
            { "bestRecord", USER.player.GetRecord() }
        };

        var fundData = new Dictionary<string, object>
        {
            { "gold", USER.fund.GetFund(ETB_FUND.GOLD) },
            { "ruby", USER.fund.GetFund(ETB_FUND.MINERAL_RUBY) },
            { "pearl", USER.fund.GetFund(ETB_FUND.MINERAL_PEARL) },
            { "amber", USER.fund.GetFund(ETB_FUND.MINERAL_AMBER) },
            { "sappaire", USER.fund.GetFund(ETB_FUND.MINERAL_SAPPAIRE) },
            { "black", USER.fund.GetFund(ETB_FUND.MINERAL_BLACK) }
        };

        await SaveDataToFirestore("record", recordData);
        await SaveDataToFirestore("fund", fundData);
    }

    private async UniTask SaveDataToFirestore(string collectionName, Dictionary<string, object> data)
    {
        var userId = CurrentUser.UserId;
        var docRef = Firestore.Collection("users").Document(userId).Collection(collectionName).Document("data");

        await docRef.SetAsync(data).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log($"{collectionName} data saved to Firestore successfully.");
            }
            else
            {
                Debug.LogError($"Failed to save {collectionName} data to Firestore: " + task.Exception);
            }
        });
    }
}
    

