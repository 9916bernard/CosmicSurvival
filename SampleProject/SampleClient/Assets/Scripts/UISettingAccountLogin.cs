using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using Cysharp.Threading.Tasks;
using Firebase;

public class UISettingAccountLogin : UIBase
{
    [Header("[ Bind Property ]")]
    [SerializeField] private Text _Text_log = null;
    [SerializeField] private InputField _Input_Email = null;
    [SerializeField] private InputField _Input_Password = null;
    [SerializeField] private GameObject _LoginTab = null;  // The login tab to be closed after successful login

    private FirebaseAuth auth;
    private FirebaseFirestore db;
    private FirebaseUser user;

    void Start()
    {
        // Initialize Firebase Auth and Firestore
        auth = FirebaseManager.Instance.Auth;
        db = FirebaseManager.Instance.Firestore;
    }

    public void OnclickLogin()
    {
        string email = _Input_Email.text;
        string password = _Input_Password.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            SetLogMessage("Email or password is empty!");
            return;
        }

        // Sign in the user
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                SetLogMessage("Login was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                HandleFirebaseAuthError(task.Exception);
                return;
            }

            // Firebase user has been signed in
            user = task.Result.User;
            FirebaseManager.Instance.SetCurrentUser(user);

            SetLogMessage($"User signed in successfully: {user.DisplayName} ({user.UserId})");
            UIM.ShowToast($"User signed in successfully: ({user.UserId})");

            // Fetch user data from Firestore and refresh UI after that
            FetchUserDataFromFirestore(user.UserId).Forget();
        });
    }

    private async UniTaskVoid FetchUserDataFromFirestore(string userId)
    {
        bool recordDataFetched = false;
        bool fundDataFetched = false;
        bool upgradeDataFetched = false;

        // Fetch record data
        var recordRef = db.Collection("users").Document(userId).Collection("record").Document("data");
        var recordSnapshot = await recordRef.GetSnapshotAsync();
        if (recordSnapshot.Exists)
        {
            var recordData = recordSnapshot.ToDictionary();
            USER.account.data.AccountName = recordData["userName"].ToString();
            USER.player.SetRecord(Convert.ToInt32(recordData["bestRecord"]));
            recordDataFetched = true;
        }
        else
        {
            Debug.LogError("Record data not found in Firestore.");
        }

        // Fetch fund data
        var fundRef = db.Collection("users").Document(userId).Collection("fund").Document("data");
        var fundSnapshot = await fundRef.GetSnapshotAsync();
        if (fundSnapshot.Exists)
        {
            var fundData = fundSnapshot.ToDictionary();
            USER.fund.SetFund(ETB_FUND.GOLD, Convert.ToInt32(fundData["gold"]));
            USER.fund.SetFund(ETB_FUND.MINERAL_RUBY, Convert.ToInt32(fundData["ruby"]));
            USER.fund.SetFund(ETB_FUND.MINERAL_PEARL, Convert.ToInt32(fundData["pearl"]));
            USER.fund.SetFund(ETB_FUND.MINERAL_AMBER, Convert.ToInt32(fundData["amber"]));
            USER.fund.SetFund(ETB_FUND.MINERAL_SAPPAIRE, Convert.ToInt32(fundData["sappaire"]));
            USER.fund.SetFund(ETB_FUND.MINERAL_BLACK, Convert.ToInt32(fundData["black"]));
            fundDataFetched = true;
        }
        else
        {
            Debug.LogError("Fund data not found in Firestore.");
        }

        // Fetch upgrade data
        var upgradeRef = db.Collection("users").Document(userId).Collection("upgrade").Document("data");
        var upgradeSnapshot = await upgradeRef.GetSnapshotAsync();
        if (upgradeSnapshot.Exists)
        {
            var upgradeData = upgradeSnapshot.ToDictionary();
            USER.upgrade.SetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.PLAYER_ATK, Convert.ToInt32(upgradeData["ATK Level"]));
            USER.upgrade.SetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.PLAYER_HP, Convert.ToInt32(upgradeData["HP Level"]));
            USER.upgrade.SetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.PLAYER_SPD, Convert.ToInt32(upgradeData["Spd Level"]));
            USER.upgrade.SetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.PLAYER_FIRERATE, Convert.ToInt32(upgradeData["FireRt Level"]));
            USER.upgrade.SetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.PLAYER_DURATION, Convert.ToInt32(upgradeData["Durat Level"]));
            USER.upgrade.SetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.PLAYER_PRJNUM, Convert.ToInt32(upgradeData["PRJNUM Level"]));
            USER.upgrade.SetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.STATION_ATK, Convert.ToInt32(upgradeData["Station ATK Level"]));
            USER.upgrade.SetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.STATION_FIRERT, Convert.ToInt32(upgradeData["Station FireRt Level"]));
            USER.upgrade.SetUpgradeEffectLevel(ETB_UPGRADE_EFFECT.STATION_PRJNUM, Convert.ToInt32(upgradeData["Station PRJNUM Level"]));
            upgradeDataFetched = true;
        }
        else
        {
            Debug.LogError("Upgrade data not found in Firestore.");
        }

        // Refresh UI after all data has been fetched
        if (recordDataFetched && fundDataFetched && upgradeDataFetched)
        {
            UIM.Inst().RefreshUI(EUI_RefreshType.ACCOUNT, EUI_RefreshType.FUND, EUI_RefreshType.PLAYER);
        }

        CloseLoginTab();
    }

    private void HandleFirebaseAuthError(AggregateException exception)
    {
        foreach (var innerException in exception.InnerExceptions)
        {
            if (innerException is FirebaseException firebaseException)
            {
                var errorCode = (AuthError)firebaseException.ErrorCode;
                switch (errorCode)
                {
                    case AuthError.InvalidEmail:
                        SetLogMessage("Invalid email address.");
                        break;
                    case AuthError.WrongPassword:
                        SetLogMessage("Incorrect password.");
                        break;
                    case AuthError.UserNotFound:
                        SetLogMessage("User not found.");
                        break;
                    case AuthError.UserDisabled:
                        SetLogMessage("User account is disabled.");
                        break;
                    case AuthError.TooManyRequests:
                        SetLogMessage("Too many requests. Try again later.");
                        break;
                    default:
                        SetLogMessage("Login failed. Please try again.");
                        break;
                }
            }
        }
    }

    private void SetLogMessage(string message)
    {
        if (_Text_log != null)
        {
            _Text_log.text = message;
        }
    }

    private void CloseLoginTab()
    {
        if (_LoginTab != null)
        {
            _LoginTab.SetActive(false);
        }
    }
}
