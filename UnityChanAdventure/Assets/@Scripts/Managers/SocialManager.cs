using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using Firebase;
using Firebase.Extensions;
using Firebase.Analytics;

public partial class SocialManager 
{
    public bool FirebaseAnalyticsInitialized { get; private set; }
    public string SocialID;

    public void Init()
    {


        InitFireBase();


    }

    public void Login()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        Debug.Log("로그인 시도");
        Social.localUser.Authenticate(LoginCallbackGPGS);

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
    private void LogOut()
    {
#if UNITY_ANDROID
       

#endif
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