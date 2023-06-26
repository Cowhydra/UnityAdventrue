using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

//https://medium.com/geekculture/how-to-create-a-simple-behaviour-tree-in-unity-c-3964c84c060e
public class LowAI_BT : Behavior_Tree
{
    [SerializeField]
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

    [Header("발사체가 있는 몬스터는 여기")]
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

        waypoints.Add(gameObject.transform.position + Vector3.left * 2);
        waypoints.Add(gameObject.transform.position + Vector3.right *2);
        waypoints.Add(gameObject.transform.position + Vector3.forward * 2);
        waypoints.Add(gameObject.transform.position + Vector3.back * 2);

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
            new TaskPatrol(transform, waypoints),
        });

        return root;
    }
}
