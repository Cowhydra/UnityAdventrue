using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckMyHp : Behavior_Node
{
    private Transform transform;
    private Animator _animator;
    private Define.BossHp BossHp;
    // Start is called before the first frame update
    public CheckMyHp(Transform transform,Define.BossHp CheckHp)
    {
        this.transform = transform;
        _animator = transform.GetComponent<Animator>();
        BossHp=CheckHp;
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
            switch (BossHp)
            {
                case Define.BossHp.Low:
                    if (monster.MyHpRatio() < 0.3f)
                    {
                        state = Define.Behavior_NodeState.SUCCESS;
                        return state;
                    }
                    break;
                case Define.BossHp.Middle:
                    if (monster.MyHpRatio() < 0.6f && monster.MyHpRatio() >= 0.3f)
                    {
                        state = Define.Behavior_NodeState.SUCCESS;
                        return state;
                    }
                    break;
                case Define.BossHp.High:
                    if (monster.MyHpRatio()>=0.6f)
                    {
                        state = Define.Behavior_NodeState.SUCCESS;
                        return state;
                    }
                    break;
            }
        
        }
        state = Define.Behavior_NodeState.FAILURE;
        return state;
    }
}
