using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill 
{
    //IDEA
    // QWER�� ���� ��ȣ�� �����صΰ� , ��ȣ�� ������ ExcuteSKill�� ����
    //skillId ���õǾ� Data�� �����ؾ� ������.. ����ؿԴ��Ŷ� �̹��� ���ڽ��ϴ�.

    public virtual void ExcuteSkill(Transform Owner)
    {
        if (SkillCode == 0) return;
        Skill skill = null;
        Managers.SKill.MySkill.TryGetValue(SkillCode, out skill);
        if (skill == null)
        {
            Debug.Log("�̻��� ����? ���� ��ų ��ȣ ");
            return;
        }
    }
    protected abstract int SkillCode { get; }

}
