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
       //소환될 몬스터들 세팅
        SettingMonster();
        //최적화? 를 위해서 일단 모든 몬스터 한번 실행하고...삭제 
        //(몬스터 개수가 많지 않고 계속 될 것.. + 계속 반복 생성되는 친구들이라
        //일단 폴링할 예정인데 미리 생성 안해두면 첫 스폰 시 30마리 생성될 때 최악의 경우 150마리가 한번에 생성됨
        StartCoroutine(nameof(MonsterSpawnInit));
        Managers.UI.ShowSceneUI<PlayerStatus_Canvas>();
        Managers.UI.ShowSceneUI<GameUI>();

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
    //몬스터 개수 확인 후 스폰 가능한 상태인지 확인
    //최적화를 위해선 -> List <Monster>를 선언한 이후
    //거기에 살아있는 몬스터를 넣어주고 죽게 되면  List에서 제거 
    // => 중앙(매니저) 관리 하거나 해야할 듯..
    //모든 씬에서 쓴다면 중앙관리가 편할듯. 근데 여기서만 쓸  예정
    // 몬스터 죽을 떄 마다 List에서 해당 몬스터 ( GetHashCdoe를 하던가? ) 해서 제거해줘야 하는데 지금 방식이 더  편할듯
    private bool isSpawnOk()
    {
        monsters.Clear();
        monsters = FindObjectsOfType<Monster>().ToList();
        return monsters.Count < 30;
    }
    public void MonsterSpawnStart()
    {
        // 프레임 마다 Clear()랑 FindObjectsType을 하게되면 비효율적 -> 코루틴을 이용해서 5초마다 스폰 여부 확인하도록 설정
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
            yield return new WaitForSeconds(5.0f);
        }


    }

    //최적화를 위해서 일단 게임 시작 시 모든 몬스터를 생성 후 제거 
    IEnumerator MonsterSpawnInit()
    {
       foreach(int i in MonsterCodeList)
        {
            GameObject monster = Managers.Resource.Instantiate(Managers.Data.MonsterDataDict[i].prefabPath);
            yield return null;
        }
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