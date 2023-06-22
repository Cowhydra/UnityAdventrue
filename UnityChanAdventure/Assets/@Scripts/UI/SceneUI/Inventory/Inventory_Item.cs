using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class Inventory_Item : UI_Scene
{
    public int MyItemCode;
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

    // Start is called before the first frame update
    void Start()
    {
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        RefreshUI();
        gameObject.BindEvent((PointerEventData data) => InventoryItemSelect());
        currentcolor = GetComponent<Image>().color;
    }


    public void RefreshUI()
    {
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
