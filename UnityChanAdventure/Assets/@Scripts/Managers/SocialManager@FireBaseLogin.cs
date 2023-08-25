using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using Firebase.Extensions;
using Firebase.Analytics;
using Google;
using System.Threading.Tasks;

public partial class SocialManager
{

    [Header("FireBase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

    private readonly string GOOGLE_CLIENT_ID = "966677147936-7jh3kv5j8no4s9kj1btflkml997njf2o.apps.googleusercontent.com";


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
#if UNITY_ANDROID
       GoogleLogin_init();
#endif
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
    #region Email Login
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

                Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("로그인 성공", Color.green);
                SocialID = user.UserId;
                Debug.Log($"내 아이디 : {user.UserId}");
                Debug.Log($"내 이름 : {user.ProviderId}");

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
    #endregion
    #region googleLogin
    private void GoogleLogin_init()
    {
        GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
            RequestIdToken = true,
            // Copy this value from the google-service.json file.
            // oauth_client with type == 3
            RequestEmail = true,
            WebClientId = GOOGLE_CLIENT_ID
        };
    }
    public void SignInGoogle_Firebase()
    {
        GoogleSignIn.DefaultInstance.SignIn().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("GoogleSignIn was canceled.");
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("GoogleSignIn was error.");
            }
            else
            {
                Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(((Task<GoogleSignInUser>)task).Result.IdToken, null);
                auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("SignInWithCredentialAsync was canceled.");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                        return;
                    }
                    Firebase.Auth.FirebaseUser newUser = task.Result;
                    SignUpServerWithFirebaseToken(newUser);

                    SocialID = newUser.UserId;
                    Debug.Log($" user id : {SocialID}");
                    Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("로그인 성공", Color.cyan);
                    myLoginType = Define.LoginType.Google;
                });
            }
        });
        

    }
    void SignUpServerWithFirebaseToken(Firebase.Auth.FirebaseUser user)
    {
        user.TokenAsync(true).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("TokenAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("TokenAsync encountered an error: " + task.Exception);
                return;
            }
            string token = task.Result;
            Debug.Log($" create token : {token} ");

        });
    }
    public void UpdateToken()
    {
        auth.CurrentUser.TokenAsync(true).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("TokenAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("TokenAsync encountered an error: " + task.Exception);
                return;
            }
            string token = task.Result;
            Debug.Log($" update token : {token} ");
        });
    }
    #endregion

    #region 익명
    public void AnonyLogin()
    {
        // 익명 로그인 진행
        auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            // 익명 로그인 연동 결과
            Firebase.Auth.FirebaseUser newUser = task.Result.User;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("로그인 성공", Color.green);

            Debug.Log("게스트 데이터의 경우 로그아웃 하면 바로 사라짐 -> 만약 실전에서 쓴다면 " +
                "PlayerPref? 여기다 저장해두고 써야할듯");

        });
    }
    #endregion

}
