using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class Inventory_Item : UI_Scene,IListener
{
    [SerializeField]
    private int _myitemcode;
    private bool isinit;
    public int MyItemCode
    {
        get { return _myitemcode;}
        set
        {
            _myitemcode = value;
            Init();
            RefreshUI();
        }
    }
    [SerializeField]
    Sprite UnActiveImage;
    public bool isActive;
    private Color currentcolor;
    enum Images
    {
        Item_Icon,
        Fill,
    }
    enum Texts
    {
        Item_Count,

    }
    public override void Init()
    {
        if (!isinit)
        {
            base.Init();
            Managers.Event.AddListener(Define.EVENT_TYPE.InventoryItemSelect, this);
            Bind<Image>(typeof(Images));
            Bind<TextMeshProUGUI>(typeof(Texts));
            GetComponent<Canvas>().sortingOrder = (int)Define.SortingOrder.InvenItem;
            gameObject.GetOrAddComponent<GraphicRaycaster>();

            gameObject.BindEvent((PointerEventData data) => Managers.Event.PostNotification(Define.EVENT_TYPE.InventoryItemSelect, this));
            currentcolor = GetComponent<Image>().color;
            isinit = true;

        
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }


    public void RefreshUI()
    {

        if (MyItemCode == 0)
        {
            GetImage((int)Images.Item_Icon).sprite = UnActiveImage;
            GetText((int)Texts.Item_Count).text = "";
            GetImage((int)Images.Fill).fillAmount = 0;
            isActive = false;
            return;
        }
        Debug.Log($"RereshUI :{MyItemCode}");
        if (isActive)
        {
            if (!Managers.Inven.Items.ContainsKey(MyItemCode)||Managers.Inven.Items[MyItemCode].Count == 0)
            {
                isActive = false;
                RefreshUI();
            }
            GetImage((int)Images.Item_Icon).sprite = Managers.Resource.Load<Sprite>($"{Managers.Data.ItemDataDict[MyItemCode].iconPath}");
            GetText((int)Texts.Item_Count).text = $"{Managers.Inven.Items[MyItemCode].Count}";

        }
        else
        {
            GetImage((int)Images.Item_Icon).sprite = UnActiveImage;
            GetText((int)Texts.Item_Count).text = "";
            GetImage((int)Images.Fill).fillAmount = 0;
        }
        
    }

    private void InventoryItemSelect(int senderitemcode)
    {
        if (!isActive||MyItemCode==0) return;
        if (senderitemcode != MyItemCode)
        {
            GetImage((int)Images.Fill).fillAmount = 0;
        }
        else
        {
            GetImage((int)Images.Fill).fillAmount = 1;
        }

    }

    public void OnEvent(Define.EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
       switch(Event_Type)
        {
            case Define.EVENT_TYPE.InventoryItemSelect:
                int sendercode= Sender.GetComponent<Inventory_Item>().MyItemCode;
                InventoryItemSelect(sendercode);
                break;
        }
    }
}
