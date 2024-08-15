using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using Cysharp.Threading.Tasks;
using Firebase;
using System;

public class UISettingAccountSignUp : UIBase
{
    [Header("[ Bind Property ]")]
    [SerializeField] private InputField _Input_userName = null;
    [SerializeField] private InputField _Input_email = null;
    [SerializeField] private InputField _Input_password = null;
    [SerializeField] private InputField _Input_password_confirm = null;
    [SerializeField] private Text _Text_log = null;  // Text element to show error messages
    [SerializeField] private GameObject _SignUpTab = null;  // The sign-up tab to be closed

    private FirebaseAuth auth;
    private FirebaseFirestore db;

    void Start()
    {
        // Get Firebase Auth and Firestore from the singleton
        auth = FirebaseManager.Instance.Auth;
        db = FirebaseManager.Instance.Firestore;
    }

    public void OnclickConfirm()
    {
        if (auth.CurrentUser != null)
        {
            SetLogMessage("You are already signed in.");
            return;
        }
        string userName = _Input_userName.text;
        string email = _Input_email.text;
        string password = _Input_password.text;
        string passwordConfirm = _Input_password_confirm.text;

        if (password != passwordConfirm)
        {
            SetLogMessage("Passwords do not match!");
            return;
        }

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            SetLogMessage("Email or password is empty!");
            return;
        }

        if (string.IsNullOrEmpty(userName))
        {
            SetLogMessage("Username is empty!");
            return;
        }

        // Create the user
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(async task => {
            if (task.IsCanceled)
            {
                SetLogMessage("Sign up was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                HandleFirebaseAuthError(task.Exception);
                return;
            }

            // Firebase user has been created
            FirebaseUser user = task.Result.User;
            FirebaseManager.Instance.SetCurrentUser(user);
            // Update user profile with username
            UserProfile profile = new UserProfile { DisplayName = userName };
            await user.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(async updateTask => {
                if (updateTask.IsCanceled)
                {
                    SetLogMessage("Updating user profile was canceled.");
                    return;
                }
                if (updateTask.IsFaulted)
                {
                    HandleFirebaseAuthError(updateTask.Exception);
                    return;
                }

                // Username has been updated
                SetLogMessage($"User created successfully with username: {user.DisplayName} ({user.UserId})");

                USER.account.data.AccountName = userName;

                // Save user data to Firestore
                await FirebaseManager.Instance.SaveAllDataToFirestore();

                UIM.ShowToast(41023);
                CloseSignUpTab();

                // Refresh the UI
                UIM.Inst().RefreshUI(EUI_RefreshType.ACCOUNT, EUI_RefreshType.FUND, EUI_RefreshType.PLAYER);
            });
        });
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
                    case AuthError.EmailAlreadyInUse:
                        SetLogMessage("Email is already in use.");
                        break;
                    case AuthError.WeakPassword:
                        SetLogMessage("Password is too weak.");
                        break;
                    case AuthError.OperationNotAllowed:
                        SetLogMessage("Operation not allowed.");
                        break;
                    case AuthError.TooManyRequests:
                        SetLogMessage("Too many requests. Try again later.");
                        break;
                    default:
                        SetLogMessage("Sign up failed. Please try again.");
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

    private void CloseSignUpTab()
    {
        if (_SignUpTab != null)
        {
            _SignUpTab.SetActive(false);
        }
    }
}
