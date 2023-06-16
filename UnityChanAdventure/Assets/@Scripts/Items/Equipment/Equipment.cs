using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Equipment
{

    public Dictionary<ItemType, Item> EQUIP { get; set; } = new Dictionary<ItemType, Item>();
    // Dictionary<int, Dictionary<ItemType, Item>> EQUIPS { get; set; } = new Dictionary<int, Dictionary<ItemType, Item>>();
    
    //장비창은 캐릭터 마다 고유로 가지고 있는 값으로,
    //싱글톤으로 구현 X -> 캐릭터 싱글톤 마다 고유의 EQUIP을 구현해주고,
    //EQUIP은 CharcterCode로 구분합니다.
    //싱글톤은 아니지만  싱글톤 멤버로 들어가기에 EQUIP멤버에 접근하려면
    //Managers.MyCHaracters....>~~~[ CharCOde].EQUIP <<< 로 접근이 가능
    public Equipment(int characterCode)
    {
        Character_Code = characterCode;

    }

    public int Character_Code { get; }
    public string JoBType { get { return Managers.Data.CharacterDataDict[Character_Code].jobType.ToString(); } }

    public int EQUIP_Attack { get; set; }
    public int EQUIP_MagicAttack { get; set; }
    public int EQUIP_Available_code { get; set; }
    public int EQUIP_Def { get; set; }
    public int EQUIP_MaxHp { get; set; }
    public int EQUIP_MaxMp { get; set; }
    public int EQUIP_MagicDef { get; set; }
    public int EQUIP_HpRecovery { get; set; }
    public int EQUIP_MpRecovery { get; set; }
    public int EQUIP_AttackRange { get; set; }





    public bool Equip(Item equipitem)
    {

        //장비 장착
      
        if (EQUIP.ContainsKey(equipitem.ItemType))
        {
            //경고 UI
            Debug.Log("이미 해당 부위에 장착한 장비가 존재");
            Debug.Log("경고 UI!");
            return false;
        }
        else if (!AvailableEquip(equipitem))
        {
            //경고UI
            Debug.Log("맞지 않는 타입임 본인 장비 인지 확인 바람");
            Debug.Log("경고 UI!");
            return false;
        }
        else
        {
            EQUIP.Add(equipitem.ItemType, equipitem);
            Managers.ItemInventory.FindItemAndRemove(equipitem);
            //장비를 장착하고, 아이템 인벤토리에서 해당 아이템을 제거해준 후, Refresh()를 통해 UI를 갱신합니다.
            Refresh();
            return true;
        }
    }


    public bool AvailableEquip(Item equipitem)
    {
        //장비가 장착 가능한 장비인지 확인 !
        switch (equipitem.ItemType)
        {
            case ItemType.Weapon:
                Item.Weapon weapon = equipitem as Item.Weapon;
                if (!Character_Code.Equals(weapon.Available_code))
                    return false;
                break;
            case ItemType.Boot:
                Item.Boot boot = equipitem as Item.Boot;
                if (!JoBType.Equals(boot.JobType.ToString()))
                    return false;
                break;
            case ItemType.Hat:
                Item.Hat hat = equipitem as Item.Hat;
                if (!JoBType.Equals(hat.JobType.ToString()))
                    return false;
                break;
            case ItemType.Cloth:
                Item.Cloth cloth = equipitem as Item.Cloth;
                if (!JoBType.Equals(cloth.JobType.ToString()))
                    return false;
                break;
        }
        return true;
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
                    EQUIP_AttackRange += weapon.AttackRange;
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
                    EQUIP_MagicDef += hat.MagicDef;
                    break;
                case ItemType.Cloth:
                    Item.Cloth cloth = EQUIP[s] as Item.Cloth;
                    EQUIP_MaxHp += cloth.MaxHp;
                    EQUIP_Def += cloth.Def;
                    break;
                case ItemType.Earring:
                    Item.Earring earring = EQUIP[s] as Item.Earring;
                    EQUIP_Attack += earring.Attack;
                    EQUIP_HpRecovery += earring.HpRecovery;
                    EQUIP_MpRecovery += earring.MpRecovery;
                    break;
                case ItemType.Ring:
                    Item.Ring ring = EQUIP[s] as Item.Ring;
                    EQUIP_HpRecovery += ring.HpRecovery;
                    EQUIP_MpRecovery += ring.MpRecovery;
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
        EQUIP_Available_code = 0;
        EQUIP_Def = 0;
        EQUIP_MaxHp = 0;
        EQUIP_MaxMp = 0;
        EQUIP_MagicDef = 0;
        EQUIP_HpRecovery = 0;
        EQUIP_MpRecovery = 0;
        EQUIP_AttackRange = 0;
    }





}

