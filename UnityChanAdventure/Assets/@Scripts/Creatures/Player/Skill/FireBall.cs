using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Skill
{

    //데이터 따로 정리해야하는데 귀찮 
    protected override int SkillCode => 1001;

    protected override void AddSkill(Skill skill)
    {
        base.AddSkill(skill);
    }

    private void Start()
    {
        AddSkill(this);
    }
    protected override void ExcuteSkill(GameObject Owner)
    {
       //기능 구현
    }
}
