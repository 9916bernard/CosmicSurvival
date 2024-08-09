using System;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Extensions;
using Cysharp.Threading.Tasks;

public class UISetting : UIBase
{
    [Header("[ Bind Property ]")]
    [SerializeField] private Text _Text_Desc = null;
    [SerializeField] private UIToggleList _Toggle_LanguageSelected = null;
    [SerializeField] private InputField _Input_Name = null;
    [SerializeField] private Text _text_islogin = null;

    private FirebaseAuth auth;

    void Start()
    {
        // Check Firebase Auth initialization
        auth = FirebaseAuth.DefaultInstance;
        SetLoginStatus();
    }

    protected override void OnOpenStart()
    {
        Set();
    }

    private void Set()
    {
    
        int nameID = GetOpenParam<int>("Name");

       

        _Toggle_LanguageSelected.Set((int)UIM.Inst().GetCurrentLanguage() - 1);

        // Update login status
        SetLoginStatus();
    }

    private void SetLoginStatus()
    {
        if (auth == null)
        {
            Debug.Log("Firebase Auth is not initialized.");
            return;
        }

        if (_text_islogin == null)
        {
            Debug.Log("Text for login status is not assigned.");
            return;
        }

        FirebaseUser user = FirebaseManager.Instance.CurrentUser;
        if (user != null)
        {
            _text_islogin.text = "Login: OK";
        }
        else
        {
            _text_islogin.text = "Login: No";
        }
    }

    public void OnClick_Confirm()
    {
        string newUsername = _Input_Name.text;
        USER.account.data.AccountName = newUsername;

        FirebaseUser user = FirebaseManager.Instance.CurrentUser;
        if (user != null)
        {
            UpdateUsername(user, newUsername).Forget();
        }

        UIM.Inst().RefreshUI(EUI_RefreshType.ACCOUNT, EUI_RefreshType.FUND, EUI_RefreshType.PLAYER);
        Close();
    }

    private async UniTaskVoid UpdateUsername(FirebaseUser user, string newUsername)
    {
        UserProfile profile = new UserProfile { DisplayName = newUsername };
        await user.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                Debug.Log("Username updated successfully.");
                FirebaseManager.Instance.SaveAllDataToFirestore().Forget();
            }
            else
            {
                Debug.LogError("Failed to update username: " + task.Exception);
            }
        });
    }

    public void OnClick_Toast()
    {
        UIM.ShowToast(11228);
    }

    public void OnClick_Toast2()
    {
        UIM.ShowToast("호옹이");
    }

    public void OnClick_Account()
    {
        UIM.ShowPopup("ui_setting_account", EUI_LoadType.SETTING);
    }

    public void OnClick_Language(int InLanguageIndex)
    {
        _Toggle_LanguageSelected.Set(InLanguageIndex - 1);

        UIM.Inst().ChangeLanguage((EUI_LanguageType)InLanguageIndex);
    }

    protected override void OnRefresh()
    {
        Set();
    }

    public override bool OnBackButton()
    {
        if (UIM.Inst().GetCurrentLanguage() == EUI_LanguageType.ENGLISH)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
