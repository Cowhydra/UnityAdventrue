using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class Inventory_Main :MonoBehaviour
{

    public static int MaxSlot = 30;
    public static int currentactiveItemCount = 0;
    private List<Inventory_Item> InvenUI = new List<Inventory_Item>();
    private void Start()
    {
        foreach (Transform transforom in gameObject.GetComponentInChildren<Transform>())
        {
            Managers.Resource.Destroy(transforom.gameObject);
        }
        Managers.Event.AddItem -= ItemAddEvent;
        Managers.Event.AddItem += ItemAddEvent;
        Managers.Event.RemoveItem -= ItemRemoveEvent;
        Managers.Event.RemoveItem += ItemRemoveEvent;
        SortedByCount();
    }
    private void SortedByCount()
    {
        List<Item> SortedList = Managers.Inven.Items.Values.ToList();

        foreach (Item myitem in SortedList.OrderByDescending(s => s.Count))
        {
            Inventory_Item _item = Managers.UI.ShowSceneUI<Inventory_Item>();
            if (myitem.Count == 0)
            {
                _item.transform.SetParent(gameObject.transform);
                _item.isActive = false;
                //Count가 없으면, myitemcode를 가질 수 없음
            }
            else
            {
                _item.transform.SetParent(gameObject.transform);
                _item.transform.SetSiblingIndex(currentactiveItemCount);
                _item.MyItemCode = myitem.ItemCode;
                _item.isActive = true;
                currentactiveItemCount++;
            }

        }
        for(int i= currentactiveItemCount; i < MaxSlot; i++)
        {
            Inventory_Item _item = Managers.UI.ShowSceneUI<Inventory_Item>();
            _item.transform.SetParent(gameObject.transform);
            _item.transform.SetSiblingIndex(MaxSlot-1);
        }
    }


    private void ItemAddEvent(int itemcode)
    {
        Inventory_Item checkitem = InvenUI.Find(s => s.MyItemCode == itemcode);
        if (checkitem == null)
        {
            Inventory_Item checkitem2 = InvenUI.First(s => s.MyItemCode == itemcode);
            if(checkitem2 == null)
            {
                Debug.Log("인벤토리창이 꽉 찼습니다.");
            }
            else
            {
                checkitem2.MyItemCode = itemcode;
                checkitem2.isActive = true;
                checkitem2.transform.SetSiblingIndex(currentactiveItemCount);
                currentactiveItemCount++;
                checkitem2.RefreshUI();
            }
        }
        else
        {
            checkitem.RefreshUI();
        }
    }

    private void ItemRemoveEvent(int itemcode)
    {
        Inventory_Item checkitem = InvenUI.Find(s => s.MyItemCode == itemcode);
        if (checkitem == null)
        {
            Debug.Log("없는 아이템을 왜 삭제합니까");
        }
        else
        {
            if (Managers.Data.ItemDataDict[itemcode].count == 0)
            {
                checkitem.transform.SetSiblingIndex(MaxSlot - 1);
                checkitem.MyItemCode = 0;
            }
            checkitem.RefreshUI();
        }
    }

}
