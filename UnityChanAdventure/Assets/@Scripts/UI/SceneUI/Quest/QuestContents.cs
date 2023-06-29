using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestContents : MonoBehaviour
{
    SortedDictionary<int, Data.QuestData> Questdata = new SortedDictionary<int, Data.QuestData>();
    MyCharacter character;
    private void Init()
    { 
        character = FindObjectOfType<MyCharacter>();
        foreach (Transform transforom in gameObject.GetComponentInChildren<Transform>())
        {
            Managers.Resource.Destroy(transforom.gameObject);
        }

        foreach (var i in Managers.Data.QuestData.Values)
        {
            if (i.isCleared == false&&i.State==Define.QuestState.Pending)
            {
                Questdata.Add(i.LevelRequirement,i);
            }
        }
        foreach(var i in Questdata.Values)
        {
            Debug.Log("���� ���� �ٽ� ��Ȱ�� �ؾ���");
            //if (i < character.Level)
            {
                QuestInfo questinfo=Managers.UI.ShowSceneUI<QuestInfo>();
                questinfo.transform.SetParent(gameObject.transform);
                questinfo.QuestID = i.UniqueId;
            }
        }
    }

    private void Start()
    {
        Init();

    }
    private void OnDestroy()
    {
        Questdata.Clear();
    }
}
