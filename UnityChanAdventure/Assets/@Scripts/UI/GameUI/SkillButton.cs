using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class SkillButton : MonoBehaviour
{


    [SerializeField]
    private int _btnskillcode;
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
    public int ButtonSkillcode
    {
        get { return _btnskillcode; }
        set
        {
            _btnskillcode = value;
            RefreshUI();
        }
    }



    //이것도 원래 동적으로 생성해줘야 하는데 이번엔 일케 해봄
    private GameObject ButtonIcon;
    private Image Cooltimme_image;
    private TextMeshProUGUI Cooltime_text;
    private float _SkillCoolTime;
    private float _CurrentTime;

    private void Start()
    {
        ButtonIcon = transform.GetChild(0).gameObject;
        Cooltimme_image = transform.GetChild(1).gameObject.GetComponent<Image>();
        Cooltime_text = transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();

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
            if(_SkillCoolTime - _CurrentTime <= 0)
            {
                Managers.Event.SkillAction?.Invoke(ButtonSkillcode);
                _CurrentTime = 0;
            }

         
       
        }
        else
        {
            return;
        }
    }
    private void Update()
    {
        if (_btnskillcode == 0)
        {
            Cooltime_text.text = "";
            Cooltimme_image.fillAmount = 0;
        }
        else
        {
            _CurrentTime += Time.deltaTime;
            //(1 -> 0 (스킬 장전)
            Cooltimme_image.fillAmount = Mathf.Clamp((float)(_SkillCoolTime-_CurrentTime) / _SkillCoolTime, 0, _SkillCoolTime);
            Cooltime_text.text = $"{Mathf.Clamp(_SkillCoolTime - _CurrentTime, 0, _SkillCoolTime):00}";
            if (_SkillCoolTime - _CurrentTime<0)
            {
                Cooltime_text.gameObject.SetActive(false);
            }
            else
            {
                Cooltime_text.gameObject.SetActive(true);
            }
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
                _SkillCoolTime = Managers.Data.SkillDataDict[_btnskillcode].cooltime;
                _CurrentTime = _SkillCoolTime;
            }
            else
            {
                Debug.Log("버그");
            }
        }
        SkillButton[] skillButtons = GameObject.FindObjectsOfType<SkillButton>();
        SkillButton targetbtn = skillButtons.FirstOrDefault(s => s.ButtonSkillcode == this._btnskillcode && s.ButtonSKillType != this.ButtonSKillType&&s.ButtonSkillcode!=0);
        if(targetbtn != null)
        {
            targetbtn.ButtonSkillcode = 0;
        }
    }

}
