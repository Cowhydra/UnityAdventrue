using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteo : Skill
{
    protected override int SkillCode => 300003;

    public override void ExcuteSkill(Transform Owner)
    {
        base.ExcuteSkill(Owner);
        GameObject go = Managers.Resource.Instantiate($"{Managers.Data.SkillDataDict[SkillCode].prefabpath}");
        go.GetOrAddComponent<Meteo_Component>().Owner=Owner;
    }


}
