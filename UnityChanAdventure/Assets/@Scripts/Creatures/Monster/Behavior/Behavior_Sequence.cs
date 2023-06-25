using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace BehaviorTree
{
    //Sequence는 And게이트 느낌
    public class Behavior_Sequence : Behavior_Node
    {
        public Behavior_Sequence() : base() { }
        public Behavior_Sequence(List<Behavior_Node> children) : base(children) { }

        public override Behavior_NodeState Evaluate()
        {
            bool anyChildIsRunning = false;

            foreach (Behavior_Node Behavior_Node in children)
            {
                switch (Behavior_Node.Evaluate())
                {
                    case Behavior_NodeState.FAILURE:
                        state = Behavior_NodeState.FAILURE;
                        return state;
                    case Behavior_NodeState.SUCCESS:
                        continue;
                    case Behavior_NodeState.RUNNING:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        state = Behavior_NodeState.SUCCESS;
                        return state;
                }
            }

            state = anyChildIsRunning ? Behavior_NodeState.RUNNING : Behavior_NodeState.SUCCESS;
            return state;
        }

    }
}

