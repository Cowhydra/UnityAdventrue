using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameUI : UI_Scene,IListener
{
    public int SelectItemcode = 0;
    private int  Equipitemcode;
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
        Quest_Contents,

    }
    enum Buttons
    {
        Inventory_Button,
      
        EquipButton,
        UseButton,
        QuikEnrollButton,
        DecomposeButton,
        Dungeon_Button,
        Quit_Button,
        SkillBook_Button,

    }

    private void OnDestroy()
    {
        Managers.Event.ActiveQuest -= ShowQusetUI;
    }
    public override void Init()
    {
        base.Init();
        Debug.Log("누가 실행하니?");
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
        InitText();
        Managers.Event.AddListener(Define.EVENT_TYPE.PlayerStatsChange, this);
        Managers.Event.AddListener(Define.EVENT_TYPE.InventoryItemSelect, this);
        Managers.Event.AddListener(Define.EVENT_TYPE.GoodsChange, this);
        Managers.Event.AddListener(Define.EVENT_TYPE.ShopClose, this);
        Managers.Event.AddListener(Define.EVENT_TYPE.ShopOpen, this);
        Managers.Event.ActiveQuest -= ShowQusetUI;
        Managers.Event.ActiveQuest += ShowQusetUI;
       
        InitButtons();
        GetButton((int)Buttons.Quit_Button).gameObject.SetActive(false);

        switch (Managers.Scene.CurrentScene.SceneType)
        {
            case Define.Scene.LavaScene:
                GetButton((int)Buttons.Quit_Button).gameObject.SetActive(true);
                GetButton((int)Buttons.SkillBook_Button).gameObject.SetActive(false);
                break;
            case Define.Scene.DesertScene:
                GetButton((int)Buttons.Quit_Button).gameObject.SetActive(true);
                GetButton((int)Buttons.SkillBook_Button).gameObject.SetActive(false);
                break;
            case Define.Scene.WaterScene:
                GetButton((int)Buttons.Quit_Button).gameObject.SetActive(true);
                GetButton((int)Buttons.SkillBook_Button).gameObject.SetActive(false);
                break;
            case Define.Scene.FightScene:
                GetButton((int)Buttons.Quit_Button).gameObject.SetActive(true);
                GetButton((int)Buttons.SkillBook_Button).gameObject.SetActive(false);
                break;
        }

        InitGameObject();
        TextWithGoods();
        if (Managers.Quest.ActiveQuest.Count > 0)
        {
          foreach(Quest quest in Managers.Quest.ActiveQuest)
            {
                ShowQusetUI(quest);
            }
        }
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
        GetText((int)Texts.AblityHP_Text).text = $"{mychar.Hp}/{mychar.MaxHp}+({Managers.EQUIP.EQUIP_MaxHp})";
        GetText((int)Texts.AblityMP_Text).text = $"{mychar.Mana}/{mychar.MaxMana}+({Managers.EQUIP.EQUIP_MaxMp})";
        GetText((int)Texts.AblityAttack_Text).text = $"{mychar.Attack}+({ Managers.EQUIP.EQUIP_Attack})";
        GetText((int)Texts.AblityMagicAttack_Text).text = $"{mychar.MagicAttack}+({Managers.EQUIP.EQUIP_MagicAttack})";
        GetText((int)Texts.AblityDef_Text).text = $"{mychar.Def}+({Managers.EQUIP.EQUIP_Def})"; 
        GetText((int)Texts.AblityMagicDef_Text).text = $"{mychar.MagicDef}+({Managers.EQUIP.EQUIP_MagicDef})";
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
        GetButton((int)Buttons.SkillBook_Button).gameObject
        .BindEvent((PointerEventData data) =>Managers.UI.ShowSceneUI<SkillBook_UI>());
        GetButton((int)Buttons.EquipButton).gameObject
            .BindEvent((PointerEventData data) => EquipTry());
        GetButton((int)Buttons.UseButton).gameObject
            .BindEvent((PointerEventData data) => ItemUse());
        GetButton((int)Buttons.QuikEnrollButton).gameObject
            .BindEvent((PointerEventData data) => QuikSlotEnroll());
        GetButton((int)Buttons.DecomposeButton).gameObject
            .BindEvent((PointerEventData data) => DeCompose());
        GetButton((int)Buttons.Dungeon_Button).gameObject
            .BindEvent((PointerEventData data) => ShowOnDungeon());
        GetButton((int)Buttons.Quit_Button).gameObject
            .BindEvent((PointerEventData data) => Managers.Scene.LoadScene(Define.Scene.TownScene));
    }
    private void ItemUse()
    {
        if (SelectItemcode == 0) return;
        if (!Managers.Inven.Items.ContainsKey(SelectItemcode)) return;
        if (Managers.Data.ItemDataDict[SelectItemcode].itemType == Define.ItemType.Consume)
        {
            Managers.Inven.Sub(SelectItemcode);
            Item.Consume item = (Item.Consume)Item.MakeItem(Managers.Data.ItemDataDict[SelectItemcode]);
            GetComponent<MyCharacter>().Hp += item.Hp;
            GetComponent<MyCharacter>().Mana += item.Mp;
        }
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
            if (!Managers.EQUIP.EQUIP.ContainsKey(Managers.Data.ItemDataDict[SelectItemcode].itemType)) return;

            Equipitemcode = Managers.EQUIP.EQUIP[Managers.Data.ItemDataDict[SelectItemcode].itemType].ItemCode;

            //장비 장착 해제 과정( 장비 장착 해제 -> 인벤 더하기 -> 
            Managers.EQUIP.UnEquip(Item.MakeItem(Managers.Data.ItemDataDict[Equipitemcode]).ItemType);

            Managers.Inven.Add(Equipitemcode);
            Managers.Event.AddItem?.Invoke(Equipitemcode);

            Managers.Event.PostNotification(Define.EVENT_TYPE.PlayerEquipChanageUI, this);
       

            if (Managers.EQUIP.Equip(Managers.Inven.GetItem(SelectItemcode)))
            {
                Managers.Inven.Sub(SelectItemcode);
                Managers.Event.PostNotification(Define.EVENT_TYPE.PlayerEquipChanageUI, this);
                Managers.Event.RemoveItem?.Invoke(SelectItemcode);
                Debug.Log("DB처리 장비 아이템 갱신  + 인벤 아이템 개수 감소");
              
            }
           
        }
    }

    private void DeCompose()
    {

    }
    private void QuikSlotEnroll()
    {
        if (!Managers.Data.ItemDataDict.ContainsKey(SelectItemcode)||(Managers.Data.ItemDataDict[SelectItemcode].itemType != Define.ItemType.Consume))
        {
            return;
        }

    }
    private void InitGameObject()
    {
        GetObject((int)GameObjects.BackGround)
            .BindEvent((PointerEventData data) => ShutOffInven());
        GetObject((int)GameObjects.Inventory).SetActive(false);

        foreach (Transform transforom in GetObject((int)GameObjects.Quest_Contents).GetComponentInChildren<Transform>())
        {
            Managers.Resource.Destroy(transforom.gameObject);
        }
        GetObject((int)GameObjects.Quest).SetActive(false);

    }

    private void ShowQusetUI(Quest quest)
    {
        GetObject((int)GameObjects.Quest).SetActive(true);
        Quest_Content_Text QuestText = Managers.UI.ShowSceneUI<Quest_Content_Text>();
        QuestText.QuestID = quest.UniqueId;
        QuestText.transform.SetParent(GetObject((int)GameObjects.Quest_Contents).transform);

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
