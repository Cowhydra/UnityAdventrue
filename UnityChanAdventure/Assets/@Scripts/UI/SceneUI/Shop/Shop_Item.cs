using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Shop_Item : UI_Scene
{
    [SerializeField]
    private int _myitemcode;
    private int price;
    private bool isinit;

    //Start 보다는 확실한 실행 순서 제어를 위해서, 프로퍼티를 통해 MyItemCode 값이 변경되면 초기화 진행
    //그래도 만약 MyItemCode가 없는 아이템이 나올경우 일반 초기화 진행을 위해 Init()도 사용 -> 중복 방지용으로 불값 isinit 활용
    public int MyItemCode
    {
        get { return _myitemcode; }
        set
        { 
            _myitemcode = value;
            AfterCodeSetInit();
        }
    }
    #region BindUI
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
    #endregion
    private void Start()
    {
        Init();
        gameObject.GetOrAddComponent<GraphicRaycaster>();
    }
    public override void Init()
    {
        if (!isinit)
        {
            base.Init();
            Bind<Button>(typeof(Buttons));
            Bind<TextMeshProUGUI>(typeof(Texts));
            Bind<Image>(typeof(Images));
            isinit = true;
        }
    }
    private void AfterCodeSetInit()
    {
        if (!isinit)
        {
            Init();
        }
        GetText((int)Texts.ItemName).text = $"{Managers.Data.ItemDataDict[_myitemcode].name}";
        GetButton((int)Buttons.Purchase_Button).gameObject
            .BindEvent((PointerEventData data) => PurchaseItem());
        price = Managers.Data.ItemDataDict[_myitemcode].price;
        GetText((int)Texts.PriceText).text = $"{price}";
        GetImage((int)Images.Shop_Item_Image).sprite = Managers.Resource.Load<Sprite>($"{Managers.Data.ItemDataDict[_myitemcode].iconPath}");
    }

    //아이템 구매는 UI에서 처리 
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
            Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("아이템 구매를 성공하셨습니다.", Color.green);

        }

    }
}
