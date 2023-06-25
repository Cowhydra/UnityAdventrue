using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{

    //Selector는 oR게이트느낌
    public class Behavior_Selector : Behavior_Node
    {
        public Behavior_Selector() : base() { }
        public Behavior_Selector(List<Behavior_Node> children) : base(children) { }

        public override Define.Behavior_NodeState Evaluate()
        {
            foreach (Behavior_Node node in children)
            {
                switch (node.Evaluate())
                {
                    case Define.Behavior_NodeState.FAILURE:
                        continue;
                    case Define.Behavior_NodeState.SUCCESS:
                        state = Define.Behavior_NodeState.SUCCESS;
                        return state;
                    case Define.Behavior_NodeState.RUNNING:
                        state = Define.Behavior_NodeState.RUNNING;
                        return state;
                    default:
                        continue;
                }
            }

            state = Define.Behavior_NodeState.FAILURE;
            return state;
        }

    }
}
