using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerStatus_Canvas : UI_Scene,IListener
{
    enum Images
    {
        PlayerHp_Fill,
        PlayerMp_Fill,
        PlayerExp_Fill,

    }
    enum Texts
    {
        PlayerHp_Text,
        PlayerMp_Text,
        PlayerInfo_Text,
    }
    enum GameObjects
    {
        Buffs,
    }
    public override void Init()
    {
        base.Init();
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        GetImage((int)Images.PlayerHp_Fill).fillAmount = 1;
        GetImage((int)Images.PlayerMp_Fill).fillAmount = 1;
        GetImage((int)Images.PlayerExp_Fill).fillAmount = 0;
        GetText((int)Texts.PlayerHp_Text).text = "";
        GetText((int)Texts.PlayerMp_Text).text = "";
        GetText((int)Texts.PlayerInfo_Text).text = "";
        GetObject((int)GameObjects.Buffs).SetActive(false);
        Managers.Event.AddListener(Define.EVENT_TYPE.PlayerStatsChange, this);
    }
    private void RefreshUI(MyCharacter character)
    {
        GetImage((int)Images.PlayerHp_Fill).fillAmount = character.Hp / character.MaxHp;
        GetImage((int)Images.PlayerMp_Fill).fillAmount = character.Mana / character.MaxMana;
        GetText((int)Texts.PlayerHp_Text).text = $"{character.Hp}/{character.MaxHp}";
        GetText((int)Texts.PlayerMp_Text).text = $"{character.Mana}/{character.MaxMana}";
        GetImage((int)Images.PlayerExp_Fill).fillAmount = character.Exp / character.RequireExp;
        GetText((int)Texts.PlayerInfo_Text).text = $"Lv.{character.Level}";
    }
    private void Start()
    {
        Init();
    }

    public void OnEvent(Define.EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        if(Sender.TryGetComponent(out MyCharacter character))
        {
            RefreshUI(character);
        }
        
    }
}
