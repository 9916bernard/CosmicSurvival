using GoogleMobileAds.Api;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleGameOver : UIBase
{
    [Header("[ Bind Property ]")]
    [SerializeField] private Text _Text_Best_Record = null;
    [SerializeField] private Text _Text_Record = null;
    [SerializeField] private Text _Text_Value = null;
    [SerializeField] private UIButton _Button_Ad = null; // Make sure this button is linked in the inspector
    private Action exitAction;
    private BattleManager battleManager;

    private bool adsDisabled; // Flag to track if ads are disabled

    private void Awake()
    {
        _Button_Ad.onClick.AddListener(OnAdButtonClick);
    }

    protected override void OnOpenStart()
    {
        exitAction = GetOpenParam<Action>("ExitAction");
        battleManager = FindAnyObjectByType<BattleManager>();
        Time.timeScale = 0f; // Pause the game

        int totalSeconds = (int)USER.player.GetRecord();
        TimeSpan timeSpan = TimeSpan.FromSeconds(totalSeconds);
        string formattedTime = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        _Text_Best_Record.SetText($"{formattedTime}");

        int totalCurrentSeconds = (int)battleManager.battleTime;
        TimeSpan timeCurrentSpan = TimeSpan.FromSeconds(totalCurrentSeconds);
        string formattedCurrentTime = string.Format("{0:D2}:{1:D2}", timeCurrentSpan.Minutes, timeCurrentSpan.Seconds);
        _Text_Record.SetText($"{formattedCurrentTime}");

        var totalAstroidsValue = 200 * (battleManager.Ruby + battleManager.Amber + battleManager.Sapphire + battleManager.pearl + battleManager.Emerald + battleManager.Black);
        _Text_Value.SetText($"{battleManager.gold + totalAstroidsValue}");

        // Check if ads are disabled
        adsDisabled = !AdMobManager.Instance.AreAdsEnabled();
    }

    private void OnAdButtonClick()
    {
        if (battleManager.HasUsedRevive())
        {
            UIM.ShowToast(41024);
            return;
        }
        else if (adsDisabled )
        {

            // If ads are disabled or revive has been used, revive the player directly
            OnAdWatched(null);
        }
        else if(AdMobManager.Instance != null)
        {
            AdMobManager.Instance.ShowRewardedAd(OnAdWatched);
        }
    }

    private void OnAdWatched(Reward reward)
    {
        Debug.Log("Rewarded ad watched, restoring player's health.");
        battleManager.RestorePlayerHealth();
        battleManager.SetReviveUsed(true); // Mark revive as used
        // Optionally, you can close the game over UI or resume the game
        Time.timeScale = 1f; // Resume the game
        this.Close();
    }

    public void OnRestartButtonClick()
    {
        exitAction();
        this.Close();
    }

    private void Set()
    {
        // Additional UI setup if necessary
    }

    protected override void OnRefresh()
    {
        Set();
    }

    public override bool OnBackButton()
    {
        return true;
    }
}
