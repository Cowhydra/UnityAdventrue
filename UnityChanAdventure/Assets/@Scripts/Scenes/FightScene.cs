using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightScene : BaseScene
{
    private void Start()
    {
        Managers.UI.ShowSceneUI<FightSceneUI>();

        GameObject.FindObjectOfType<MyCharacter>().transform.position = gameObject.transform.position;

    }



    public override void Clear()
    {

    }

}
