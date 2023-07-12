using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : BaseScene
{
    // Start is called before the first frame update
    void Start()
    {
        Managers.Data.Init();
        Managers.EQUIP.Init();
        Managers.Scene.LoadScene(Define.Scene.TownScene);
    }

    public override void Clear()
    {
       
    }
}
