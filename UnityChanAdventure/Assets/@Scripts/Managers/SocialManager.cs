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
        Debug.Log("�α��� �õ�");
        Social.localUser.Authenticate(LoginCallbackGPGS);

    }

    private void LoginCallbackGPGS(bool result)
    {
        if (result)
        {
            Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("�α��� ����", Color.green);
            SocialID= $"g{((PlayGamesLocalUser)Social.localUser).id}";
            Debug.Log($"�� ���̵� : {((PlayGamesLocalUser)Social.localUser).id}");
            Debug.Log($"�� �̸� : {((PlayGamesLocalUser)Social.localUser).userName}");
            SetFirebaseUser();
        }

        else
        {
            Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("�α��� ����", Color.green);
        }
    }
    private void LogOut()
    {
#if UNITY_ANDROID
       

#endif
    }
    private void SetFirebaseUser()
    {


        Debug.Log("�����Ӽ� ����");
        FirebaseAnalytics.SetUserProperty(FirebaseAnalytics.UserPropertySignUpMethod, "Google");

        // Set the user ID.
        string nickName = SocialID;
        Debug.Log($"�� �г����� : {nickName}");

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