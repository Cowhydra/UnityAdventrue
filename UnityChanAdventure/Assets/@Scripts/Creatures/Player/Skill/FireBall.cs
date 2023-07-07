using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Skill
{

    //������ ���� �����ؾ��ϴµ� ���� 
    protected override int SkillCode => 300002;

    //3�̶�� ������ ���� �����͸� �̿��ؼ� ��Ƽ ���� �����̸� � �������� ���� ���� 
    public override void ExcuteSkill()
    {
        base.ExcuteSkill();
        for (int i = 0; i < 10; i++)
        {  
          GameObject go =Managers.Resource.Instantiate($"{Managers.Data.SkillDataDict[SkillCode].prefabpath}");
            go.GetOrAddComponent<FireBall_Component>();
            
        }
    }

}
