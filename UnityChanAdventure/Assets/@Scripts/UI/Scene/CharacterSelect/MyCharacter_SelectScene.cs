using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class MyCharacter_SelectScene : UI_Scene,IListener
{
    public int Charcode=-1;
    private bool isinit = false;
    public override void Init()
    {
        if (isinit) return;
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));


        GetText((int)Texts.MyCharacterInfo_Text1).text = "";
        GetText((int)Texts.MyCharacterInfo_Text2).text = "";
        GetText((int)Texts.MyCharacterInfo_Text3).text = "";
        GetText((int)Texts.MyCharacterInfo_Text4).text = "";
        gameObject.BindEvent((PointerEventData data) => Managers.Event.PostNotification(Define.EVENT_TYPE.SelectCharacter,this));
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
        GetText((int)Texts.MyCharacterInfo_Text3).text = $"만든날 : {DateTime.Parse(Managers.Data.CharacterDataDict[charcode].dateTime)}";

    }
    public void RefreshUI()
    {
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
    }

    public void OnEvent(Define.EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
       if(Event_Type == Define.EVENT_TYPE.SelectCharacter)
        {
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
