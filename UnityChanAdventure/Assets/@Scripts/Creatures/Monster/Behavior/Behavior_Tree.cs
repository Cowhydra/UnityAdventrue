using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace BehaviorTree
{
    //Behavior Ʈ���� �ᱹ ����� ��忡�� ���� �������� �������鼭 �� (Evaluate()�� ���� ) ���� �ϸ鼭
    //���а� ������ �ٽ� ó������ ���ư��� ���� ( Seuence, Selector �� ���� ���(???)���� �����Ӱ� ������ �� ����
    // ��� ( ���, ��� (���,���))) �� �پ��� ���� ������ ���� 

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

