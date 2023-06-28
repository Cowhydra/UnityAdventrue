using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameUI : UI_Scene,IListener
{
    public int SelectItemcode = 0;
    enum Texts
    {
        BlueDiamond_Text,
        RedDiamond_Text,
        Gold_Text,

        AblityHP_Text,
        AblityMP_Text,
        AblityAttack_Text,
        AblityMagicAttack_Text,
        AblityDef_Text,
        AblityMagicDef_Text,

    }
    enum GameObjects 
    {
        BackGround,//클릭시 인벤창 꺼지도록 설정 예정
        Inventory,
        Quest,
    }
    enum Buttons
    {
        Inventory_Button,
        Quest_Button,
        
        EquipButton,
        UnEquipButton,
        QuikEnrollButton,
        DecomposeButton,
        Dungeon_Button,

    }


    public override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
        InitText();
        Managers.Event.AddListener(Define.EVENT_TYPE.PlayerStatsChange, this);
        Managers.Event.AddListener(Define.EVENT_TYPE.InventoryItemSelect, this);
        Managers.Event.AddListener(Define.EVENT_TYPE.GoodsChange, this);
        Managers.Event.AddListener(Define.EVENT_TYPE.ShopClose, this);
        Managers.Event.AddListener(Define.EVENT_TYPE.ShopOpen, this);
        InitButtons();



        InitGameObject();
    }
    private void Start()
    {
        Init();
    }

    private void InitText()
    {
        GetText((int)Texts.BlueDiamond_Text).text = "";
        GetText((int)Texts.RedDiamond_Text).text = "";
        GetText((int)Texts.Gold_Text).text = "";
        GetText((int)Texts.AblityHP_Text).text = "";
        GetText((int)Texts.AblityMP_Text).text = "";
        GetText((int)Texts.AblityAttack_Text).text = "";
        GetText((int)Texts.AblityMagicAttack_Text).text = "";
        GetText((int)Texts.AblityDef_Text).text = "";
        GetText((int)Texts.AblityMagicDef_Text).text = "";
    }
    private void TextWithCharacter(MyCharacter mychar)
    {
        GetText((int)Texts.AblityHP_Text).text = $"{mychar.Hp}/{mychar.MaxHp}";
        GetText((int)Texts.AblityMP_Text).text = $"{mychar.Mana}/{mychar.MaxMana}";
        GetText((int)Texts.AblityAttack_Text).text = $"{mychar.Attack}";
        GetText((int)Texts.AblityMagicAttack_Text).text = $"{mychar.MagicAttack}";
        GetText((int)Texts.AblityDef_Text).text = $"{mychar.Def}";
        GetText((int)Texts.AblityMagicDef_Text).text = $"{mychar.MagicDef}";
    }
    private void TextWithGoods()
    {
        GetText((int)Texts.BlueDiamond_Text).text = $"{Managers.Game.BlueDiamond}";
        GetText((int)Texts.RedDiamond_Text).text = $"{Managers.Game.RedDiamond}";
        GetText((int)Texts.Gold_Text).text = $"{Managers.Game.Gold}";
    }
    private void InitButtons()
    {
        GetButton((int)Buttons.Inventory_Button).gameObject
            .BindEvent((PointerEventData data) => ShowOnInven());
        GetButton((int)Buttons.EquipButton).gameObject
            .BindEvent((PointerEventData data) => EquipTry());
        GetButton((int)Buttons.UnEquipButton).gameObject
            .BindEvent((PointerEventData data) => UnEquipTry());
        GetButton((int)Buttons.QuikEnrollButton).gameObject
            .BindEvent((PointerEventData data) => QuikSlotEnroll());
        GetButton((int)Buttons.DecomposeButton).gameObject
            .BindEvent((PointerEventData data) => DeCompose());
        GetButton((int)Buttons.Dungeon_Button).gameObject
            .BindEvent((PointerEventData data) => ShowOnDungeon());

    }
    private void EquipTry()
    {
        if (SelectItemcode == 0) return;
        if (Managers.EQUIP.Equip(Managers.Inven.GetItem(SelectItemcode)))
        {
            Managers.Inven.Sub(SelectItemcode);
            Managers.Event.PostNotification(Define.EVENT_TYPE.PlayerEquipChanageUI, this);
            Managers.Event.RemoveItem?.Invoke(SelectItemcode);
            Debug.Log("DB처리 장비 아이템 갱신  + 인벤 아이템 개수 감소");
        }
        else
        {
            Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("이미 장착한 장비가 있습니다.", Color.red);
        
        }
    }
    private void UnEquipTry()
    {
        if (SelectItemcode == 0) return;
        if (Managers.EQUIP.EQUIP.ContainsKey(Managers.Inven.GetItem(SelectItemcode).ItemType))
        {
            if (Managers.EQUIP.EQUIP[Managers.Inven.GetItem(SelectItemcode).ItemType].ItemCode == SelectItemcode)
            {
                Managers.EQUIP.UnEquip(Managers.Inven.GetItem(SelectItemcode).ItemType);
                Managers.Inven.Add(SelectItemcode);
                Managers.Event.PostNotification(Define.EVENT_TYPE.PlayerEquipChanageUI, this);
                Managers.Event.AddItem?.Invoke(SelectItemcode);
            }
            else
            {
                Debug.Log("이상한 버그 발생");
            }
        }
        else
        {
            Debug.Log("없는 아이템 장착헤제 시도");
            return;
        }
     
    }
    private void DeCompose()
    {

    }
    private void QuikSlotEnroll()
    {

    }
    private void InitGameObject()
    {
        GetObject((int)GameObjects.BackGround)
            .BindEvent((PointerEventData data) => ShutOffInven());
        GetObject((int)GameObjects.Inventory).SetActive(false);
    }
    private void ShutOffInven()
    {
        GetObject((int)GameObjects.Inventory).SetActive(false);
        GetButton((int)Buttons.Dungeon_Button).gameObject.SetActive(true);
        GetButton((int)Buttons.Inventory_Button).gameObject.SetActive(true);
        Managers.Event.PostNotification(Define.EVENT_TYPE.InventoryClose, this);
    }
    private void ShowOnInven()
    {
        GetObject((int)GameObjects.Inventory).SetActive(true);
        GetButton((int)Buttons.Dungeon_Button).gameObject.SetActive(false);
        GetButton((int)Buttons.Inventory_Button).gameObject.SetActive(false);
 
        Managers.Event.PostNotification(Define.EVENT_TYPE.InventoryOpen, this);
    }

    private void ShowOnDungeon()
    {
        Managers.UI.ShowPopupUI<DungeonUI>();
        Debug.Log("던전 UI 생성");
    }

    public void OnEvent(Define.EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case Define.EVENT_TYPE.PlayerStatsChange:
               if(  Sender.TryGetComponent(out MyCharacter character))
                {
                    TextWithCharacter(character);
                }
                else
                {
                    Debug.Log("버그");
                }
                break;
            case Define.EVENT_TYPE.InventoryItemSelect:
                if(Sender.TryGetComponent(out Inventory_Item Inven_item))
                {
                    SelectItemcode = Inven_item.MyItemCode;
                }
                break;
            case Define.EVENT_TYPE.GoodsChange:
                TextWithGoods();
                break;
            case Define.EVENT_TYPE.ShopOpen:
                GetObject((int)GameObjects.Quest).SetActive(false);
                GetButton((int)Buttons.Dungeon_Button).gameObject.SetActive(false);
                GetButton((int)Buttons.Inventory_Button).gameObject.SetActive(false);
                break;
            case Define.EVENT_TYPE.ShopClose:
                GetObject((int)GameObjects.Quest).SetActive(true);
                GetButton((int)Buttons.Dungeon_Button).gameObject.SetActive(true);
                GetButton((int)Buttons.Inventory_Button).gameObject.SetActive(true);
                break;
        }
    }
}
