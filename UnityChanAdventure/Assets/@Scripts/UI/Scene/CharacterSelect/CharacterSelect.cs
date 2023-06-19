using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharacterSelect : UI_Scene,IListener
{
    private int SelectCharcter;
    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<TMP_InputField>(typeof(InputFields));
        Bind<GameObject>(typeof(GameObjects));
        Managers.Event.AddListener(Define.EVENT_TYPE.SelectCharacter, this);

        GetObject((int)GameObjects.MakeCharacter).SetActive(false);

    }
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
        ConfirmButton,
        CancelButton,
    }

    public void ShowMakeCharacter()
    {

    }

    public void OnEvent(Define.EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        if (Event_Type == Define.EVENT_TYPE.SelectCharacter)
        {
            if (Managers.Data.CharacterDataDict[Sender.GetComponent<MyCharacter_SelectScene>().Charcode].isActive)
            {
                SelectCharcter = Sender.GetComponent<MyCharacter_SelectScene>().Charcode;
            }
            else
            {
                GetObject((int)GameObjects.MakeCharacter).SetActive(true);
            }
        }
    }
}
