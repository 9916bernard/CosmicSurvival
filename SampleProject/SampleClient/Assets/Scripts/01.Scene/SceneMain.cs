using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System;
using Cysharp.Threading.Tasks.Triggers;
using Firebase;
using Firebase.Extensions;
using Firebase.Auth;
using Firebase.Firestore;
using GoogleMobileAds.Api;

public partial class SceneMain : MonoBehaviour
{
    [SerializeField] private Canvas _Canvas = null;
    [SerializeField] private BattleManager battleManager = null;
    //[SerializeField] private Button adButton = null; // Reference to your ad button

    private UIMainBase _uiMain = null;

    // Start is called before the first frame update
    void Start()
    {
        UIM.Inst().SetCanvas(_Canvas);
        SOUND.Inst().PlayBgm(EUI_BGM.BATTLE);

        // Initialize AdMobManager
        InitializeAdMob();

        Action startAction = () =>
        {
            BattleStartAction();
        };

        _uiMain = UIM.ShowBase("ui_main_base", EUI_LoadType.MAIN, true, new() { { "StartAction", startAction }, { "BattleManager", battleManager } }) as UIMainBase;

        Application.targetFrameRate = 60;

        battleManager.Init(_uiMain.GetBattleUI(), BattleEndAction);
        battleManager.SetLobby();
        battleManager.PauseGame();

        // Add button click listener
        //adButton.onClick.AddListener(OnAdButtonClicked);
    }

    public void Init()
    {
        Action startAction = () =>
        {
            BattleStartAction();
        };

        battleManager.PauseGame();
    }

    private void InitializeAdMob()
    {
        // Ensure the AdMobManager is initialized
        AdMobManager.Instance.ShowBanner(); // Show the banner ad
    }

    public void BattleStartAction()
    {
        battleManager.StartGame();
    }

    public void BattleEndAction()
    {
        _uiMain.SetBattleEnd();
        // battleManager.EndGame();
    }

    private void OnAdButtonClicked()
    {
        // Show the rewarded ad when the button is clicked
        AdMobManager.Instance.ShowRewardedAd(OnUserEarnedReward);
    }

    private void OnUserEarnedReward(Reward reward)
    {
        // Reward the user with full health
        var playerController = FindAnyObjectByType<PlayerController>();
        if (playerController != null)
        {
            playerController.health = playerController.maxHealth;
            Debug.Log("User rewarded with full health.");
        }
    }
}
