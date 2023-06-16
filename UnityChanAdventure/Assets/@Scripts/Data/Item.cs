using Data;
using static Define;

public class Item
{

    //아이템 입니다. 아이템 데이터에서 데이터를 받아와
    //실제 사용될 아이템으로 넣어줍니다.
    //maKEiTEM의 경우 던전 클리어 보상, 보물상자 보상 ,뽑기 보상 등
    //다양하게 아이템을 유저에게 건넬 떄 사용하기에 sTATIC으로 구현했습니다.
    // 공통적인 속성은 ITem으로, 나머지 item의 경우 타입에 따라 다르게 받을 수 있도록 설정했습니다.
    public ItemData Info { get; } = new ItemData();

    public int ItemCode { get { return Info.itemcode; } set { Info.itemcode = value; } }
    public string ItemName { get { return Info.name; } set { Info.name = value; } }
    public string ItemTooltip { get { return Info.itemtooltip; } set { Info.itemtooltip = value; } }
    public string ItemIconPath { get { return Info.iconpath; } set { Info.iconpath = value; } }
    public int Count { get { return Info.count; } set { Info.count = value; } }
    public ItemType ItemType { get; private set; }

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

        }

        if (item != null)
        {
            item.ItemCode = itemInfo.itemcode;
            item.ItemName = itemInfo.name;
            item.ItemIconPath = itemInfo.iconpath;
            item.ItemTooltip = itemInfo.itemtooltip;
            

        }

        return item;
    }

    public class Weapon : Item
    {
        public int Attack { get; private set; }
        public int MagicAttack { get; private set; }
        public int AttackRange { get; private set; }
        public int Available_code { get; private set; }
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
                ItemCode = data.itemcode;
                Count = 1;
                Attack = data.attack;
                MagicAttack = data.magicattack;
                AttackRange = data.range;
                Available_code = data.available_code;

            }
        }
    }
    public class Boot : Item
    {
        public int Def { get; private set; }
        public int MaxHp { get; private set; }
        public string JobType { get; private set; }
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
                ItemCode = data.itemcode;
                Count = 1;
                Def = data.def;
                MaxHp = data.hp;
                JobType = data.jobtype;

            }
        }
    }

    public class Hat : Item
    {
        public int MaxMp { get; private set; }
        public int MagicDef { get; private set; }
        public string JobType { get; private set; }
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
                ItemCode = data.itemcode;
                Count = 1;
                MaxMp = data.mp;
                MagicDef = data.magicdef;
                JobType = data.jobtype;

            }
        }
    }
    public class Cloth : Item
    {
        public int MaxHp { get; private set; }
        public int Def { get; private set; }
        public string JobType { get; private set; }
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
                ItemCode = data.itemcode;
                Count = 1;
                MaxHp = data.hp;
                Def = data.def;
                JobType = data.jobtype;

            }
        }
    }
    public class Earring : Item
    {
        public int Attack { get; private set; }
        public int HpRecovery { get; private set; }
        public int MpRecovery { get; private set; }
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
                ItemCode = data.itemcode;
                Count = 1;
                HpRecovery = data.hprecovery;
                MpRecovery = data.mprecovery;


            }
        }
    }

    public class Ring : Item
    {
        public int HpRecovery { get; private set; }
        public int MpRecovery { get; private set; }
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
                ItemCode = data.itemcode;
                Count = 1;
                HpRecovery = data.hprecovery;
                MpRecovery = data.mprecovery;


            }
        }
    }
    public class Consume: Item
    {
        public int Value { get; private set; }
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
                ItemCode = data.itemcode;
                Count = 1;
                Value = data.value;
            }
        }
    }
    public class Ingredient : Item
    {
        public int Value { get; private set; }
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

            ETCData data = (ETCData)itemData;
            {
                ItemCode = data.itemcode;
                Count = 1;
            }
        }
    }


}