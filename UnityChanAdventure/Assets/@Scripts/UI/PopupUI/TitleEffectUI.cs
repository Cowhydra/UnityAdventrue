using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleEffectUI : UI_Scene
{
    enum Texts
    {
        Title_Text
    }
    enum GameObjects
    {
        TitlePannel
    }
    private string _title;
    public string Title
    {
        get { return _title; }
        set
        {
            _title = value;
            Init();
        }
    }
    public override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));

        GetObject((int)GameObjects.TitlePannel).GetComponent<Rigidbody2D>().AddForce(12000*Vector2.right);
        GetText((int)Texts.Title_Text).text= _title;
        StartCoroutine(Util.LifeCycle_co(this.gameObject,5));

    }
}
