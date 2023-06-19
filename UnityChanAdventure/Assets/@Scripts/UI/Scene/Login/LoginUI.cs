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

    //DB에서 회원 있나 확인 후  있으면 캐릭터 보이는 씬으로 넘어감 (Lobby)
    private void GameStart()
    {
        //TODO : DB 관련 로직처리! , DB에서 아이디 비번 확인 후 통과 혹은 불통과 여부 결정
       // Managers.DB.ChecK_Account(Get<TMP_InputField>((int)InputFields.ID_InputField).text, Get<TMP_InputField>((int)InputFields.ID_InputField).text);
        Debug.Log("DB 관련 처리");
    }
    //회원 가입 InputFeild의 Text 입력값으로 회원가입 그대로 진행
    private void Join()
    {
        GetObject((int)GameObjects.MakeAccount).SetActive(true);
    }
    private void MakeAccount()
    {
        if (Get<TMP_InputField>((int)InputFields.MakeID_InputField).text == null || Get<TMP_InputField>((int)InputFields.MakePW_InputField).text == null)
        {
            Managers.UI.ShowPopupUI<WarningText>("ID나 비밀번호는 빈칸이 없어야 합니다.");
            return;
        }
        else
        {
            if (Get<TMP_InputField>((int)InputFields.MakeID_InputField).text != null)
            {
                if (Get<TMP_InputField>((int)InputFields.MakePW_InputField).text != null)
                {
                    //Managers.DB.ChecK_Account(Get<TMP_InputField>((int)InputFields.MakeID_InputField).text, Get<TMP_InputField>((int)InputFields.ID_InputField).text);
                       //ID 조회 성공 및 아이디 만들기 + Chearacter 만들기()
                }
                else
                {
                    Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("ID나 비밀번호는 빈칸이 없어야 합니다.",Color.red);
                }
            }
            else
            {
                Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("ID나 비밀번호는 빈칸이 없어야 합니다.",Color.red);

            }

        }
    }


    public void OnEvent(Define.EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {

        switch (Event_Type)
        {
            case Define.EVENT_TYPE.LoginSucess:
                Debug.Log("씬로드");
                break;
            case Define.EVENT_TYPE.LoginFail_ID_NotFound:
                Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("ID를 찾을 수 없습니다.", Color.red);
                break;
            case Define.EVENT_TYPE.LoginFail_PW_Wrong:
                Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("패스워드를 확인해주시길 바랍니다.", Color.red);
                break;
            case Define.EVENT_TYPE.CreateAccount_Sucess:
                Debug.Log("이게 안들어오는거 같은데");
                Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("계정이 생성되었습니다..", Color.green);
                break;
            case Define.EVENT_TYPE.CreateAccount_Fail_IDSame:
                Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("이미 동일한 ID가 존재합니다.", Color.blue);
                break;
        }


    }



    //글자 효과
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
