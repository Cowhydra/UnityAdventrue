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
    //public Dictionary<int, Data.Skill> SkillDataDict { get; private set; } = new Dictionary<int, Data.Skill>();
    public void Init()
    {
        ItemDataDict = LoadJson<Data.ItemLoader, int, Data.ItemData>("ItemData").MakeDict();
        CharacterDataDict = LoadJson<Data.CharacterLoader, int, Data.CharacterData>("CharacterData").MakeDict();
        MonsterDataDict = LoadJson<Data.MonsterDataLoader, int, Data.MonsterData>("MonsterData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
