using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class AutoFight_BT : Behavior_Tree
{
    public static int speed;
    public static int fovRange;
    public static int attackRange;
    public static int attackdamage;
    private PlayerController playerController;


    //오토 만들 떄 해야 할일  캐릭터 컨트롤러 껏다 켜기 + 애니메이션, 기본 이동 설계를 .. 캐릭터 컨트롤러로 해놔서
    //오토 때문에 다시 Navi로 바꾸는건 이상함 
    protected override void Start()
    {

        playerController = GetComponent<PlayerController>();
        speed = 20;
        fovRange = 40;
        attackRange = 3;
        attackdamage = 25;
        base.Start();
    }
    protected override Behavior_Node SetupTree()
    {

        Behavior_Node root = new Behavior_Selector(new List<Behavior_Node>
        {
            new Behavior_Sequence(new List<Behavior_Node>
            {
                new CheckEnemyInAttackRange_Player(transform),
                new TaskAttack_Player(transform,playerController)
            
            }),
             new Behavior_Sequence(new List<Behavior_Node>
            {
                new CheckEnemyInFovRange_Player(transform),
                new TaskGotoTarget(transform)
              
            }),

            new TaskPatorl_Player(transform),
        });

        return root;
    }
}
