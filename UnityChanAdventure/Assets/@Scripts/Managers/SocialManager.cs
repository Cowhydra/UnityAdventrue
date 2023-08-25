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
        Debug.Log("�α��� �õ�");
        // Social.localUser.Authenticate(LoginCallbackGPGS);
        TryGoogleLogin();
    }
    public void TryGoogleLogin()
    {
        if (!Social.localUser.authenticated) // �α��� �Ǿ� ���� �ʴٸ�
        {
            Social.localUser.Authenticate(success => // �α��� �õ�
            {
                if (success) // �����ϸ�
                {
                    SocialID = $"g{((PlayGamesLocalUser)Social.localUser).id}";
                    Debug.Log($"�� ���̵� : {((PlayGamesLocalUser)Social.localUser).id}");
                    Debug.Log($"�� �̸� : {((PlayGamesLocalUser)Social.localUser).userName}");
                }
                else // �����ϸ�
                {
                    Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("�α��� ����", Color.red);
                }
            });
        }
    }














    public void FireBaseLogOut()
    {
        Debug.Log("�α׾ƿ�");
        if (auth.CurrentUser != null)
        {
            auth.SignOut();
            if (myLoginType == Define.LoginType.Google)
            {
                GoogleSignIn.DefaultInstance.SignOut();

            }
            Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("�α� �ƿ�", Color.cyan);

        }
       
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