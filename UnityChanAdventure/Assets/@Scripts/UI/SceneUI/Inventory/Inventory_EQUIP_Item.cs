using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_EQUIP_Item : MonoBehaviour,IListener
{

    [SerializeField]
    private Define.ItemType MyEquipType;
    [SerializeField]
    private Image MyImage;
    [SerializeField]
    private Sprite defaultImage;
    private int mycode;
    private void Start()
    {
        Managers.Event.AddListener(Define.EVENT_TYPE.PlayerEquipChanage, this);
    }
    public void OnEvent(Define.EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
       if(Sender.TryGetComponent(out GameUI gameUi))
        {
            mycode = gameUi.SelectItemcode;
            EquipRefrshUI(Managers.Data.ItemDataDict[mycode].itemType);
        }
    }

    private void EquipRefrshUI(Define.ItemType EventOfEquipType)
    {
        if(MyEquipType != EventOfEquipType)
        {
            return;
        }
        else
        {
            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        if (mycode == 0)
        {
            MyImage.sprite = defaultImage;
        }
        else
        {
            MyImage.sprite = Managers.Resource.Load<Sprite>($"{Managers.Data.ItemDataDict[mycode].iconPath}");

        }
    }

}
