using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
    //인벤토리를 현재 매니저에 넣어뒀음..
    //기존 2D 프리코네 프로젝트는 캐릭터 마다 인벤이 필요해서 매니저에 둘 수 없었으나, 이건 가능  혼자 쓸 것이기에
    //매니저를 쓰지 않는다면 Inven Object를 하나 생성해서 다녀야 할듯
    public Dictionary<int, Item> Items { get; } = new Dictionary<int, Item>();

    public void init()
    {
        //데이터를 불러와 개수가 1개 이상인 아이템들만 인벤에 넣어줍니다.
        foreach (Data.ItemData itemdata in Managers.Data.ItemDataDict.Values)
        {
            if (itemdata.count >= 1)
            {
                Items.Add(itemdata.itemcode, Item.MakeItem(itemdata));
            }
        }
    }
    public bool FindItem(int itemcode)
    {
        if (Items.ContainsKey(itemcode))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //아이템 추가 제거 를 진행 - > UI및 다른 아이템 획득 기능을 통해 여기서 전부 진행 -> DB 업데이트 편하게 하기 위함
    public bool Sub(int itemcode,int count = 1)
    {
        if (!Items.ContainsKey(itemcode))
        {
            Debug.Log("없는데 어떻게 뻅니까");
            return false;
        }
        else
        {
            Items[itemcode].Count--;
            if (Items[itemcode].Count == 0)
            {
                Items.Remove(itemcode);
            }
            Managers.Event.RemoveItem?.Invoke(itemcode);
        }
        Debug.Log("인벤에 아이템 제거 + 추후 DB 연동");
        return true;
    }
    public bool Add(int itemcode, int count = 1)
    {
        //아이템을 획득할 경우 Add함수를 이용해 쉽게 아이템을 추가할 수 있습니다.
        if (Items.ContainsKey(itemcode))
        {
            Items[itemcode].Count += count;
        }
        else
        {
            Item item = Item.MakeItem(Managers.Data.ItemDataDict[itemcode]);
            Add(item, count);
        }
        Debug.Log("인벤에 아이템 추가 + 추후 DB 연동");
        Managers.Event.AddItem?.Invoke(itemcode);

        return true;
    }
    public void Add(Item item, int count = 1)
    {
        if (Items.ContainsKey(item.ItemCode))
        {
            Items[item.ItemCode].Count += count;

        }
        else
        {
            item.Count += count;
            Items.Add(item.ItemCode, item);

        }
    }
    public int FindCode(Item item)
    {
        foreach (int i in Items.Keys)
        {
            if (Items[i].Equals(item))
            {
                return i;
            }
        }
        return -1;
    }
    public bool FindItemAndRemove(Item item)
    {
        int code = FindCode(item);
        if (code.Equals(-1)) return false;
        else
        {
            Items[code].Count--;
            if (Items[code].Count.Equals(0))
            {
                Items.Remove(code);
            }

        }
        return true;
    }

    public Item GetItem(int itemId)
    {
        Item item = null;
        Items.TryGetValue(itemId, out item);
        return item;
    }
    public Item Find(Func<Item, bool> condition)
    {
        foreach (Item item in Items.Values)
        {
            if (condition.Invoke(item))
                return item;
        }

        return null;
    }

    public void Clear()
    {
        Items.Clear();
    }

}
