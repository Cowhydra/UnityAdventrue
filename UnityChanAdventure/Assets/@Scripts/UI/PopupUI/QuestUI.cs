using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestUI : UI_Popup
{
    enum GameObjects
    {
        Blocker,
    }
    public override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        GetObject((int)GameObjects.Blocker).BindEvent((PointerEventData data) => Managers.UI. ClosePopupUI());
    }
    private void Start()
    {
        Init();
       
    }
}
