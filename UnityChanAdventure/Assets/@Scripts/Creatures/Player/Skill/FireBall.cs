using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Skill
{

    //������ ���� �����ؾ��ϴµ� ���� 
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
       //��� ����
    }
}
