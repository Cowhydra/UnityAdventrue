using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpwanSystem : MonoBehaviour
{

    [SerializeField]
    private Transform[] MonsterSpawnPoint;
    [SerializeField]
    private Define.MonsterEnvType CurrentEnv;
    List<GameObject> MonsterList=new List<GameObject>();

    private void Start()
    {
        MonsterList.Clear();
        SettingMonster();
        for(int i= 0; i < MonsterSpawnPoint.Length; i++)
        {
            RandSpawn(MonsterSpawnPoint[i]);
        }
        Debug.Log("���� ������ �س��� ���� ������Ʈ ( Behavior Tree) ���� �س��ƾ� ��");
    }

    private void SettingMonster()
    {
        foreach (var monster in Managers.Data.MonsterDataDict.Values)
        {
            if (monster.EnvType == CurrentEnv && monster.moncode % 10 != 0)
            {
                MonsterList.Add(Managers.Resource.Load<GameObject>($"{monster.prefabPath}")); ;
            }
        }
    }
    private void RandSpawn(Transform transform)
    {
       for(int i = 0; i < 5; i++)
        {
            int randValue = Random.Range(0, MonsterList.Count);
           GameObject monster = Instantiate(MonsterList[randValue]);
           monster.SetRandomPositionSphere(2, 3);
        }
    }
}
