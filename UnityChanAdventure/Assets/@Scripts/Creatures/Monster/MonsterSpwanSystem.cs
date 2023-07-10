using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpwanSystem : MonoBehaviour
{

    [SerializeField]
    private Transform[] MonsterSpawnPoint;
    [SerializeField]
    private Define.MonsterEnvType CurrentEnv;
    List<int> MonsterCodeList=new List<int>();
    

    private void Start()
    {
        MonsterCodeList.Clear();
        SettingMonster();
        for(int i= 0; i < MonsterSpawnPoint.Length; i++)
        {
            RandSpawn(MonsterSpawnPoint[i]);
        }
    }

    private void SettingMonster()
    {
        switch (Managers.Scene.CurrentScene.SceneType)
        {
            case Define.Scene.LavaScene:
                CurrentEnv = Define.MonsterEnvType.Lava;
                break;
            case Define.Scene.DesertScene:
                CurrentEnv=Define.MonsterEnvType.Desert;
                break;
            case Define.Scene.WaterScene:
                CurrentEnv= Define.MonsterEnvType.Water;
                break;
            case Define.Scene.FightScene:
                CurrentEnv = Define.MonsterEnvType.None;
                break;
        }
        foreach (var monster in Managers.Data.MonsterDataDict.Values)
        {
            if (CurrentEnv == Define.MonsterEnvType.None)
            {
                if (monster.moncode % 10 != 0)
                {
                    MonsterCodeList.Add(monster.moncode);
                }
            }
            else
            {
                if (monster.EnvType == CurrentEnv && monster.moncode % 10 != 0)
                {
                    MonsterCodeList.Add(monster.moncode);
                }

            }
           
        }
    }
    private void RandSpawn(Transform transform)
    {
       for(int i = 0; i < 4; i++)
        {
           int randValue = Random.Range(0, MonsterCodeList.Count);
            GameObject monster = Managers.Resource.Instantiate(Managers.Data.MonsterDataDict[MonsterCodeList[randValue]].prefabPath);
            monster.gameObject.transform.position = transform.position;
            monster.SetRandomPositionSphere(2, 3);
        }
    }
}
