using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillBook_Skill : UI_Scene
{
    private int _skillcode;
    public int SKillCode
    {
        get {  return _skillcode;}
        set
        {
            _skillcode = value;
            Init();
        }
    }
    public override void Init()
    {
        base.Init();
        GetComponent<Canvas>().sortingOrder = (int)Define.SortingOrder.SKillBook+1;
        gameObject.GetOrAddComponent<GraphicRaycaster>();
        RefreshUI();
        gameObject.BindEvent((PointerEventData data) => gameObject.transform.position = data.position, Define.UIEvent.OnDrag);

    }
    private void RefreshUI()
    {
        GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>($"{Managers.Data.SkillDataDict[_skillcode].iconpath}");
    }

}
