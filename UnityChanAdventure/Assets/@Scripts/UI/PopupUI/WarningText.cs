using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class WarningText : UI_Popup
{
    private bool isInit = false;
    enum Texts
    {
        WarningText_Text
    }
    enum GameObjects
    {
        Warning_Pannel,
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
    public override void Init()
    {
        if (isInit) return;
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        GetText((int)Texts.WarningText_Text).text = "";
        isInit = true;
        StartCoroutine(nameof(Text_Destory_co));
    }
    public void Set_WarningText(string text,Color Textcolor)
    {
        if (!isInit)
        {
            Init();
        }
        GetText((int)Texts.WarningText_Text).color = Textcolor;
        GetText((int)Texts.WarningText_Text).text = $"{text}";
    
    }
    IEnumerator Text_Destory_co()
    {
        yield return new WaitForSeconds(1.5f);
        Managers.UI.ClosePopupUI();
    }

}
