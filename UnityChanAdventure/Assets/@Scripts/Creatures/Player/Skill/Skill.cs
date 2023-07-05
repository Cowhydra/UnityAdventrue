using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill :MonoBehaviour
{
    //IDEA
    // QWER에 각각 번호를 매핑해두고 , 번호를 누르면 ExcuteSKill할 예정
    //skillId 관련되어 Data를 정리해야 하지만.. 계속해왔던거라 이번엔 쉬겠습니다.
    protected List<Skill> skill_list=new List<Skill>();
    protected GameObject Player;
    protected virtual void AddSkill(Skill skill)
    {
        skill_list.Add(skill);
    }
    protected virtual void RemoveSkill(Skill skill)
    {
        skill_list.Remove(skill);
    }
    protected virtual void Init()
    {
        Player =GameObject.FindGameObjectWithTag("Player");
    }
    protected virtual void ExcuteSkill(GameObject Owner)
    {
        Transform target = Util.GetNbhdMonster(Owner.transform.position, 
            Owner == Player ? (int)Define.LayerMask.Player : (int)Define.LayerMask.Enemy, 
            Owner.TryGetComponent(out Monster monster) ? Managers.Data.MonsterDataDict[monster.MyCode].fovRange : 100);

    }
    public void ExcuteSkill(int skillcode,GameObject Owner)
    {
        if (skillcode == 0) return;
        Skill skill = null;
        skill = skill_list.Find(s => s.SkillCode == skillcode);
        if (skill == null)
        {
            Debug.Log("이상한 버그?");
            return;
        }
        skill.ExcuteSkill(Owner);
    }
    protected abstract int SkillCode { get; }

}
