using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class LoginUI : UI_Scene
{
    private float fadeSpeed = 0.5f;
    #region Bind Objects
    enum Texts
    {
        LoginTitle_Text,

        IDTitle_Text,

        PWTitle_Text,

        LoginUI_Text
    }
    enum InputFields
    {
        ID_InputField,
        PW_InputField,

        MakeID_InputField,
        MakePW_InputField,

    }
    enum Buttons
    {
        GameStart_Button,
        Join_Button,
        Cancel_Button,

        MakeSummit_Button,
        MakeCancel_Button,
        Purchase_Cancel_Button,
        Limit119000,
        NonLimit119000,
        Limit49000,
        NonLimit49000,
       
        GoogleLogin,
        OpenShop,
        GoogleLogOut,
        AnonyLogin
            ,
        Gpgs_Button,

    }
    enum GameObjects
    {
        Login,
        BackGround_Pannel,

        MakeAccount,
        Purchase_Pannel,


    }
    #endregion
    public override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<TMP_InputField>(typeof(InputFields));
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
        ButtonInit();

        #region BindEvent

        GetObject((int)GameObjects.BackGround_Pannel)
            .BindEvent((PointerEventData data) => GetObject((int)GameObjects.Login).SetActive(true));

        Managers.Event.LoginProgess -= Login_Event;
        Managers.Event.LoginProgess += Login_Event;

        #endregion
    }
    private void OnDestroy()
    {
        Managers.Event.LoginProgess -= Login_Event;
    }
    private void ButtonInit()
    {
        GetButton((int)Buttons.GameStart_Button).gameObject
         .BindEvent((PointerEventData data) => Managers.SocialManager.EmailLogin(Get<TMP_InputField>((int)InputFields.ID_InputField).text, Get<TMP_InputField>((int)InputFields.PW_InputField).text));
        GetButton((int)Buttons.Join_Button).gameObject
            .BindEvent((PointerEventData data) => GetObject((int)GameObjects.MakeAccount).SetActive(true));
        GetButton((int)Buttons.Cancel_Button).gameObject
            .BindEvent((PointerEventData data) => GetObject((int)GameObjects.Login).SetActive(false));
        GetButton((int)Buttons.Purchase_Cancel_Button).gameObject
            .BindEvent((PointerEventData data) => GetObject((int)GameObjects.Purchase_Pannel).SetActive(false));
        GetButton((int)Buttons.OpenShop).gameObject
            .BindEvent((PointerEventData data) => GetObject((int)GameObjects.Purchase_Pannel).SetActive(true));
        //����
        GetButton((int)Buttons.GoogleLogin).gameObject.BindEvent((PointerEventData data) => GameStart_WithGoogle());
        GetButton((int)Buttons.Limit119000).gameObject
            .BindEvent((PointerEventData data) => Managers.IAPManager.BuyProductID(IAPManager._Android_limit11900));
        GetButton((int)Buttons.NonLimit119000).gameObject
            .BindEvent((PointerEventData data) => Managers.IAPManager.BuyProductID(IAPManager._Android_nonlimit11900));
        GetButton((int)Buttons.Limit49000).gameObject
            .BindEvent((PointerEventData data) => Managers.IAPManager.BuyProductID(IAPManager._Android_limit49000));
        GetButton((int)Buttons.NonLimit49000).gameObject
    .BindEvent((PointerEventData data) => Managers.IAPManager.BuyProductID(IAPManager._Android_nonlimit49000));
         GetButton((int)Buttons.MakeCancel_Button).gameObject.BindEvent((PointerEventData data) => GetObject((int)GameObjects.MakeAccount).SetActive(false));
        GetButton((int)Buttons.AnonyLogin).gameObject.BindEvent
            ((PointerEventData data) => Managers.SocialManager.AnonyLogin());
        GetButton((int)Buttons.Gpgs_Button).gameObject
            .BindEvent((PointerEventData data) => Managers.SocialManager.GpgsLogin());

         GetButton((int)Buttons.MakeSummit_Button).gameObject.BindEvent((PointerEventData data) => MakeAccount());
        GetButton((int)Buttons.GoogleLogOut).gameObject.BindEvent((PointerEventData data) => Managers.SocialManager.FireBaseLogOut());
    }
   

    void Start()
    {
        Init();


        StartCoroutine(nameof(TextEffect_CO), GetText((int)Texts.LoginUI_Text));
        GetObject((int)GameObjects.Login).SetActive(false);
        GetObject((int)GameObjects.MakeAccount).SetActive(false);
        GetObject((int)GameObjects.Purchase_Pannel).SetActive(false);

    }

    private void GameStart_WithGoogle()
    {
        Managers.SocialManager.SignInGoogle_Firebase();
    }

    //DB���� ȸ�� �ֳ� Ȯ�� ��  ������ ĳ���� ���̴� ������ �Ѿ (Lobby)
    private void GameStart_WithDB()
    {
        //TODO : DB ���� ����ó��! , DB���� ���̵� ��� Ȯ�� �� ��� Ȥ�� ����� ���� ����
        Managers.DB.ChecK_Account(Get<TMP_InputField>((int)InputFields.ID_InputField).text, Get<TMP_InputField>((int)InputFields.PW_InputField).text);
        Debug.Log("DB ���� ó��");
    }
    //ȸ�� ���� InputFeild�� Text �Է°����� ȸ������ �״�� ����
    private void Join()
    {
        GetObject((int)GameObjects.MakeAccount).SetActive(true);
    }
    private void MakeAccount()
    {
        #region DB Login
        //if (Get<TMP_InputField>((int)InputFields.MakeID_InputField).text == string.Empty || Get<TMP_InputField>((int)InputFields.MakePW_InputField).text == string.Empty)
        //{
        //    Managers.Event.LoginProgess?.Invoke(Define.Login_Event_Type.LoginNotBlink);
        //    return;
        //}
        //else
        //{
        //    if (Get<TMP_InputField>((int)InputFields.MakeID_InputField).text != string.Empty)
        //    {
        //        if (Get<TMP_InputField>((int)InputFields.MakePW_InputField).text != string.Empty)
        //        {
        //            Debug.Log($"{Get<TMP_InputField>((int)InputFields.MakeID_InputField).text}");
        //            Debug.Log($"{Get<TMP_InputField>((int)InputFields.MakePW_InputField).text}");

        //            Managers.DB.CheckAccountID(Get<TMP_InputField>((int)InputFields.MakeID_InputField).text, Get<TMP_InputField>((int)InputFields.MakePW_InputField).text);
        //            //ID ��ȸ ���� �� ���̵� ����� + Chearacter �����()
        //            Debug.Log("��������");
        //        }
        //        else
        //        {
        //            Managers.Event.LoginProgess?.Invoke(Define.Login_Event_Type.LoginNotBlink);
        //        }
        //    }
        //    else
        //    {
        //        Managers.Event.LoginProgess?.Invoke(Define.Login_Event_Type.LoginNotBlink);

        //    }

        //}
        #endregion
        Managers.SocialManager.MakeEmailAccount(Get<TMP_InputField>((int)InputFields.MakeID_InputField).text, Get<TMP_InputField>((int)InputFields.MakePW_InputField).text);
    }

    #region ACtion���� �޾Ƽ� �̺�Ʈ ó�� ( �̷��� ���ϴϱ�, DB �����ɸ��� �۾��� �� �Լ� ���� 
    private Queue<(Action<Define.Login_Event_Type>, Define.Login_Event_Type)> ActionQueue = new Queue<(Action<Define.Login_Event_Type>, Define.Login_Event_Type)>();
    private void EnqueueAction(Action<Define.Login_Event_Type> action, Define.Login_Event_Type eventType)
    {
        ActionQueue.Enqueue((action, eventType));
    }
    private void Login_Event(Define.Login_Event_Type EventType)
    {
        switch (EventType)
        {
            case Define.Login_Event_Type.LoginSucess:
                EnqueueAction(action =>
                {
                    Managers.Game.AccountNumber = Get<TMP_InputField>((int)InputFields.ID_InputField).text;
                    Managers.DB.DataFetch(Managers.Game.AccountNumber);
                    Managers.Scene.LoadScene(Define.Scene.CharacterSelectScene);
                    Debug.Log("���ε�");
                }, EventType);
                break;
            case Define.Login_Event_Type.LoginFail_ID_NotFound:
                EnqueueAction(action =>
                {
                    Debug.Log("�̺�Ʈ �Գ���??");
                    Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("ID�� ã�� ���߽��ϴ�. ȸ������ ���ֽñ� �ٶ��ϴ�.", Color.red);
                }, EventType);
                break;
            case Define.Login_Event_Type.LoginFail_PW_Wrong:
                EnqueueAction(action =>
                {
                    Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("�н����带 Ȯ�����ֽñ� �ٶ��ϴ�.", Color.red);
                }, EventType);
                break;
            case Define.Login_Event_Type.CreateAccount_Sucess:
                EnqueueAction(action =>
                {
                    Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("������ �����Ǿ����ϴ�.. \nȭ������ ���ư� �α����� �������ֽñ� �ٶ��ϴ�.", Color.green);
                }, EventType);
                break;
            case Define.Login_Event_Type.CreateAccount_Fail_IDSame:
                EnqueueAction(action =>
                {
                    Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("�̹� ������ ID�� �����մϴ�.", Color.blue);
                }, EventType);
                break;
            case Define.Login_Event_Type.LoginNotBlink:
                EnqueueAction(action =>
                {
                    Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("��ĭ�� �Է����� ���ʽÿ�.", Color.red);
                }, EventType);
                break;
        }
    }

    private void Update()
    {
        if( ActionQueue.Count > 0)
        {
            StartCoroutine(ProcessActionQueue());
        }
    }
  

    private IEnumerator ProcessActionQueue()
    {
        while (ActionQueue.Count > 0)
        {
            (Action<Define.Login_Event_Type> action, Define.Login_Event_Type eventType) = ActionQueue.Dequeue();
            action?.Invoke(eventType);

            // Action�� ������ ��, ������ �ð� �����̳� ������ ��ٸ� �� �ֽ��ϴ�.
            yield return new WaitForSeconds(1.0f);
        }
    }
    #endregion
    //���� ȿ��
    #region Effects
    private IEnumerator TextEffect_CO(TextMeshProUGUI Text)
    {
        while (true)
        {
            yield return FadeOut(Text);
            yield return FadeIn(Text);
        }
    }
    private IEnumerator FadeOut(TextMeshProUGUI text)
    {
        float t = 1f;
        while (t >= 0f)
        {
            Color textColor = text.color;
            textColor.a = Mathf.Lerp(1, 0, t);
            text.color = textColor;
            t -= fadeSpeed * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FadeIn(TextMeshProUGUI text)
    {
        float t = 0f;
        while (t <= 1f)
        {
            Color textColor = text.color;
            textColor.a = Mathf.Lerp(0, 1, t);
            text.color = textColor;
            t += fadeSpeed * Time.deltaTime;
            yield return null;
        }
    }

    #endregion

 
}
