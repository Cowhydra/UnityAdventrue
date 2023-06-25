using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace BehaviorTree
{

    public abstract class Behavior_Tree : UnityEngine.MonoBehaviour
    {

        private Behavior_Node _root = null;

        protected virtual void Start()
        {
            _root = SetupTree();
        }

        private void Update()
        {
            if (_root != null)
                _root.Evaluate();
        }

        protected abstract Behavior_Node SetupTree();

    }
}

