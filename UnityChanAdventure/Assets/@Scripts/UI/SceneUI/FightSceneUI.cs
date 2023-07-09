using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FightSceneUI : UI_Scene
{
    private int startTime = 5;
    private bool isStated ;
    private float _totaltime = 120.0f;
    private int currentGold;
    enum GameObjects
    {
        TimeLine,
        ResultPannel,
        StartPannel
    }
    enum Texts
    {
        TimeLine_Text,
        ResultTitle_Text,
        ResultContets_Text_Exp,
        ResultContets_Text_DefeatMonster,
        ResultContets_Text_Gold,
        StartPannel_Time_Text,
        StartPannel_Clcik_Text,

    }
    enum Buttons
    {
        Result_Quit_Button
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        currentGold = Managers.Game.Gold;
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));
        InitText();

        GetButton((int)Buttons.Result_Quit_Button).gameObject
            .BindEvent((PointerEventData data) => Managers.Scene.LoadScene(Define.Scene.TownScene));
        GetObject((int)GameObjects.StartPannel).BindEvent
            ((PointerEventData data) => GoStart());
        Time.timeScale = 0;
        InitGameObject();
    }
    private void InitText()
    {
        GetText((int)Texts.StartPannel_Time_Text).gameObject.SetActive(false);
    }
    private void InitGameObject()
    {
        GetObject((int)GameObjects.ResultPannel).SetActive(false);
        GetObject((int)GameObjects.TimeLine).SetActive(false);
    }

    private void GoStart()
    {
        Time.timeScale = 1;
        if (isStated) return;
        GetText((int)Texts.StartPannel_Clcik_Text).gameObject.SetActive(false);
        GetObject((int)GameObjects.StartPannel).GetComponent<Image>().raycastTarget = false;
        GetText((int)Texts.StartPannel_Time_Text).gameObject.SetActive(true);
        StartCoroutine(nameof(TimeCount));
    }
    private IEnumerator TimeCount()
    {

        while (startTime!=0)
        {
            yield return new WaitForSecondsRealtime(1);
            GetText((int)Texts.StartPannel_Time_Text).text = $"{startTime}";
            startTime--;
        }
        GetObject((int)GameObjects.StartPannel).SetActive(false);
        StartCoroutine(nameof(GameTimeCount));
        FindAnyObjectByType<FightScene_MonsterSpawn>().MonsterSpawnStart();
        //½ºÆù ½ºÅ¸Æ® 
    }

    private IEnumerator GameTimeCount()
    {
        GetObject((int)GameObjects.TimeLine).SetActive(true);
        while (_totaltime!=0)
        {
            _totaltime = Mathf.Clamp(_totaltime - Time.deltaTime, 0, 120.0f);
            GetText((int)Texts.TimeLine_Text).text = $"{_totaltime/60:00} : {_totaltime%60:00}";
            yield return null;
        }
        FindAnyObjectByType<FightScene_MonsterSpawn>().ClearMonster();
        Time.timeScale = 0.5f;
        GetObject((int)GameObjects.TimeLine).SetActive(false);
        GetObject((int)GameObjects.ResultPannel).SetActive(true);
        GetComponent<Canvas>().sortingOrder = 9999;
        SetResult_Text();

    }
    private void SetResult_Text()
    {
        GetText((int)Texts.ResultContets_Text_DefeatMonster).text = $"";
        GetText((int)Texts.ResultContets_Text_Gold).text = $"È¹µæÇÑ °ñµå :{Managers.Game.Gold- currentGold} ";
        GetText((int)Texts.ResultContets_Text_Exp).text = $"";

    }
}
