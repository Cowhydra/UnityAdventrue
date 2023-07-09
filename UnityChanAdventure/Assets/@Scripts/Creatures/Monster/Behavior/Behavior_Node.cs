using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace BehaviorTree
{

    public class Behavior_Node
    {
        protected Define.Behavior_NodeState state;

        public Behavior_Node parent;
        protected List<Behavior_Node> children = new List<Behavior_Node>();
        //노드 하나 자체가 _dataContext를 가지고 있음 ->
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();


        public Behavior_Node()
        {
            parent = null;
        }
        public Behavior_Node(List<Behavior_Node> children)
        {
            //자식들을 부모에게 연결
            foreach (Behavior_Node child in children)
                _Attach(child);
        }

        private void _Attach(Behavior_Node Behavior_Node)
        {
            Behavior_Node.parent = this;
            children.Add(Behavior_Node);
        }
        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }
        //우선 현재 자신에서 찾고 부모에서 계속 찾음
        public object GetData(string key)
        {
            object value = null;
            if (_dataContext.TryGetValue(key, out value))
                return value;

            Behavior_Node Behavior_Node = parent;
            while (Behavior_Node != null)
            {
                value = Behavior_Node.GetData(key);
                if (value != Behavior_Node)
                {
                    return value;
                }
                Behavior_Node = Behavior_Node.parent;
            }
            return null;
        }

        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            Behavior_Node Behavior_Node = parent;
            while (Behavior_Node != null)
            {
                bool cleared = Behavior_Node.ClearData(key);
                if (cleared)
                {
                    return true;
                }
                Behavior_Node = Behavior_Node.parent;
            }
            return false;
        }
        public virtual Define.Behavior_NodeState Evaluate() => Define.Behavior_NodeState.FAILURE;

    }

}