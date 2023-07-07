using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Data.CharacterData> CharacterDataDict { get; private set; } = new Dictionary<int, Data.CharacterData>();
    public Dictionary<int, Data.ItemData> ItemDataDict { get; private set; } = new Dictionary<int, Data.ItemData>();
    public Dictionary<int,Data.MonsterData> MonsterDataDict { get; private set; }=new Dictionary<int, Data.MonsterData>();
    public Dictionary<Define.ItemType, int> EquipData = new Dictionary<Define.ItemType, int>
    {
        {Define.ItemType.Boot,0 },
        {Define.ItemType.Cloth,0 },
        {Define.ItemType.Earring,0 },
        {Define.ItemType.Hat,0 },
        {Define.ItemType.Weapon,0 },
        {Define.ItemType.Ring,0 },

    };
    public Dictionary<int, Data.QuestData> QuestData { get; private set; } = new Dictionary<int, Data.QuestData>();
    public List<int> ItemCodes { get; private set; } = new List<int>();
    
    
    public Dictionary<int, Data.SkillData> SkillDataDict { get; private set; } = new Dictionary<int, Data.SkillData>();
    public void Init()
    {
        ItemDataDict = LoadJson<Data.ItemLoader, int, Data.ItemData>("ItemData").MakeDict();
        CharacterDataDict = LoadJson<Data.CharacterLoader, int, Data.CharacterData>("CharacterData").MakeDict();
        MonsterDataDict = LoadJson<Data.MonsterDataLoader, int, Data.MonsterData>("MonsterData").MakeDict();
        QuestData = LoadJson<Data.QuestDataLoader, int, Data.QuestData>("QuestData").MakeDict();
        SkillDataDict = LoadJson<Data.SKillDataLoader, int, Data.SkillData>("SkillData").MakeDict();
        foreach (var code in ItemDataDict.Keys)
        {
            ItemCodes.Add(code);
        }
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"@Resources/Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
