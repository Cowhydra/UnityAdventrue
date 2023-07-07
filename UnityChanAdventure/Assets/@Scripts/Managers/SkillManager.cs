using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillManager 
{
    public Dictionary<int, Skill> MySkill { get; set; }=new Dictionary<int, Skill>();
    SKillFactory skillFactory=new SKillFactory();
    public void ExcuteSKill(int code)
    {
        if (!MySkill.ContainsKey(code))
        {
            Debug.Log("TEMP");
            AddSKill(code);
        }
        MySkill[code].ExcuteSkill();
    }
    public void AddSKill(int skillid)
    {
        if (MySkill.ContainsKey(skillid))
        {
            return;
        }
        else
        {
            MySkill.Add(skillid, skillFactory.CreateSkill(skillid));
        }
    }
    public void ClearSKill()
    {
        MySkill = null;
    }
}
public class SKillFactory
{
    public Skill CreateSkill(int skillid)
    {
        switch (skillid)
        {
            case 300001:
                return new FireBall();
            case 300002:
                return new Storm();
            //case 300003:
            //    return new Item1004Skill();
            //case 300004:
            //    return new Item1005Skill();
            //case 300005:
            //    return new Item1009Skill();
            //case 300006:
            //    return new Item1011Skill();
            //case 300007:
            //    return new Item1012Skill();
            //case 300008:
            //    return new Item1016Skill();
            //case 300009:
            //    return new Item1018Skill();
            //case 300010:
            //    return new Item1019Skill();
            default:
                throw new System.Exception($"Invalid skill ID : {skillid}");
        }
    }
}