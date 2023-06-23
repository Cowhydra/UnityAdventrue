using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopUI : UI_Scene,IListener
{
    enum GameObjects
    {
        ShopView,

        Weapon_Shop,
        Boot_Shop,
        Cloth_Shop,
        Earring_Shop,
        Hat_Shop,
        Ring_Shop,
        Blokcer,
    }
    enum Buttons
    {
        Weapon_Shop,
        Boot_Shop,
        Cloth_Shop,
        Earring_Shop,
        Hat_Shop,
        Ring_Shop,

    }
    enum ShowShopTpye
    {
        Weapon,
        Boot,
        Cloth,
        Earring,
        Hat,
        Ring,

    }
    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        GetComponent<Canvas>().sortingOrder = (int)Define.SortingOrder.ShopUI;
        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));
        InitButton();

        ChanageShopPannel(ShowShopTpye.Weapon);
        GetObject((int)GameObjects.Blokcer).BindEvent((PointerEventData data) => gameObject.SetActive(false));
        Managers.Event.AddListener(Define.EVENT_TYPE.ShopOpen, this);
        gameObject.SetActive(false);


    }
    private void InitButton()
    {
        GetButton((int)Buttons.Boot_Shop).gameObject
            .BindEvent((PointerEventData data)=>ChanageShopPannel(ShowShopTpye.Weapon));
        GetButton((int)Buttons.Weapon_Shop).gameObject
           .BindEvent((PointerEventData data) => ChanageShopPannel(ShowShopTpye.Boot));
        GetButton((int)Buttons.Cloth_Shop).gameObject
           .BindEvent((PointerEventData data) => ChanageShopPannel(ShowShopTpye.Cloth));
        GetButton((int)Buttons.Earring_Shop).gameObject
           .BindEvent((PointerEventData data) => ChanageShopPannel(ShowShopTpye.Earring));
        GetButton((int)Buttons.Hat_Shop).gameObject
           .BindEvent((PointerEventData data) => ChanageShopPannel(ShowShopTpye.Hat));
        GetButton((int)Buttons.Ring_Shop).gameObject
           .BindEvent((PointerEventData data) => ChanageShopPannel(ShowShopTpye.Ring));
    }
    private void ChanageShopPannel(ShowShopTpye myshoptype)
    {

        GetObject((int)GameObjects.Boot_Shop).SetActive(false);
        GetObject((int)GameObjects.Weapon_Shop).SetActive(false);
        GetObject((int)GameObjects.Cloth_Shop).SetActive(false);
        GetObject((int)GameObjects.Earring_Shop).SetActive(false);
        GetObject((int)GameObjects.Hat_Shop).SetActive(false);
        GetObject((int)GameObjects.Ring_Shop).SetActive(false);
        GetButton((int)Buttons.Boot_Shop).GetComponent<Image>().color = Color.white;
        GetButton((int)Buttons.Weapon_Shop).GetComponent<Image>().color = Color.white;
        GetButton((int)Buttons.Cloth_Shop).GetComponent<Image>().color = Color.white;
        GetButton((int)Buttons.Earring_Shop).GetComponent<Image>().color = Color.white;
        GetButton((int)Buttons.Hat_Shop).GetComponent<Image>().color = Color.white;
        GetButton((int)Buttons.Boot_Shop).GetComponent<Image>().color = Color.white;
        switch (myshoptype)
        {
            case ShowShopTpye.Weapon:
                GetObject((int)GameObjects.Weapon_Shop).SetActive(true);
                GetObject((int)GameObjects.ShopView).GetComponent<ScrollRect>().content
                    = GetObject((int)GameObjects.Weapon_Shop).GetComponent<RectTransform>();
                GetButton((int)Buttons.Weapon_Shop).GetComponent<Image>().color = Color.red;

                break;
            case ShowShopTpye.Boot:
                GetObject((int)GameObjects.Boot_Shop).SetActive(true);
                GetObject((int)GameObjects.ShopView).GetComponent<ScrollRect>().content
                   = GetObject((int)GameObjects.Boot_Shop).GetComponent<RectTransform>();
                GetButton((int)Buttons.Boot_Shop).GetComponent<Image>().color = Color.red;
                break;
            case ShowShopTpye.Cloth:
                GetObject((int)GameObjects.Cloth_Shop).SetActive(true);
                GetObject((int)GameObjects.ShopView).GetComponent<ScrollRect>().content
                   = GetObject((int)GameObjects.Cloth_Shop).GetComponent<RectTransform>();
                GetButton((int)Buttons.Cloth_Shop).GetComponent<Image>().color = Color.red;
                break;
            case ShowShopTpye.Earring:
                GetObject((int)GameObjects.Earring_Shop).SetActive(true);
                GetObject((int)GameObjects.ShopView).GetComponent<ScrollRect>().content
                   = GetObject((int)GameObjects.Earring_Shop).GetComponent<RectTransform>();
                GetButton((int)Buttons.Earring_Shop).GetComponent<Image>().color = Color.red;
                break;
            case ShowShopTpye.Hat:
                GetObject((int)GameObjects.Hat_Shop).SetActive(true);
                GetObject((int)GameObjects.ShopView).GetComponent<ScrollRect>().content
                   = GetObject((int)GameObjects.Hat_Shop).GetComponent<RectTransform>();
                GetButton((int)Buttons.Hat_Shop).GetComponent<Image>().color = Color.red;
                break;
            case ShowShopTpye.Ring:
                GetObject((int)GameObjects.Ring_Shop).SetActive(true);
                GetObject((int)GameObjects.ShopView).GetComponent<ScrollRect>().content
                   = GetObject((int)GameObjects.Ring_Shop).GetComponent<RectTransform>();
                GetButton((int)Buttons.Ring_Shop).GetComponent<Image>().color = Color.red;
                break;
        }
    }

    public void OnEvent(Define.EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case Define.EVENT_TYPE.ShopOpen:
                gameObject.SetActive(true);
                break;
        }
    }
}
