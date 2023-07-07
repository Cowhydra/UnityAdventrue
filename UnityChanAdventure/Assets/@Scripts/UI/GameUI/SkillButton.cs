using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : MonoBehaviour
{
    //이벤트를 쓰지 않고 ㄱㄱ

    [SerializeField] private Define.SkillType ButtonSKillType;
    private GameObject ButtonIcon;
    private int _btnskillcode;

    private void Start()
    {
        ButtonIcon = transform.GetChild(0).gameObject;
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

        }
        else
        {

        }
    }

}
