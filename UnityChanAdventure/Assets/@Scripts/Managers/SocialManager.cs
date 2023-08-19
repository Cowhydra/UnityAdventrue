using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;

public class SocialManager
{

    public void Init()
    {

        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    public void Login()
    {
        Debug.Log("로그인 시도");
        Social.localUser.Authenticate(LoginCallbackGPGS);
    }

    private void LoginCallbackGPGS(bool result)
    {
        if (result)
        {
            Managers.Event.LoginProgess?.Invoke(Define.Login_Event_Type.LoginSucess);
            // SetFirebaseUser();
        }

        else
        {
            Managers.Event.LoginProgess?.Invoke(Define.Login_Event_Type.CreateAccount_Fail_IDSame);
        }
    }

}