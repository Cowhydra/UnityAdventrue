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
            // Action을 실행한 후, 지정된 시간 지연이나 조건을 기다릴 수 있습니다.
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
        GetText((int)Texts.MyCharacterInfo_Text1).text = $"이름 : {Managers.Data.CharacterDataDict[charcode].name}";
        GetText((int)Texts.MyCharacterInfo_Text2).text = $"레벨 : {Managers.Data.CharacterDataDict[charcode].level}";
        GetText((int)Texts.MyCharacterInfo_Text3).text = $"경험치 : {Managers.Data.CharacterDataDict[charcode].exp}";
        GetText((int)Texts.MyCharacterInfo_Text4).text = $"만든날 : {DateTime.Parse(Managers.Data.CharacterDataDict[charcode].dateTime)}";

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
                Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("캐릭터가 생성되었습니다.", Color.yellow);
            }
            else
            {

                GetObject((int)GameObjects.NoCharacter_Image).SetActive(true);
                TextInit();
                Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("캐릭터가 삭제되었습니다.", Color.red);

            }
        }
    }

    public void OnEvent(Define.EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
       if(Event_Type == Define.EVENT_TYPE.SelectCharacter)
        {
            Debug.Log($"{gameObject.name} :SelectCharacter 이벤트 받음 ");
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
