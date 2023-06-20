using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class MyCharacter_SelectScene : UI_Scene,IListener
{
    
    public int Charcode;
    private bool isinit = false;


    #region Init
    private void TextInit()
    {

        GetText((int)Texts.MyCharacterInfo_Text1).text = "";
        GetText((int)Texts.MyCharacterInfo_Text2).text = "";
        GetText((int)Texts.MyCharacterInfo_Text3).text = "";
        GetText((int)Texts.MyCharacterInfo_Text4).text = "";
    }
    public override void Init()
    {
        if (isinit) return;
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        TextInit();

        #region Event
        gameObject.BindEvent((PointerEventData data) => Managers.Event.PostNotification(Define.EVENT_TYPE.SelectCharacter,this));

        Managers.Event.CreateOrDeleteCharacter -= EnActionQueue;
        Managers.Event.CreateOrDeleteCharacter += EnActionQueue;
        Managers.Event.AddListener(Define.EVENT_TYPE.SelectCharacter, this);
        gameObject.BindEvent((PointerEventData Data) => Managers.Event.PostNotification(Define.EVENT_TYPE.SelectCharacter, this));

        #endregion
        GetObject((int)GameObjects.NoCharacter_Image).SetActive(false);
        GetObject((int)GameObjects.MyCharacter_Image).SetActive(false);
        GetObject((int)GameObjects.MyCharacterInfo_TextPannel).SetActive(false);



        if (Charcode != -1)
        {
            if (Managers.Data.CharacterDataDict[Charcode].isActive)
            {
                GetObject((int)GameObjects.MyCharacter_Image).SetActive(true);
                GetObject((int)GameObjects.MyCharacterInfo_TextPannel).SetActive(true);
                SetText(Charcode);
            }
            else
            {
                GetObject((int)GameObjects.NoCharacter_Image).SetActive(true);

            }
        }


        isinit = true;
    }
    #endregion
    #region ActionQueue
    Queue<(Action<int>, int charcode)> ActionQueue = new Queue<(Action<int>, int)>();
    private void Update()
    {
        if (ActionQueue.Count > 0)
        {
            StartCoroutine(ProcessActionQueue());
        }
    }
    private IEnumerator ProcessActionQueue()
    {
        while (ActionQueue.Count > 0)
        {

            (Action<int> action, int charcode) = ActionQueue.Dequeue();
            action?.Invoke(charcode);
            // Action�� ������ ��, ������ �ð� �����̳� ������ ��ٸ� �� �ֽ��ϴ�.
            yield return new WaitForSeconds(1.0f);
        }
    }
    private void EnActionQueue(int charcode)
    {
        ActionQueue.Enqueue((RefreshUI, charcode));
    }
    #endregion

    private void OnDestroy()
    {
        Managers.Event.CreateOrDeleteCharacter -= EnActionQueue;
    }
    enum GameObjects
    {
        NoCharacter_Image,
        MyCharacterInfo_TextPannel,
        MyCharacter_Image
    }
    enum Texts
    {
        MyCharacterInfo_Text1,
        MyCharacterInfo_Text2,
        MyCharacterInfo_Text3,
        MyCharacterInfo_Text4,
    }
    private void Start()
    {
        Init();
    }
    private void SetText(int charcode)
    {
        GetText((int)Texts.MyCharacterInfo_Text1).text = $"�̸� : {Managers.Data.CharacterDataDict[charcode].name}";
        GetText((int)Texts.MyCharacterInfo_Text2).text = $"���� : {Managers.Data.CharacterDataDict[charcode].level}";
        GetText((int)Texts.MyCharacterInfo_Text3).text = $"����ġ : {Managers.Data.CharacterDataDict[charcode].exp}";
        GetText((int)Texts.MyCharacterInfo_Text4).text = $"���糯 : {DateTime.Parse(Managers.Data.CharacterDataDict[charcode].dateTime)}";

    }
    public void RefreshUI(int charcode)
    {
        if (Charcode ==charcode)
        {
            GetObject((int)GameObjects.MyCharacter_Image).SetActive(false);
            GetObject((int)GameObjects.MyCharacterInfo_TextPannel).SetActive(false);
            GetObject((int)GameObjects.NoCharacter_Image).SetActive(false);

            if (Managers.Data.CharacterDataDict[Charcode].isActive)
            {
                GetObject((int)GameObjects.MyCharacter_Image).SetActive(true);
                GetObject((int)GameObjects.MyCharacterInfo_TextPannel).SetActive(true);
                SetText(Charcode);
                Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("ĳ���Ͱ� �����Ǿ����ϴ�.", Color.yellow);
            }
            else
            {

                GetObject((int)GameObjects.NoCharacter_Image).SetActive(true);
                TextInit();
                Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("ĳ���Ͱ� �����Ǿ����ϴ�.", Color.red);

            }
        }
    }

    public void OnEvent(Define.EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
       if(Event_Type == Define.EVENT_TYPE.SelectCharacter)
        {
            Debug.Log($"{gameObject.name} :SelectCharacter �̺�Ʈ ���� ");
            if (Sender.GetComponent<MyCharacter_SelectScene>().Charcode != this.Charcode)
            {
                gameObject.GetComponent<Image>().color = Color.white;
                
            }
            else
            {        
                gameObject.GetComponent<Image>().color = Color.green;
            }
        }
    }
}
