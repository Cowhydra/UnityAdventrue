using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class Inventory_Item : UI_Scene
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
        }
    }
    [SerializeField]
    Sprite UnActiveImage;
    public bool isActive;
    private Color currentcolor;
    enum Images
    {
        Item_Icon,

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
            Bind<Image>(typeof(Images));
            Bind<TextMeshProUGUI>(typeof(Texts));

            gameObject.BindEvent((PointerEventData data) => InventoryItemSelect());
            currentcolor = GetComponent<Image>().color;
            isinit = true;

            RefreshUI();
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }


    public void RefreshUI()
    {
        Debug.Log($"RereshUI :{MyItemCode}");
        if (MyItemCode == 0) return;

        if (isActive)
        {
            GetImage((int)Images.Item_Icon).sprite = Managers.Resource.Load<Sprite>($"{Managers.Data.ItemDataDict[MyItemCode].iconPath}");
            GetText((int)Texts.Item_Count).text = $"{Managers.Data.ItemDataDict[MyItemCode].count}";
            if (Managers.Data.ItemDataDict[MyItemCode].count == 0)
            {
                isActive = false;
                RefreshUI();
            }
        }
        else
        {
            GetImage((int)Images.Item_Icon).sprite = UnActiveImage;
            GetText((int)Texts.Item_Count).text = "";
        }
        
    }

    private void InventoryItemSelect()
    {
        if (!isActive||MyItemCode==0) return;
        Managers.Event.PostNotification(Define.EVENT_TYPE.InventoryItemSelect, this);
        if (GetComponent<Image>().color == currentcolor)
        {
            GetComponent<Image>().color = Color.red;
        }
        else
        {
            GetComponent<Image>().color = currentcolor;
        }
    }
}
