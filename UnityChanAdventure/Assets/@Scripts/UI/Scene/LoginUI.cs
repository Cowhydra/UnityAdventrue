using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoginUI : UI_Scene
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
    }
    enum Buttons
    {
        GameStart_Button,
        Join_Button,
        Cancel_Button,
    }
    enum GameObjects
    {
        Login,
        BackGround_Pannel
    }

    public override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<TMP_InputField>(typeof(InputFields));
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
        StartCoroutine(nameof(TextEffect_CO), GetText((int)Texts.LoginUI_Text));
    }
    void Start()
    {
        Init();
        GetButton((int)Buttons.GameStart_Button).gameObject
            .BindEvent((PointerEventData data) => GameStart());
        GetButton((int)Buttons.Join_Button).gameObject
            .BindEvent((PointerEventData data) => Join());
        GetButton((int)Buttons.Cancel_Button).gameObject
            .BindEvent((PointerEventData data) => GetObject((int)GameObjects.Login).SetActive(false));

        GetObject((int)GameObjects.BackGround_Pannel)
            .BindEvent((PointerEventData data) => GetObject((int)GameObjects.Login).SetActive(true));
       
        GetObject((int)GameObjects.Login).SetActive(false);
    }

    //DB���� ȸ�� �ֳ� Ȯ�� ��  ������ ĳ���� ���̴� ������ �Ѿ (Lobby)
    private void GameStart()
    {
        //TODO : DB ���� ����ó��! , DB���� ���̵� ��� Ȯ�� �� ��� Ȥ�� ����� ���� ����

        Debug.Log("DB ���� ó��");
    }
    //ȸ�� ���� InputFeild�� Text �Է°����� ȸ������ �״�� ����
    private void Join()
    {
        if (Get<InputField>((int)InputFields.ID_InputField).text == null || Get<InputField>((int)InputFields.PW_InputField).text == null)
        {
            Debug.Log("���Ŀ� ���â UI � ���� --- ");
            return;
        }
        //TODO : DB ���� ����ó��!

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
}
