using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using Firebase.Extensions;
using Firebase.Analytics;
using Google;


public partial class SocialManager
{

    [Header("FireBase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;




    private void InitFireBase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseAnalyticsInitialized = true;

            DependencyStatus dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("Enabling data collection.");
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

                Debug.Log("Firebase SetSessionTimeoutDuration");
                FirebaseAnalytics.SetSessionTimeoutDuration(new System.TimeSpan(0, 30, 0));

                Init_auth();

                Debug.Log("파이어베이스 잘 설정됨");

            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }

        });
    }
    void Init_auth()
    {
        //set default instance object
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChage;
        AuthStateChage(this, null);
    }

    // 계정 로그인에 어떠한 변경점이 발생시 진입.
    void AuthStateChage(object sender,System.EventArgs eventargs)
    {
        if (auth.CurrentUser != user)
        {
            // 연동된 계정과 기기의 계정이 같다면 true를 리턴한다
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if (!signedIn && user != null)
            {
                Debug.Log($"Signed out : {user.UserId}");
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log($"Signed in{user.UserId}");
            }
        }
    }
    public void EmailLogin(string email,string password)
    {
        EmainLoginAsync(email, password);
    }
    public void MakeEmailAccount( string email,string password)
    {
        RegisterAsync(email, password, password);
    }
    private void EmainLoginAsync(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(loginTask =>
        {
            if (loginTask.Exception != null)
            {
                Debug.Log(loginTask.Exception);
                FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;

                string fieldMessgae = $"Login Failed Because : {authError.ToString()}";

            }
            else
            {
                user = loginTask.Result.User;
                Debug.Log($"Login Sucessfully {user.DisplayName} ");
                
                Debug.Log("이후 씬 넘어감 추가해야함");
            }
        });

       
    }

    private void RegisterAsync(string email, string password, string confirmPassword)
    {
        if (password == string.Empty||email==string.Empty)
        {
            Debug.Log("does nt allow empty");
        }
        else if (password != confirmPassword)
        {
            Debug.LogError("Password does not match");
        }
        else
        {
            auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(registertask =>
            {
                if (registertask.Exception != null)
                {
                    Debug.LogError(registertask.Exception);

                    Debug.Log(registertask.Exception);
                    FirebaseException firebaseException = registertask.Exception.GetBaseException() as FirebaseException;
                    AuthError authError = (AuthError)firebaseException.ErrorCode;

                    string fieldMessgae = $"Login Failed Because : {authError.ToString()}";
                }
                else
                {
                    Debug.Log($"Registratuion Sucessfull welcome{user.DisplayName}");
                }
            });
        }
    }

    private void GoogleLogin_FireBase()
    {
     

    }

}
