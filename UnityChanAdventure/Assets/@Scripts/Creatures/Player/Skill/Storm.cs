using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storm : Skill
{
    protected override int SkillCode => 300001;
    [SerializeField]
    private int count = 6;
    
    //3이라고 했지만 추후 데이터를 이용해서 멀티 개수 생성이면 몇개 생성할지 설정 가능 
    public override void ExcuteSkill()
    {
        base.ExcuteSkill();
        for (int i = 0; i < count; i++)
        {
            GameObject go = Managers.Resource.Instantiate($"{Managers.Data.SkillDataDict[SkillCode].prefabpath}");
            Storm_Component strom= go.GetOrAddComponent<Storm_Component>();

            float angle = 360 / count;

            float x = Mathf.Cos(angle*i * Mathf.PI / 180.0f);
            float z= Mathf.Sin(angle *i* Mathf.PI / 180.0f);
            strom.MoveDir = new Vector3(x, 0, z);
        }
    }
}
