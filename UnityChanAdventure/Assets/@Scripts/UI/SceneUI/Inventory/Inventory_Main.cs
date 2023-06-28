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
        Debug.Log("이벤트 연동");
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
                _item.MyItemCode = 0;
                _item.transform.SetParent(gameObject.transform);
                _item.isActive = false;
                _item.RefreshUI();
                InvenUI.Add(_item);
                //Count가 없으면, myitemcode를 가질 수 없음
            }
            else
            {
                _item.transform.SetParent(gameObject.transform);
                _item.transform.SetSiblingIndex(currentactiveItemCount);
                _item.MyItemCode = myitem.ItemCode;
                _item.isActive = true;
                currentactiveItemCount++;
                _item.RefreshUI();
                InvenUI.Add(_item);
            }

        }
        for(int i= currentactiveItemCount; i < MaxSlot; i++)
        {
            Inventory_Item _item = Managers.UI.ShowSceneUI<Inventory_Item>();
            _item.transform.SetParent(gameObject.transform);
            _item.transform.SetSiblingIndex(MaxSlot-1);
            InvenUI.Add(_item);
        }
 
    }


    private void ItemAddEvent(int itemcode)
    {
        //우선 활성된 아이템인지 체크 
        Inventory_Item checkitem = InvenUI.Find(s => s.MyItemCode == itemcode);
        if (checkitem == null)
        {
            //만약 비활성된 아이템 에서 아이템 코드가 0인 것들이 있는지 확인
            Inventory_Item checkitem2 = InvenUI.FirstOrDefault(s => s.MyItemCode == 0);
            if(checkitem2 == null)
            {//아이템 코드가 0이 아닌 것이 없다면 인벤 꽉참
                Debug.Log("인벤토리창이 꽉 찼습니다.");
            }
            else
            {
                checkitem2.isActive = true;
                checkitem2.transform.SetSiblingIndex(currentactiveItemCount);
                currentactiveItemCount++;
                checkitem2.MyItemCode = itemcode;
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
            if (!Managers.Inven.Items.ContainsKey(itemcode)||Managers.Inven.Items[itemcode].Count == 0)
            {
                checkitem.transform.SetSiblingIndex(MaxSlot - 1);
                checkitem.MyItemCode = 0;
            }
            else
            {
                checkitem.RefreshUI();
            }
        }
    }

}
