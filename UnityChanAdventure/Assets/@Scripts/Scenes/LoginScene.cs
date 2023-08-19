using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScene : BaseScene
{
    public override void Clear()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        Managers.Data.Init();
        //Managers.DB.Init();
    }


}
