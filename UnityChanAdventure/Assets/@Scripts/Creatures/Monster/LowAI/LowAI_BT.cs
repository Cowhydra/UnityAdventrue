using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

//https://medium.com/geekculture/how-to-create-a-simple-behaviour-tree-in-unity-c-3964c84c060e
public class LowAI_BT : Behavior_Tree
{

    public static int speed ;
    public static int fovRange ;
    public static int attackRange ;
    public static int attackdamage;

  
    public void SetStats(int speed,int fovRange)
    {
        LowAI_BT.speed = speed;
        LowAI_BT.fovRange = fovRange;
    }
    [SerializeField]
    private GameObject MyProjectile;
    protected override void Start()
    {
        base.Start();
        speed = Managers.Data.MonsterDataDict[GetComponent<Monster>().MyCode].speed;
        fovRange = Managers.Data.MonsterDataDict[GetComponent<Monster>().MyCode].fovRange;
        attackRange = Managers.Data.MonsterDataDict[GetComponent<Monster>().MyCode].attackRange;
        attackdamage = Managers.Data.MonsterDataDict[GetComponent<Monster>().MyCode].attack
            + Managers.Data.MonsterDataDict[GetComponent<Monster>().MyCode].level*10;

        if (Managers.Data.MonsterDataDict[GetComponent<Monster>().MyCode].AttackType == Define.MonsterAttackType.RangeAttack)
        {
            MyProjectile = Managers.Resource.Load<GameObject>($"{Managers.Data.MonsterDataDict[GetComponent<Monster>().MyCode].prefabPath}_Projectile");
        }
    }
    protected override Behavior_Node SetupTree()
    {

        Behavior_Node root = new Behavior_Selector(new List<Behavior_Node>
        { 
            new Behavior_Sequence(new List<Behavior_Node>
            {
                new CheckEnemyInAttackRange(transform),
                new TaskAttack(transform,MyProjectile),
            }),
             new Behavior_Sequence(new List<Behavior_Node>
            {
                new CheckEnemyInFovRange(transform),
                new TaskGotoTarget(transform),
            }),
           
            new TaskPatrol(transform),
        });

        return root;
    }
}
