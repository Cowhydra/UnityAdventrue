using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlast : Skill
{
    protected override int SkillCode => 300006;
    public override void ExcuteSkill(Transform Owner)
    {
        base.ExcuteSkill(Owner);
        GameObject go = Managers.Resource.Instantiate($"{Managers.Data.SkillDataDict[SkillCode].prefabpath}");
        go.GetOrAddComponent<IceBlast_Component>().Owner=Owner;
    }

}
