using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

//https://medium.com/geekculture/how-to-create-a-simple-behaviour-tree-in-unity-c-3964c84c060e
public class LowAI_BT : Behavior_Tree
{
    //Guard
    protected List<Vector3> waypoints=new List<Vector3>();

    public static float speed = 2f;
    public static float fovRange = 6f;
    public static float attackRange = 2f;


    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _fovRange;
    [SerializeField]
    private float _attackRange;

    [Header("�߻�ü�� �ִ� ���ʹ� ����")]
    [SerializeField]
    private GameObject MyProjectile;
    private CharacterController characterController;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    protected override void Start()
    {
        base.Start();
        speed = _speed;
        fovRange = _fovRange;
        attackRange = _attackRange;
        waypoints.Add(gameObject.transform.position + Vector3.left * 4);
        waypoints.Add(gameObject.transform.position + Vector3.right * 4);
        waypoints.Add(gameObject.transform.position + Vector3.forward * 4);
        waypoints.Add(gameObject.transform.position + Vector3.back * 4);

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
                new TaskGotoTarget(transform,characterController),
            }),
            new TaskPatrol(transform, waypoints,characterController),
        });

        return root;
    }
}
