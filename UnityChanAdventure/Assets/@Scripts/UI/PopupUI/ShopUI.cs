using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopUI : UI_Scene,IListener
{
    #region Object BInd
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
        Weapon,
        Boot,
        Cloth,
        Earring,
        Hat,
        Ring,
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
    #endregion

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
        Debug.Log("디버그 숨겨놓기~");
        ChanageShopPannel(ShowShopTpye.Weapon);
        GetObject((int)GameObjects.Blokcer).BindEvent((PointerEventData data) => ShutoffShop());
        Managers.Event.AddListener(Define.EVENT_TYPE.ShopOpen, this);
        gameObject.SetActive(false);


    }
    private void ShutoffShop()
    {
        Managers.Event.PostNotification(Define.EVENT_TYPE.ShopClose, this);
        gameObject.SetActive(false);

    }
    private void InitButton()
    {
        GetButton((int)Buttons.Boot_Shop).gameObject
            .BindEvent((PointerEventData data)=>ChanageShopPannel(ShowShopTpye.Boot));
        GetButton((int)Buttons.Weapon_Shop).gameObject
           .BindEvent((PointerEventData data) => ChanageShopPannel(ShowShopTpye.Weapon));
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
        GetObject((int)GameObjects.Weapon).SetActive(false);
        GetObject((int)GameObjects.Boot).SetActive(false);
        GetObject((int)GameObjects.Cloth).SetActive(false);
        GetObject((int)GameObjects.Earring).SetActive(false);
        GetObject((int)GameObjects.Hat).SetActive(false);
        GetObject((int)GameObjects.Ring).SetActive(false);

        GetButton((int)Buttons.Boot_Shop).GetComponent<Image>().color = Color.white;
        GetButton((int)Buttons.Weapon_Shop).GetComponent<Image>().color = Color.white;
        GetButton((int)Buttons.Cloth_Shop).GetComponent<Image>().color = Color.white;
        GetButton((int)Buttons.Earring_Shop).GetComponent<Image>().color = Color.white;
        GetButton((int)Buttons.Hat_Shop).GetComponent<Image>().color = Color.white;
        GetButton((int)Buttons.Ring_Shop).GetComponent<Image>().color = Color.white;
        switch (myshoptype)
        {
            case ShowShopTpye.Weapon:
                GetObject((int)GameObjects.Weapon).SetActive(true);
                GetObject((int)GameObjects.ShopView).GetComponent<ScrollRect>().content
                    = GetObject((int)GameObjects.Weapon).GetComponent<RectTransform>();
                GetButton((int)Buttons.Weapon_Shop).GetComponent<Image>().color = Color.red;

                break;
            case ShowShopTpye.Boot:
                GetObject((int)GameObjects.Boot).SetActive(true);
                GetObject((int)GameObjects.ShopView).GetComponent<ScrollRect>().content
                   = GetObject((int)GameObjects.Boot).GetComponent<RectTransform>();
                GetButton((int)Buttons.Boot_Shop).GetComponent<Image>().color = Color.red;
                break;
            case ShowShopTpye.Cloth:
                GetObject((int)GameObjects.Cloth).SetActive(true);
                GetObject((int)GameObjects.ShopView).GetComponent<ScrollRect>().content
                   = GetObject((int)GameObjects.Cloth).GetComponent<RectTransform>();
                GetButton((int)Buttons.Cloth_Shop).GetComponent<Image>().color = Color.red;
                break;
            case ShowShopTpye.Earring:
                GetObject((int)GameObjects.Earring).SetActive(true);
                GetObject((int)GameObjects.ShopView).GetComponent<ScrollRect>().content
                   = GetObject((int)GameObjects.Earring).GetComponent<RectTransform>();
                GetButton((int)Buttons.Earring_Shop).GetComponent<Image>().color = Color.red;
                break;
            case ShowShopTpye.Hat:
                GetObject((int)GameObjects.Hat).SetActive(true);
                GetObject((int)GameObjects.ShopView).GetComponent<ScrollRect>().content
                   = GetObject((int)GameObjects.Hat).GetComponent<RectTransform>();
                GetButton((int)Buttons.Hat_Shop).GetComponent<Image>().color = Color.red;
                break;
            case ShowShopTpye.Ring:
                GetObject((int)GameObjects.Ring).SetActive(true);
                GetObject((int)GameObjects.ShopView).GetComponent<ScrollRect>().content
                   = GetObject((int)GameObjects.Ring).GetComponent<RectTransform>();
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
