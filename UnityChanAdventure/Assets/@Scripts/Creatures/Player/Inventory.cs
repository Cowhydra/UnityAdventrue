using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{

    public Dictionary<int, Item> Items { get; } = new Dictionary<int, Item>();

    public void init()
    {
        //�����͸� �ҷ��� ������ 1�� �̻��� �����۵鸸 �κ��� �־��ݴϴ�.
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
        //�������� ȹ���� ��� Add�Լ��� �̿��� ���� �������� �߰��� �� �ֽ��ϴ�.
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