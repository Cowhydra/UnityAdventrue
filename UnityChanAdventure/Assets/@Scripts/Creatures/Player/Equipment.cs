﻿using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Equipment
{

    public Dictionary<ItemType, Item> EQUIP { get; set; } = new Dictionary<ItemType, Item>();

    public int EQUIP_Attack { get; set; }
    public int EQUIP_MagicAttack { get; set; }
    public int EQUIP_Def { get; set; }
    public int EQUIP_MaxHp { get; set; }
    public int EQUIP_MaxMp { get; set; }
    public int EQUIP_MagicDef { get; set; }


    public bool Equip(Item equipitem)
    {

        //장비 장착
      
        if (EQUIP.ContainsKey(equipitem.ItemType)||(equipitem.ItemType==Define.ItemType.Consume)||(equipitem.ItemType==Define.ItemType.Ingredient))
        {
            return false;
        }
        else
        {
            EQUIP.Add(equipitem.ItemType, equipitem);
            Debug.Log("장비 장착 관련 구조 생각해보자 이벤트가 가장 무난할듯?");
            Debug.Log("장비 장착하면  UI 변경 예정");
            //장비를 장착하고, 아이템 인벤토리에서 해당 아이템을 제거해준 후, Refresh()를 통해 UI를 갱신합니다.
            Refresh();
            Managers.Data.EquipData[equipitem.ItemType] = equipitem.ItemCode;
            //[DB:Update]
            Managers.DB.UpdateEquip(Managers.Game.AccountNumber, Managers.Game.currentCharNumber, equipitem.ItemCode,  (Update_DB_EQUIPType)((int)equipitem.ItemType));
            return true;
        }
    }
    public void UnEquip(ItemType itemtype)
    {
        EQUIP.Remove(itemtype);
        Managers.Data.EquipData[itemtype] = 0;
        //[DB:Update]
        Managers.DB.UpdateEquip(Managers.Game.AccountNumber, Managers.Game.currentCharNumber, 0,  (Update_DB_EQUIPType)((int)itemtype));
    }


    public void SumEquipAblity()
    {
        Reset();
        ///장비 능력치를 계산
        ///각 장비마다 부여해주는 능력치가 고정적 ( 무기는 공격, 공격범위,마법공격 ) , ( 부츠는 방어력, 최대엠피 )
        ///내가 장착하고 있는 모든 장비를 확인하여 장비 내 증가된 능력을 계싼
        ///계산 전에는 중복 계산이 되지 않게 하기 위해, 기존 정보를 리셋
        foreach (var s in EQUIP.Keys)
        {
            switch (s)
            {
                case ItemType.Weapon:
                    Item.Weapon weapon = EQUIP[s] as Item.Weapon;
                    EQUIP_Attack += weapon.Attack;
                    EQUIP_MagicAttack += weapon.MagicAttack;
                    break;
                case ItemType.Boot:
                    Item.Boot boot = EQUIP[s] as Item.Boot;
                    EQUIP_Def += boot.Def;
                    EQUIP_MaxHp += boot.MaxHp;
                    break;
                case ItemType.Hat:
                    Item.Hat hat = EQUIP[s] as Item.Hat;
                    EQUIP_MaxMp += hat.MaxMp;
                    EQUIP_Def += hat.Def;
                    break;
                case ItemType.Cloth:
                    Item.Cloth cloth = EQUIP[s] as Item.Cloth;
                    EQUIP_MaxHp += cloth.MaxHp;
                    EQUIP_Def += cloth.Magicdef;
                    break;
                case ItemType.Earring:
                    Item.Earring earring = EQUIP[s] as Item.Earring;
                    EQUIP_Attack += earring.Attack;
                    EQUIP_MagicAttack += earring.MagicAttack;
                    break;
                case ItemType.Ring:
                    Item.Ring ring = EQUIP[s] as Item.Ring;
                    EQUIP_MaxHp += ring.MaxHp;
                    EQUIP_MaxMp += ring.MaxMp;
                    break;
            }

        }
    }

    public void Refresh()
    {
        SumEquipAblity();
    }


    public void Reset()
    {
        EQUIP_Attack = 0;
        EQUIP_MagicAttack = 0;
        EQUIP_Def = 0;
        EQUIP_MaxHp = 0;
        EQUIP_MaxMp = 0;
        EQUIP_MagicDef = 0;
    }

    public void Init()
    {
     
        foreach(Define.ItemType itemtype in Managers.Data.EquipData.Keys)
        {
            if (Managers.Data.EquipData[itemtype] != 0)
            {
                if (!EQUIP.ContainsKey(itemtype))
                {
                    EQUIP.Add(itemtype, Item.MakeItem(Managers.Data.ItemDataDict[Managers.Data.EquipData[itemtype]]));
                }
            }
        }
    }
    public void Clear()
    {
        EQUIP.Clear();
    }


}

