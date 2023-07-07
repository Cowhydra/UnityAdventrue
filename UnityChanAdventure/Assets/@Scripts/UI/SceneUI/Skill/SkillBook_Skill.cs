using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillBook_Skill : UI_Scene
{
    private int _skillcode;
    public bool isIndicator;
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
        RefreshUI();
        GetComponent<Canvas>().sortingOrder = (int)Define.SortingOrder.SKillBook+1;
        if (isIndicator) return;
        gameObject.GetOrAddComponent<GraphicRaycaster>();
 
        gameObject.BindEvent((PointerEventData data) => OnDragEvent(data), Define.UIEvent.OnDrag);
        gameObject.BindEvent((PointerEventData data) => StartDrag(), Define.UIEvent.OnBeginDrag);
        gameObject.BindEvent((PointerEventData data) => DropEvent(), Define.UIEvent.OnEndDrag);
        _ParentCanvas = gameObject.transform.parent.parent.parent;
    }
    private GameObject Indicater;
    private Transform _ParentCanvas;
    private void RefreshUI()
    {
        GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>($"{Managers.Data.SkillDataDict[_skillcode].iconpath}");
    }
    private void StartDrag()
    {
        Indicater = Managers.UI.ShowSceneUI<SkillBook_Skill>().gameObject;
        Indicater.GetComponent<SkillBook_Skill>().isIndicator = true;
        Indicater.GetComponent<SkillBook_Skill>().SKillCode = this.SKillCode;
        Indicater.transform.SetParent(_ParentCanvas);
        Indicater.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
    }
    private void OnDragEvent(PointerEventData data)
    {
        if (Indicater == null) return;
        Indicater.transform.position = data.position;
    }
    private void DropEvent()
    {
        if (Indicater == null) return;
        Managers.Resource.Destroy(Indicater);
    }
}
