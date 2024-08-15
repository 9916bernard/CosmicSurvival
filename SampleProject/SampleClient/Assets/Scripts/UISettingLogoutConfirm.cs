using System;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;

public class UISettingLogoutConfirm : UIBase
{
    protected override void OnOpenStart()
    {
        // Any initialization code can go here
    }

    public void OnClick_No()
    {
        // Close the logout confirmation popup
        Close();
    }

    public void OnClick_Logout()
    {
        // Sign out the user from Firebase Auth
        FirebaseAuth auth = FirebaseManager.Instance.Auth;
        auth.SignOut();

        // Clear the current user in FirebaseManager
        FirebaseManager.Instance.SetCurrentUser(null);

        // Reset local user data
        ResetLocalUserData();

        // Optionally, update the UI or navigate to a different screen
        UIM.ShowToast(41022);
        UIM.Inst().RefreshUI(EUI_RefreshType.ACCOUNT, EUI_RefreshType.FUND, EUI_RefreshType.PLAYER);

        // Close the logout confirmation popup
        Close();
    }

    private void ResetLocalUserData()
    {
        USER.account.data.AccountName = "Guest";

        // Reset funds to 0
        foreach (ETB_FUND fundType in Enum.GetValues(typeof(ETB_FUND)))
        {
            USER.fund.SetFund(fundType, 0);
        }

        // Reset player record to 0
        USER.player.SetRecord(0);

        // Reset upgrade levels to 0
        foreach (ETB_UPGRADE_EFFECT effectType in Enum.GetValues(typeof(ETB_UPGRADE_EFFECT)))
        {
            USER.upgrade.SetUpgradeEffectLevel(effectType, 0);
        }

        // Reset tutorial completion statuses
        USER.player.ResetTutorials();
    }
}
