﻿using System;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using static Define;

namespace Data
{
    #region Item
    [Serializable]
    public class ItemData
    {
        public int itemcode;
        public string name;
        public ItemType itemType;
        public string iconPath;
        public string itemtooltip;
        public int count;
        public int price;
        public string itemgrade;
        public ItemGrade itemGrade;

    }
    [Serializable]
    public class WeaponData : ItemData
    {
        public int attack;
        public int magicattack;
        public string prefabPath;
    }
    [Serializable]
    public class BootData : ItemData
    {
        public int def;
        public int hp;
    }
    [Serializable]
    public class HatData : ItemData
    {
        public int mp;
        public int def;
    }
    [Serializable]
    public class ClothData : ItemData
    {
        public int hp;
        public int magicdef;
    }
    [Serializable]
    public class EarringData : ItemData
    {
        public int attack;
        public int magicattack;
    }
    [Serializable]
    public class RingData : ItemData
    {
        public int hp;
        public int mp;
    }
    [Serializable]
    public class ConsumeData : ItemData
    {
        public int hp;
        public int mp;
    }
    [Serializable]
    public class IngredientData : ItemData
    {
    }


    [Serializable]
    public class ItemLoader : ILoader<int, ItemData>
    {
        public List<WeaponData> Weapon = new List<WeaponData>();
        public List<BootData> Boot = new List<BootData>();
        public List<HatData> Hat = new List<HatData>();
        public List<ClothData> Cloth = new List<ClothData>();
        public List<EarringData> Earing = new List<EarringData>();
        public List<RingData> Ring = new List<RingData>();
        public List<ConsumeData> Consume = new List<ConsumeData>();
        public List<IngredientData> Ingredient = new List<IngredientData>();

        public Dictionary<int, ItemData> MakeDict()
        {
            Dictionary<int, ItemData> dict = new Dictionary<int, ItemData>();
            foreach (ItemData item in Weapon)
            {
                item.itemType = ItemType.Weapon;
                item.itemGrade = (ItemGrade)Enum.Parse(typeof(ItemGrade),item.itemgrade);
                dict.Add(item.itemcode, item);
            }
            foreach (ItemData item in Boot)
            {
                item.itemType = ItemType.Boot;
                item.itemGrade = (ItemGrade)Enum.Parse(typeof(ItemGrade), item.itemgrade);

                dict.Add(item.itemcode, item);
            }
            foreach (ItemData item in Hat)
            {
                item.itemType = ItemType.Hat;
                item.itemGrade = (ItemGrade)Enum.Parse(typeof(ItemGrade), item.itemgrade);

                dict.Add(item.itemcode, item);
            }
            foreach (ItemData item in Cloth)
            {
                item.itemType = ItemType.Cloth;
                item.itemGrade = (ItemGrade)Enum.Parse(typeof(ItemGrade), item.itemgrade);

                dict.Add(item.itemcode, item);
            }
            foreach (ItemData item in Earing)
            {
                item.itemType = ItemType.Earring;
                item.itemGrade = (ItemGrade)Enum.Parse(typeof(ItemGrade), item.itemgrade);

                dict.Add(item.itemcode, item);
            }
            foreach (ItemData item in Ring)
            {
                item.itemType = ItemType.Ring;
                item.itemGrade = (ItemGrade)Enum.Parse(typeof(ItemGrade), item.itemgrade);

                dict.Add(item.itemcode, item);
            }
            foreach (ItemData item in Consume)
            {
                item.itemType = ItemType.Consume;
                item.itemGrade = (ItemGrade)Enum.Parse(typeof(ItemGrade), item.itemgrade);
                dict.Add(item.itemcode, item);
            }
            foreach (ItemData item in Ingredient)
            {
                item.itemType = ItemType.Ingredient;
                item.itemGrade = (ItemGrade)Enum.Parse(typeof(ItemGrade), item.itemgrade);
                dict.Add(item.itemcode, item);
            }
            return dict;
        }
    }
    #endregion

    #region Character
    [Serializable]
    public class CharacterData
    {
        public string name;
        public int charcode;

        public string jobType;
        public int maxhp;
        public int maxmana;
        public int magicdef;
        public int def;
        public int magicattack;
        public int attack;
        public int attackspeed;
        public int level;
        public string iconPath;
        public string prefabPath;


    }
    [Serializable]
    public class CharacterLoader : ILoader<int, CharacterData>
    {
        public List<CharacterData> Character = new List<CharacterData>();

        public Dictionary<int, CharacterData> MakeDict()
        {
            Dictionary<int, CharacterData> dict = new Dictionary<int, CharacterData>();
            foreach (CharacterData character in Character)
                dict.Add(character.charcode, character);
            return dict;
        }
    }

    #endregion

    #region Monster
    [Serializable]
    public class MonsterData
    {
        public string name;
        public int moncode;
        public string environment;
        public int maxhp;
        public int def;
        public int attack;
        public int level;
        public string prefabPath;

        public MonsterEnvType EnvType;
    }
    [Serializable]
    public class MonsterDataLoader : ILoader<int, MonsterData>
    {
        public List<MonsterData> Monster = new List<MonsterData>();

        public Dictionary<int, MonsterData> MakeDict()
        {
            Dictionary<int, MonsterData> dict = new Dictionary<int, MonsterData>();
            foreach (MonsterData mon in Monster)
            {
                mon.EnvType = (MonsterEnvType)Enum.Parse(typeof(MonsterEnvType), mon.environment);
                dict.Add(mon.moncode, mon);
              
            }
             
            return dict;
        }
    }

    #endregion

    #region Skills
    [Serializable]
    public class Skill
    {
        public int skillcode;
        public string skillName;
        public int skillDamage;
        public string skillInfo;
    }
    [Serializable]
    public class SKillDataLoader : ILoader<int, Skill>
    {
        public List<Skill> Skills = new List<Skill>();
        public Dictionary<int, Skill> MakeDict()
        {
            Dictionary<int, Skill> dict = new Dictionary<int, Skill>();
            foreach (Skill skill in Skills)
            {
                dict.Add(skill.skillcode, skill);
            }
            return dict;
        }
    }



    #endregion
}