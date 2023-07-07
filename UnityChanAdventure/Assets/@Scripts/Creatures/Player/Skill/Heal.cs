using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Skill
{
    protected override int SkillCode => 300004;

    public override void ExcuteSkill(Transform Owner)
    {
        base.ExcuteSkill(Owner);
        GameObject go = Managers.Resource.Instantiate($"{Managers.Data.SkillDataDict[SkillCode].prefabpath}");
        go.GetOrAddComponent<Heal_Component>().Owner = Owner;
    }

}
