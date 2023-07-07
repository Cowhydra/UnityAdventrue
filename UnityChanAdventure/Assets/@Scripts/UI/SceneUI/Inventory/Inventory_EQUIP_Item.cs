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
      
        SetMyEquipType();
        Managers.Event.AddListener(Define.EVENT_TYPE.PlayerEquipChanageUI, this);
    }


    public void OnEvent(Define.EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
       if(Sender.TryGetComponent(out GameUI gameUi))
        {
            mycode = gameUi.SelectItemcode;
            EquipRefrshUI(Managers.Data.ItemDataDict[mycode].itemType);
        }
    }
    private void SetMyEquipType()
    {
        if (gameObject.name.Contains("Weapon"))
        {
            MyEquipType = Define.ItemType.Weapon;
        }
        else if (gameObject.name.Contains("Boot"))
        {
            MyEquipType = Define.ItemType.Boot;
        }
        else if (gameObject.name.Contains("Cloth"))
        {
            MyEquipType = Define.ItemType.Cloth;
        }
        else if (gameObject.name.Contains("Ring"))
        {
            MyEquipType = Define.ItemType.Ring;
        }
        else if (gameObject.name.Contains("Earring"))
        {
            MyEquipType = Define.ItemType.Earring;
        }
        else if (gameObject.name.Contains("Hat"))
        {
            MyEquipType = Define.ItemType.Hat;
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
    //장비를 장착하게 된다면 UI 갱신
    private void RefreshUI()
    {
        if (mycode == 0||!Managers.EQUIP.EQUIP.ContainsKey(MyEquipType))
        {
            MyImage.sprite = defaultImage;
        }
        else
        {
            MyImage.sprite = Managers.Resource.Load<Sprite>($"{Managers.Data.ItemDataDict[mycode].iconPath}");

        }
    }

}
