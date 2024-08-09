using System;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Google;
using System.Threading.Tasks;

public class UISettingSignUp : UIBase
{
    private FirebaseAuth auth;
    //private GoogleSignInConfiguration configuration;

    protected override void OnOpenStart()
    {
        auth = FirebaseAuth.DefaultInstance;

        //configuration = new GoogleSignInConfiguration
        //{
        //    WebClientId = "YOUR_WEB_CLIENT_ID", // Replace with your web client ID
        //    RequestEmail = true,
        //    RequestIdToken = true
        //};
    }

    public void OnClick_Google()
    {
        //SignInWithGoogle();
    }

    //private void SignInWithGoogle()
    //{
    //    GoogleSignIn.Configuration = configuration;
    //    GoogleSignIn.Configuration.UseGameSignIn = false;
    //    GoogleSignIn.Configuration.RequestIdToken = true;

    //    GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleSignIn);
    //}

    //private void OnGoogleSignIn(Task<GoogleSignInUser> task)
    //{
    //    if (task.IsCanceled)
    //    {
    //        Debug.LogError("Google sign-in canceled.");
    //        return;
    //    }
    //    if (task.IsFaulted)
    //    {
    //        Debug.LogError("Google sign-in encountered an error: " + task.Exception);
    //        return;
    //    }

    //    GoogleSignInUser googleUser = task.Result;
    //    Credential credential = GoogleAuthProvider.GetCredential(googleUser.IdToken, null);

    //    auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
    //    {
    //        if (authTask.IsCanceled)
    //        {
    //            Debug.LogError("Firebase sign-in canceled.");
    //            return;
    //        }
    //        if (authTask.IsFaulted)
    //        {
    //            Debug.LogError("Firebase sign-in encountered an error: " + authTask.Exception);
    //            return;
    //        }

    //        FirebaseUser newUser = authTask.Result;
    //        Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);

    //        // Handle user sign-up or login here
    //        HandleUserLoginOrSignup(newUser);
    //    });
    //}

    private void HandleUserLoginOrSignup(FirebaseUser user)
    {
        // Add your logic to handle user sign-up or login here.
        // For example, save user data to Firestore, update UI, etc.
    }

    public void OnClick_Email()
    {
        UIM.ShowPopup("ui_setting_signup_email", EUI_LoadType.SETTING);
    }
}
