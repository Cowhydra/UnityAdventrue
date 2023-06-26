using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class LazyPattern : Behavior_Node
{

    private Transform transform;
    private Animator _animator;
    public LazyPattern(Transform transform)
    {
        this.transform = transform;
        _animator = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
