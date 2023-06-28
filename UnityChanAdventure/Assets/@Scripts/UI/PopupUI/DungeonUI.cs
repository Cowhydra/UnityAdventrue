using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DungeonUI : UI_Popup
{

    private Define.Scene ChoiceScene=Define.Scene.None;

    enum Texts 
    {
        InfoPannel_TitleText,
        InfoPannel_ContentsText,

    }
    enum Buttons
    {
        GoDesert_Button,
        GoWater_Button,
        GoLava_Button,
        GoFight_Button,
        StartButton,
        CancelButton,
    }
    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));

         GetButton((int)Buttons.GoDesert_Button).gameObject.BindEvent((PointerEventData data)=> Change_Dungeon(Buttons.GoDesert_Button));
        GetButton((int)Buttons.GoWater_Button ).gameObject.BindEvent((PointerEventData data)=>Change_Dungeon(Buttons.GoWater_Button));
         GetButton((int)Buttons.GoLava_Button  ).gameObject.BindEvent((PointerEventData data)=>Change_Dungeon(Buttons.GoLava_Button));
         GetButton((int)Buttons.GoFight_Button ).gameObject.BindEvent((PointerEventData data)=>Change_Dungeon(Buttons.GoFight_Button));
         GetButton((int)Buttons.StartButton    ).gameObject.BindEvent((PointerEventData data)=> ChangeSecne());
         GetButton((int)Buttons.CancelButton   ).gameObject.BindEvent((PointerEventData data)=>Managers.UI.ClosePopupUI());

    }
    private void ChangeSecne()
    {
        if (ChoiceScene == Define.Scene.None)
        {
            return;
        }
        else
        {
            Managers.Scene.LoadScene(ChoiceScene);
        }
    }
    private void Change_Dungeon(Buttons Button)
    {
        switch (Button)
        {
            case Buttons.GoDesert_Button:
                ChoiceScene = Define.Scene.DesertScene;
                GetText((int)Texts.InfoPannel_TitleText).text = "�縷 ����";
                GetText((int)Texts.InfoPannel_ContentsText).text = "�縷 ������ ������ ��\n ���� ���� Lv.1~10";
                break;
            case Buttons.GoWater_Button:
                ChoiceScene = Define.Scene.WaterScene;
                GetText((int)Texts.InfoPannel_TitleText).text = "�� ����";
                GetText((int)Texts.InfoPannel_ContentsText).text = "�縷 ������ ������ ��\n ���� ���� Lv.21~30";
                break;
            case Buttons.GoLava_Button:
                ChoiceScene = Define.Scene.LavaScene;
                GetText((int)Texts.InfoPannel_TitleText).text = "ȭ�� ����";
                GetText((int)Texts.InfoPannel_ContentsText).text = "�縷 ������ ������ ��\n ���� ���� Lv.11~20";
                break;
            case Buttons.GoFight_Button:
                Debug.Log("���� �̱��� ");
                return;
                ChoiceScene = Define.Scene.FightScene;
                GetText((int)Texts.InfoPannel_TitleText).text = "�볭��";
                GetText((int)Texts.InfoPannel_ContentsText).text = "�پ� ������ ������ ��ܺ�����";
                break;
        }
    }


}
