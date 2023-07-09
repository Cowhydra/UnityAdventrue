using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillManager 
{
    public Dictionary<int, Skill> MySkill { get; set; }=new Dictionary<int, Skill>();
    SKillFactory skillFactory=new SKillFactory();
    public void ExcuteSKill(int code,Transform Owner)
    {
        if (!MySkill.ContainsKey(code))
        {
            Debug.Log("TEMP");
            AddSKill(code);
        }
        MySkill[code].ExcuteSkill(Owner);
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
                return new Storm();
            case 300002:
                return new FireBall();
            case 300003:
                return new Meteo();
            case 300004:
               return new Heal();
            case 300005:
               return new Sanctuary();
            case 300006:
                return new IceBlast();
            //case 300007:
            //    return new Item1012Skill();
            default:
                throw new System.Exception($"Invalid skill ID : {skillid}");
        }
    }
}