using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBook :MonoBehaviour
{
    void Start()
    {
        Init();
    }

    private  void Init()
    {
        foreach (Transform transforom in gameObject.GetComponentInChildren<Transform>())
        {
            Managers.Resource.Destroy(transforom.gameObject);
        }
        foreach (var i in Managers.Data.SkillDataDict.Keys)
        {
            SkillBook_Skill Skills = Managers.UI.ShowSceneUI<SkillBook_Skill>();
            Skills.transform.SetParent(gameObject.transform);
            Skills.SKillCode = i;
        }
    }
}
