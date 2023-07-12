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
       //��ȯ�� ���͵� ����
        SettingMonster();
        //����ȭ? �� ���ؼ� �ϴ� ��� ���� �ѹ� �����ϰ�...���� 
        //(���� ������ ���� �ʰ� ��� �� ��.. + ��� �ݺ� �����Ǵ� ģ�����̶�
        //�ϴ� ������ �����ε� �̸� ���� ���صθ� ù ���� �� 30���� ������ �� �־��� ��� 150������ �ѹ��� ������
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
    //���� ���� Ȯ�� �� ���� ������ �������� Ȯ��
    //����ȭ�� ���ؼ� -> List <Monster>�� ������ ����
    //�ű⿡ ����ִ� ���͸� �־��ְ� �װ� �Ǹ�  List���� ���� 
    // => �߾�(�Ŵ���) ���� �ϰų� �ؾ��� ��..
    //��� ������ ���ٸ� �߾Ӱ����� ���ҵ�. �ٵ� ���⼭�� ��  ����
    // ���� ���� �� ���� List���� �ش� ���� ( GetHashCdoe�� �ϴ���? ) �ؼ� ��������� �ϴµ� ���� ����� ��  ���ҵ�
    private bool isSpawnOk()
    {
        monsters.Clear();
        monsters = FindObjectsOfType<Monster>().ToList();
        return monsters.Count < 30;
    }
    public void MonsterSpawnStart()
    {
        // ������ ���� Clear()�� FindObjectsType�� �ϰԵǸ� ��ȿ���� -> �ڷ�ƾ�� �̿��ؼ� 5�ʸ��� ���� ���� Ȯ���ϵ��� ����
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

    //����ȭ�� ���ؼ� �ϴ� ���� ���� �� ��� ���͸� ���� �� ���� 
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