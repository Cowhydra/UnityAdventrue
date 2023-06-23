using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Shop_Item : UI_Scene
{
    private int _myitemcode;
    private int price;
    
    public int MyItemCode
    {
        get { return _myitemcode; }
        set
        { 
            _myitemcode = value;
            AfterCodeSetInit();
        }
    }
    enum Buttons
    {
        Purchase_Button

    }
    enum Images 
    {
        Shop_Item_Image,

    }
    enum Texts
    {
        ItemName,
        PriceText,

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
        Bind<Image>(typeof(Images));
    


    }
    private void AfterCodeSetInit()
    {
        GetText((int)Texts.ItemName).text = $"{Managers.Data.ItemDataDict[_myitemcode].name}";
        GetButton((int)Buttons.Purchase_Button).gameObject
            .BindEvent((PointerEventData data) => PurchaseItem());
        price = Managers.Data.ItemDataDict[_myitemcode].price;
        GetText((int)Texts.PriceText).text = $"{price}";
    }
    private void PurchaseItem()
    {
        if (Managers.Game.Gold < price)
        {
            Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("금액이 부족합니다.",Color.red);
        }
        else
        {
            Managers.Game.GoldChange(-price);
            Managers.Inven.Add(_myitemcode);
        }
        
    }
}
