using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckMyHp : Behavior_Node
{
    private Transform transform;
    private Animator _animator;
    // Start is called before the first frame update
    public CheckMyHp(Transform transform)
    {
        this.transform = transform;
        _animator = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    public override Define.Behavior_NodeState Evaluate()
    {
        object t = GetData("target");
        if (t == null)
        {
            state = Define.Behavior_NodeState.FAILURE;
            return state;
        }
        if(transform.TryGetComponent(out Monster monster))
        {
            if (monster.MyHpRatio() < 0.3f)
            {
                state= Define.Behavior_NodeState.SUCCESS;
                return state;
            }
        }
        state = Define.Behavior_NodeState.FAILURE;
        return state;
    }
}
