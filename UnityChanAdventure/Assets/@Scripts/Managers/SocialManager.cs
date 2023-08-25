using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using Firebase;
using Firebase.Extensions;
using Firebase.Analytics;
using Google;

public partial class SocialManager 
{
    public bool FirebaseAnalyticsInitialized { get; private set; }
    public string SocialID;
    public Define.LoginType myLoginType = Define.LoginType.None;
    public void Init()
    {


        InitFireBase();


    }

    public void GpgsLogin()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        Debug.Log("로그인 시도");
        // Social.localUser.Authenticate(LoginCallbackGPGS);
        TryGoogleLogin();
    }
    public void TryGoogleLogin()
    {
        if (!Social.localUser.authenticated) // 로그인 되어 있지 않다면
        {
            Social.localUser.Authenticate(success => // 로그인 시도
            {
                if (success) // 성공하면
                {
                    SocialID = $"g{((PlayGamesLocalUser)Social.localUser).id}";
                    Debug.Log($"내 아이디 : {((PlayGamesLocalUser)Social.localUser).id}");
                    Debug.Log($"내 이름 : {((PlayGamesLocalUser)Social.localUser).userName}");
                }
                else // 실패하면
                {
                    Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("로그인 실패", Color.red);
                }
            });
        }
    }














    public void FireBaseLogOut()
    {
        Debug.Log("로그아웃");
        if (auth.CurrentUser != null)
        {
            auth.SignOut();
            if (myLoginType == Define.LoginType.Google)
            {
                GoogleSignIn.DefaultInstance.SignOut();

            }
            Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("로그 아웃", Color.cyan);

        }
       
    }





    private void LoginCallbackGPGS(bool result)
    {
        if (result)
        {
            Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("로그인 성공", Color.green);
            SocialID= $"g{((PlayGamesLocalUser)Social.localUser).id}";
            Debug.Log($"내 아이디 : {((PlayGamesLocalUser)Social.localUser).id}");
            Debug.Log($"내 이름 : {((PlayGamesLocalUser)Social.localUser).userName}");
            SetFirebaseUser();
        }

        else
        {
            Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("로그인 실패", Color.green);
        }
    }

    private void SetFirebaseUser()
    {


        Debug.Log("유저속성 설정");
        FirebaseAnalytics.SetUserProperty(FirebaseAnalytics.UserPropertySignUpMethod, "Google");

        // Set the user ID.
        string nickName = SocialID;
        Debug.Log($"내 닉네임은 : {nickName}");

        FirebaseAnalytics.SetUserId(nickName);
        Debug.LogFormat("{0} User FireBaseSet", nickName);
        SetLoginEvent();
    }
    private void SetLoginEvent()
    {
        string nickName = SocialID;
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin, nickName, 0);

    }
    
}