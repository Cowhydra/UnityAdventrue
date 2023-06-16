using System;
using System.Collections.Generic;


public class ItemInventoryManager
{
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

    public Item Get(int itemId)
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

