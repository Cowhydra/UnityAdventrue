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
            GetComponent<Canvas>().overrideSorting = false;
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

        //빈 아이템일 경우  각종 요소들 초기화 -> 아이템 이미지 변경 등 
        if (MyItemCode == 0)
        {
            GetImage((int)Images.Item_Icon).sprite = UnActiveImage;
            GetText((int)Texts.Item_Count).text = "";
            GetImage((int)Images.Fill).fillAmount = 0;
            isActive = false;
            return;
        }
        Debug.Log($"RereshUI :{MyItemCode}");
        //아 아이템이 활성 된 상태라면 
        if (isActive)
        {
            //만약  활성된 상태인데 -> 인벤에 없다 -> 다시 Refresh
            if (!Managers.Inven.Items.ContainsKey(MyItemCode)||Managers.Inven.Items[MyItemCode].Count == 0)
            {
                isActive = false;
                RefreshUI();
            }
            //만약 활성된 상태 + 인벤에 있으면 Sprite 및 count 갱신
            GetImage((int)Images.Item_Icon).sprite = Managers.Resource.Load<Sprite>($"{Managers.Data.ItemDataDict[MyItemCode].iconPath}");
            GetText((int)Texts.Item_Count).text = $"{Managers.Inven.Items[MyItemCode].Count}";

        }
        //갱신 하는데 비활성 상태이면 초기화
        else
        {
            GetImage((int)Images.Item_Icon).sprite = UnActiveImage;
            GetText((int)Texts.Item_Count).text = "";
            GetImage((int)Images.Fill).fillAmount = 0;
        }
        
    }


    //아이템을 선택하면 선택한 아이템에 색깔을 입혀서 차별화
    //모든 Inventroy_item이 같이 쓰니까, 이벤트가 발생한 코드와 내 코드가 다르면 색을 없게 변경, 같으면 나 자신을 색이 있게 변경
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
