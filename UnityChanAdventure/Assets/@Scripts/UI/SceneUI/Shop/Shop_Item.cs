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

    //Start ���ٴ� Ȯ���� ���� ���� ��� ���ؼ�, ������Ƽ�� ���� MyItemCode ���� ����Ǹ� �ʱ�ȭ ����
    //�׷��� ���� MyItemCode�� ���� �������� ���ð�� �Ϲ� �ʱ�ȭ ������ ���� Init()�� ��� -> �ߺ� ���������� �Ұ� isinit Ȱ��
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

    //������ ���Ŵ� UI���� ó�� 
    private void PurchaseItem()
    {
        if (Managers.Game.Gold < price)
        {
            Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("�ݾ��� �����մϴ�.",Color.red);
        }
        else
        {
            Managers.Game.GoldChange(-price);
            Managers.Inven.Add(_myitemcode);
            Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("������ ���Ÿ� �����ϼ̽��ϴ�.", Color.green);

        }

    }
}
