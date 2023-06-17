using Data;
using System.Diagnostics;
using static Define;

public class Item
{
    public ItemData Info { get; } = new ItemData();

    public int ItemCode { get { return Info.itemcode; } }
    public string ItemName { get { return Info.name; } }
    public ItemType ItemType { get; private set; }
    public string ItemIconPath { get { return Info.iconPath; } }
    public string ItemTooltip { get { return Info.itemtooltip; } }
    public int Count { get { return Info.count; }set { Info.count=value; }}
    public int Price { get { return Info.price; }}
    public ItemGrade ItemGrade { get; private set; }

    public Item(ItemType itemType)
    {
        ItemType = itemType;
    }

    public static Item MakeItem(ItemData itemInfo)
    {
        Item item = null;

        ItemData itemData = null;
        Managers.Data.ItemDataDict.TryGetValue(itemInfo.itemcode, out itemData);

        if (itemData == null)
            return null;

        switch (itemData.itemType)
        {
            case ItemType.Weapon:
                item = new Weapon(itemInfo.itemcode);
                break;
            case ItemType.Hat:
                item = new Hat(itemInfo.itemcode);
                break;
            case ItemType.Cloth:
                item = new Cloth(itemInfo.itemcode);
                break;
            case ItemType.Boot:
                item = new Boot(itemInfo.itemcode);
                break;
            case ItemType.Earring:
                item = new Earring(itemInfo.itemcode);
                break;
            case ItemType.Ring:
                item = new Ring(itemInfo.itemcode);
                break;
            case ItemType.Consume:
                item = new Consume(itemInfo.itemcode);
                break;
            case ItemType.Ingredient:
                item = new Ingredient(itemInfo.itemcode);
                break;
        }
        if (item == null)
        {
            throw new System.Exception("아이템 설정 오류");
        }

        return item;
    }

    public class Weapon : Item
    {
        public int Attack { get; private set; }
        public int MagicAttack { get; private set; }
        public string PrefabPath { get; private set; }
        public Weapon(int templateId) : base(ItemType.Weapon)
        {
            Init(templateId);
        }

        void Init(int templateId)
        {
            ItemData itemData = null;
            Managers.Data.ItemDataDict.TryGetValue(templateId, out itemData);
            if (itemData.itemType != ItemType.Weapon)
                return;

            WeaponData data = (WeaponData)itemData;
            {
                Attack = data.attack;
                MagicAttack = data.magicattack;
                PrefabPath=data.prefabPath;
            }
        }
    }
    public class Boot : Item
    {
        public int Def { get; private set; }
        public int MaxHp { get; private set; }
        public Boot(int templateId) : base(ItemType.Boot)
        {
            Init(templateId);
        }

        void Init(int templateId)
        {
            ItemData itemData = null;
            Managers.Data.ItemDataDict.TryGetValue(templateId, out itemData);
            if (itemData.itemType != ItemType.Boot)
                return;

            BootData data = (BootData)itemData;
            {
                Def = data.def;
                MaxHp = data.hp;
            }
        }
    }

    public class Hat : Item
    {
        public int MaxMp { get; private set; }
        public int Def { get; private set; }
        public Hat(int templateId) : base(ItemType.Hat)
        {
            Init(templateId);
        }

        void Init(int templateId)
        {
            ItemData itemData = null;
            Managers.Data.ItemDataDict.TryGetValue(templateId, out itemData);
            if (itemData.itemType != ItemType.Hat)
                return;

            HatData data = (HatData)itemData;
            {

                MaxMp = data.mp;
                Def=data.def;

            }
        }
    }
    public class Cloth : Item
    {
        public int MaxHp { get; private set; }
        public int Magicdef { get; private set; }
        public Cloth(int templateId) : base(ItemType.Cloth)
        {
            Init(templateId);
        }

        void Init(int templateId)
        {
            ItemData itemData = null;
            Managers.Data.ItemDataDict.TryGetValue(templateId, out itemData);
            if (itemData.itemType != ItemType.Cloth)
                return;

            ClothData data = (ClothData)itemData;
            {
                Magicdef= data.magicdef;
                MaxHp = data.hp;
            }
        }
    }
    public class Earring : Item
    {
        public int Attack { get; private set; }
        public int MagicAttack { get; private set; }
        public Earring(int templateId) : base(ItemType.Earring)
        {
            Init(templateId);
        }

        void Init(int templateId)
        {
            ItemData itemData = null;
            Managers.Data.ItemDataDict.TryGetValue(templateId, out itemData);
            if (itemData.itemType != ItemType.Earring)
                return;

            EarringData data = (EarringData)itemData;
            {
                Attack= data.attack;
                MagicAttack= data.magicattack;

            }
        }
    }

    public class Ring : Item
    {
        public int MaxHp { get; private set; }
        public int MaxMp { get; private set; }
        public Ring(int templateId) : base(ItemType.Ring)
        {
            Init(templateId);
        }

        void Init(int templateId)
        {
            ItemData itemData = null;
            Managers.Data.ItemDataDict.TryGetValue(templateId, out itemData);
            if (itemData.itemType != ItemType.Ring)
                return;

            RingData data = (RingData)itemData;
            {
                MaxHp = data.hp;
                MaxMp = data.mp;
            }
        }
    }
    public class Consume: Item
    {
        public int Hp { get; private set; }
        public int Mp { get; private set; }
        public Consume(int templateId) : base(ItemType.Consume)
        {
            Init(templateId);
        }

        void Init(int templateId)
        {
            ItemData itemData = null;
            Managers.Data.ItemDataDict.TryGetValue(templateId, out itemData);
            if (itemData.itemType != ItemType.Consume)
                return;

            ConsumeData data = (ConsumeData)itemData;
            {
                Hp = data.hp;
                Mp = data.mp;
            }
        }
    }
    public class Ingredient : Item
    {
        public Ingredient(int templateId) : base(ItemType.Ingredient)
        {
            Init(templateId);
        }

        void Init(int templateId)
        {
            ItemData itemData = null;
            Managers.Data.ItemDataDict.TryGetValue(templateId, out itemData);
            if (itemData.itemType != ItemType.Ingredient)
                return;

            IngredientData data = (IngredientData)itemData;
            {
             
            }
        }
    }


}