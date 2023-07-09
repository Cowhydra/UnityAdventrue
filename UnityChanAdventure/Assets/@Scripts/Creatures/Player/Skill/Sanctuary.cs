using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sanctuary : Skill
{
    protected override int SkillCode => 300005;
    public override void ExcuteSkill(Transform Owner)
    {
        base.ExcuteSkill(Owner);
        GameObject go = Managers.Resource.Instantiate($"{Managers.Data.SkillDataDict[SkillCode].prefabpath}");
        go.GetOrAddComponent<Sanctuary_Component>().Owner = Owner;
    }
}
