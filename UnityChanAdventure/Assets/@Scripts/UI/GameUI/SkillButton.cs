using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SkillButton : MonoBehaviour
{
    //이벤트를 쓰지 않고 ㄱㄱ
    [SerializeField] private Define.SkillType _btnSKillType;
    public Define.SkillType ButtonSKillType
    {
        get {  return _btnSKillType; }
        private set
        {
            _btnSKillType = value;
        }
    }
    private GameObject ButtonIcon;
    [SerializeField]
    private int _btnskillcode;

    private void Start()
    {
        ButtonIcon = transform.GetChild(0).gameObject;
        SetButtonSKilType();
        Managers.Event.SkillInputAction -= ExcuteSKill;
        Managers.Event.SkillInputAction += ExcuteSKill;
    }
    private void SetButtonSKilType()
    {
        if (gameObject.name.Contains("Q"))
        {
            ButtonSKillType = Define.SkillType.QSkill;
        }
        else if (gameObject.name.Contains("W"))
        {
            ButtonSKillType = Define.SkillType.WSkill;
        }
        else if (gameObject.name.Contains("E"))
        {
            ButtonSKillType = Define.SkillType.ESkill;
        }
        else if (gameObject.name.Contains("R"))
        {
            ButtonSKillType = Define.SkillType.RSkill;
        }
        else
        {
            Debug.Log("Bug");
        }
    }
    private void ExcuteSKill(Define.SkillType skilltype)
    {
        if(skilltype== ButtonSKillType)
        {
            if (ButtonSkillcode == 0) return;
            Managers.SKill.ExcuteSKill(_btnskillcode,GameObject.FindAnyObjectByType<PlayerController>().transform);
        }
        else
        {
            return;
        }
    }
    public int ButtonSkillcode
    {
        get { return _btnskillcode;}
        set
        {
            _btnskillcode = value;
            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        if (_btnskillcode == 0)
        {
            ButtonIcon.GetComponent<Image>().sprite = null;
        }
        else
        {
            if (Managers.Data.SkillDataDict.ContainsKey(_btnskillcode))
            {
                ButtonIcon.GetComponent<Image>().sprite
                  = Managers.Resource.Load<Sprite>($"{Managers.Data.SkillDataDict[_btnskillcode].iconpath}");
            }
            else
            {
                Debug.Log("버그");
            }
        }
        SkillButton[] skillButtons = GameObject.FindObjectsOfType<SkillButton>();
        SkillButton targetbtn = skillButtons.FirstOrDefault(s => s._btnskillcode == this._btnskillcode && s.ButtonSKillType != this.ButtonSKillType);
        if(targetbtn != null)
        {
            targetbtn._btnskillcode = 0;
        }
    }

}
