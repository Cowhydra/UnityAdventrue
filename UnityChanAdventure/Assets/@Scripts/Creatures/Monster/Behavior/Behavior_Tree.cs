using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace BehaviorTree
{
    //Behavior 트리는 결국 꼭대기 노드에서 부터 좌측으로 내려가면서 평가 (Evaluate()를 진행 ) 진행 하면서
    //실패가 나오면 다시 처음부터 돌아가는 느낌 ( Seuence, Selector 등 조건 노드(???)등을 자유롭게 설정할 수 있음
    // 노드 ( 노드, 노드 (노드,노드))) 등 다양한 구조 연출이 가능 

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


        private IEnumerator Update_co()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
            }

        }

    }
   
}

