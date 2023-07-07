using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBook_UI : UI_Scene
{
    // Start is called before the first frame update
    void Start()
    {
        Init();  
    }
    public override void Init()
    {
        base.Init();
        GetComponent<Canvas>().sortingOrder = (int)Define.SortingOrder.SKillBook;
    }
}
