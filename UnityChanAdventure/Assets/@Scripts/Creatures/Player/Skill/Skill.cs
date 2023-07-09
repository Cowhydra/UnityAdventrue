using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill 
{
    //IDEA
    // QWER에 각각 번호를 매핑해두고 , 번호를 누르면 ExcuteSKill할 예정
    //skillId 관련되어 Data를 정리해야 하지만.. 계속해왔던거라 이번엔 쉬겠습니다.

    public virtual void ExcuteSkill(Transform Owner)
    {
        if (SkillCode == 0) return;
        Skill skill = null;
        Managers.SKill.MySkill.TryGetValue(SkillCode, out skill);
        if (skill == null)
        {
            Debug.Log("이상한 버그? 없는 스킬 번호 ");
            return;
        }
    }
    protected abstract int SkillCode { get; }

}
