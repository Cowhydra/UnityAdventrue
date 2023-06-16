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
    public Dictionary<int, Data.GoodsData> GoodsDataDict { get; private set; } = new Dictionary<int, Data.GoodsData>();
    public Dictionary<int, Data.Stage> StageDataDict { get; private set; } = new Dictionary<int, Data.Stage>();
    public Dictionary<int, Data.Skill> SkillDataDict { get; private set; } = new Dictionary<int, Data.Skill>();
    public void Init()
    {
        ItemDataDict = LoadJson<Data.ItemLoader, int, Data.ItemData>("ItemData").MakeDict();
        CharacterDataDict = LoadJson<Data.CharacterLoader, int, Data.CharacterData>("CharacterData").MakeDict();
        GoodsDataDict = LoadJson<Data.GoodsDataLoader, int, Data.GoodsData>("GoodsData").MakeDict();
        StageDataDict = LoadJson<Data.StageDataLoader, int, Data.Stage>("StageData").MakeDict();
        SkillDataDict = LoadJson<Data.SKillDataLoader, int, Data.Skill>("SkillData").MakeDict();
    }


    //지정해둔 Loader형식을 받는 LoadJSon 함수 
    // Loader,와 Key,Value를 받은 후 , JsonUtility를 통해 Loader 형식으로 반환 해줍니다.
    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
