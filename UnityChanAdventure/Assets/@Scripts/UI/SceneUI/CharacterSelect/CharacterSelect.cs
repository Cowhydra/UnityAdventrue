using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CharacterSelect : UI_Scene,IListener
{

    private int SelectCharcter=-1;
    enum GameObjects
    {
        MakeCharacter,

    }
    enum InputFields
    {
        CharacterName_InputField,

    }
    enum Buttons
    {
        QuitButton,
        SettingButton,
        DeleteButton,
        StartButton,

        MakeCharacterConfirmButton,
        MakeCharacterCancelButton,
    }

    private void Start()
    {
        Init();
        GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
     
    }
    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<TMP_InputField>(typeof(InputFields));
        Bind<GameObject>(typeof(GameObjects));
        Managers.Event.AddListener(Define.EVENT_TYPE.SelectCharacter, this);

        InitButton();

        GetObject((int)GameObjects.MakeCharacter).SetActive(false);

    }
    private void InitButton()
    {
        GetButton((int)Buttons.QuitButton).gameObject
            .BindEvent((PointerEventData data) => Managers.Scene.LoadScene(Define.Scene.LoginScene));
        GetButton((int)Buttons.SettingButton).gameObject
            .BindEvent((PointerEventData data) => ShowSettingMenu());
        GetButton((int)Buttons.StartButton).gameObject
           .BindEvent((PointerEventData data) => GameStart());
        GetButton((int)Buttons.DeleteButton).gameObject
           .BindEvent((PointerEventData data) => DeleCharacter());
        GetButton((int)Buttons.MakeCharacterCancelButton).gameObject
            .BindEvent((PointerEventData data) => GetObject((int)GameObjects.MakeCharacter).SetActive(false));
        GetButton((int)Buttons.MakeCharacterConfirmButton).gameObject
          .BindEvent((PointerEventData data) =>CreateCharacter());

    }
    private void CreateCharacter()
    {
        Debug.Log("캐릭터 추가");
        Managers.DB.CharacterInit(Managers.Game.AccountNumber.ToString(), SelectCharcter,
            Get<TMP_InputField>((int)InputFields.CharacterName_InputField).text);
        GetObject((int)GameObjects.MakeCharacter).SetActive(false);
    }
    private void DeleCharacter()
    {
        if (SelectCharcter == 0)
        {
            Debug.Log("캐릭터를 선택하지 않음!");
        }
        Debug.Log("DB삭제");
        Managers.DB.DeleteCharacter(Managers.Game.AccountNumber.ToString(), SelectCharcter);
    }
    private void GameStart()
    {
        if (Managers.Data.CharacterDataDict[SelectCharcter].isActive)
        {
            Managers.Game.currentCharNumber = SelectCharcter;
            Debug.Log("TownScene로 이동 구현");
        }
    }
    private void ShowSettingMenu()
    {
        Debug.Log("Setting 추후에 기능 만든다면 여기!");
    }
  
    public void ShowMakeCharacter()
    {
        GetObject((int)GameObjects.MakeCharacter).SetActive(true);
   
    }

    public void OnEvent(Define.EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        if (Event_Type == Define.EVENT_TYPE.SelectCharacter)
        {
            Debug.Log($"{gameObject.name} :SelectCharacter 이벤트 받음 ");
            if (Managers.Data.CharacterDataDict[Sender.GetComponent<MyCharacter_SelectScene>().Charcode].isActive)
            {
              
            }
            else
            {
                ShowMakeCharacter();
              
            }
            SelectCharcter = Sender.GetComponent<MyCharacter_SelectScene>().Charcode;
        }
    }
}
