using System.Collections;
using System.Collections.Generic;
using UnityEditor.Recorder;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillBook_UI : UI_Scene
{
    // Start is called before the first frame update
    void Start()
    {
        Init();  
    }
    enum Images
    {
        SkillBook_Close,
    }
    public override void Init()
    {
        base.Init();
        GetComponent<Canvas>().sortingOrder = (int)Define.SortingOrder.SKillBook;
        Bind<Image>(typeof(Images));
        Get<Image>((int)Images.SkillBook_Close).gameObject
            .BindEvent((PointerEventData data) => Managers.Resource.Destroy(gameObject));
    }
}
