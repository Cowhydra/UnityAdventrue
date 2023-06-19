using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoginUI : UI_Scene,IListener
{
    private float fadeSpeed = 0.5f;
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

    }
    enum GameObjects
    {
        Login,
        BackGround_Pannel,

        MakeAccount,

    }

    public override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<TMP_InputField>(typeof(InputFields));
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
      

        #region BindEvent
        GetButton((int)Buttons.GameStart_Button).gameObject
         .BindEvent((PointerEventData data) => GameStart());
        GetButton((int)Buttons.Join_Button).gameObject
            .BindEvent((PointerEventData data) => Join());
        GetButton((int)Buttons.Cancel_Button).gameObject
            .BindEvent((PointerEventData data) => GetObject((int)GameObjects.Login).SetActive(false));
        GetButton((int)Buttons.MakeCancel_Button).gameObject
            .BindEvent((PointerEventData data) => GetObject((int)GameObjects.MakeAccount).SetActive(false));


        GetObject((int)GameObjects.BackGround_Pannel)
            .BindEvent((PointerEventData data) => GetObject((int)GameObjects.Login).SetActive(true));
        


        Managers.Event.AddListener(Define.EVENT_TYPE.CreateAccount_Fail_IDSame, this);
        Managers.Event.AddListener(Define.EVENT_TYPE.CreateAccount_Sucess, this);
        Managers.Event.AddListener(Define.EVENT_TYPE.LoginFail_ID_NotFound, this);
        Managers.Event.AddListener(Define.EVENT_TYPE.LoginFail_PW_Wrong, this);
        Managers.Event.AddListener(Define.EVENT_TYPE.LoginSucess, this);


        #endregion
    }
    void Start()
    {
        Init();


        StartCoroutine(nameof(TextEffect_CO), GetText((int)Texts.LoginUI_Text));
        GetObject((int)GameObjects.Login).SetActive(false);
        GetObject((int)GameObjects.MakeAccount).SetActive(false);

    }

    //DB���� ȸ�� �ֳ� Ȯ�� ��  ������ ĳ���� ���̴� ������ �Ѿ (Lobby)
    private void GameStart()
    {
        //TODO : DB ���� ����ó��! , DB���� ���̵� ��� Ȯ�� �� ��� Ȥ�� ����� ���� ����
       // Managers.DB.ChecK_Account(Get<TMP_InputField>((int)InputFields.ID_InputField).text, Get<TMP_InputField>((int)InputFields.ID_InputField).text);
        Debug.Log("DB ���� ó��");
    }
    //ȸ�� ���� InputFeild�� Text �Է°����� ȸ������ �״�� ����
    private void Join()
    {
        GetObject((int)GameObjects.MakeAccount).SetActive(true);
    }
    private void MakeAccount()
    {
        if (Get<TMP_InputField>((int)InputFields.MakeID_InputField).text == null || Get<TMP_InputField>((int)InputFields.MakePW_InputField).text == null)
        {
            Managers.UI.ShowPopupUI<WarningText>("ID�� ��й�ȣ�� ��ĭ�� ����� �մϴ�.");
            return;
        }
        else
        {
            if (Get<TMP_InputField>((int)InputFields.MakeID_InputField).text != null)
            {
                if (Get<TMP_InputField>((int)InputFields.MakePW_InputField).text != null)
                {
                    //Managers.DB.ChecK_Account(Get<TMP_InputField>((int)InputFields.MakeID_InputField).text, Get<TMP_InputField>((int)InputFields.ID_InputField).text);
                       //ID ��ȸ ���� �� ���̵� ����� + Chearacter �����()
                }
                else
                {
                    Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("ID�� ��й�ȣ�� ��ĭ�� ����� �մϴ�.",Color.red);
                }
            }
            else
            {
                Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("ID�� ��й�ȣ�� ��ĭ�� ����� �մϴ�.",Color.red);

            }

        }
    }


    public void OnEvent(Define.EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {

        switch (Event_Type)
        {
            case Define.EVENT_TYPE.LoginSucess:
                Debug.Log("���ε�");
                break;
            case Define.EVENT_TYPE.LoginFail_ID_NotFound:
                Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("ID�� ã�� �� �����ϴ�.", Color.red);
                break;
            case Define.EVENT_TYPE.LoginFail_PW_Wrong:
                Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("�н����带 Ȯ�����ֽñ� �ٶ��ϴ�.", Color.red);
                break;
            case Define.EVENT_TYPE.CreateAccount_Sucess:
                Debug.Log("�̰� �ȵ����°� ������");
                Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("������ �����Ǿ����ϴ�..", Color.green);
                break;
            case Define.EVENT_TYPE.CreateAccount_Fail_IDSame:
                Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("�̹� ������ ID�� �����մϴ�.", Color.blue);
                break;
        }


    }



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

    private IEnumerator LoadingBackGroundEffect_co()
    {
        yield return new WaitForSeconds(2.0f);
    }
}
