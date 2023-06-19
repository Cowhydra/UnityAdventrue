using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WarningText : UI_Popup
{
    private bool isInit = false;
    enum Texts
    {
        WarningText_Text
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
        StartCoroutine(nameof(Text_Destory_co));
    }
    public override void Init()
    {
        if (isInit) return;
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));
        GetText((int)Texts.WarningText_Text).text = "";
        isInit = true;

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
        yield return new WaitForSeconds(2.5f);
        Managers.UI.ClosePopupUI();
    }

}
