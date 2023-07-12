using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Skill
{

    //데이터 따로 정리해야하는데 귀찮 
    protected override int SkillCode => 300002;

    //3이라고 했지만 추후 데이터를 이용해서 멀티 개수 생성이면 몇개 생성할지 설정 가능 
    public override void ExcuteSkill(Transform Owner)
    {
        base.ExcuteSkill(Owner);
        for (int i = 0; i < 5; i++)
        {  
          GameObject go =Managers.Resource.Instantiate($"{Managers.Data.SkillDataDict[SkillCode].prefabpath}");
            go.GetOrAddComponent<FireBall_Component>().Owner=Owner;
            
        }
        
    }

}
