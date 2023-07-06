using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class FightScene_MonsterSpawn : MonoBehaviour
{
    [SerializeField]
    private Transform[] MonsterSpawnPoint;
    List<int> MonsterCodeList = new List<int>();
    List<Monster> monsters = new List<Monster>();
    private void Start()
    {
        MonsterCodeList.Clear();
        SettingMonster();
        Managers.UI.ShowSceneUI<PlayerStatus_Canvas>();
        Managers.UI.ShowSceneUI<GameUI>();
        StartCoroutine(nameof(MonsterSpawnInit));
    }
    private void SettingMonster()
    {
        foreach (var monster in Managers.Data.MonsterDataDict.Values)
        {
            if (monster.moncode % 10 != 0)
            {
                MonsterCodeList.Add(monster.moncode);
            }
        }
    }
    private void RandSpawn(Transform transform)
    {
        for (int i = 0; i < 2; i++)
        {
            int randValue = Random.Range(0, MonsterCodeList.Count);
            GameObject monster = Managers.Resource.Instantiate(Managers.Data.MonsterDataDict[MonsterCodeList[randValue]].prefabPath);
         
            monster.gameObject.transform.position = transform.position;
            monster.SetRandomPositionSphere(2, 3);
        }
    }
    private bool isSpawnOk()
    {
        monsters.Clear();
        monsters = FindObjectsOfType<Monster>().ToList();
        return monsters.Count < 30;
    }
    public void MonsterSpawnStart()
    {
        StartCoroutine(nameof(MonsterSpawn));
    }
    IEnumerator MonsterSpawn()
    {
        while (true)
        {
            if (isSpawnOk())
            {
                for(int i = 0; i < MonsterSpawnPoint.Length; i++)
                {
                    RandSpawn(MonsterSpawnPoint[i].transform);
                    
                }
            }
            else
            {

            }
            yield return new WaitForSeconds(3.0f);
        }


    }
    IEnumerator MonsterSpawnInit()
    {
        StartCoroutine(nameof(MonsterSpawn));
        yield return null;
        ClearMonster();
    }


    public void ClearMonster()
    {
        StopAllCoroutines();
        monsters.Clear();
        monsters = FindObjectsOfType<Monster>().ToList();
        foreach(var mon in monsters)
        {
            Managers.Resource.Destroy(mon.gameObject);
        }
    }
}