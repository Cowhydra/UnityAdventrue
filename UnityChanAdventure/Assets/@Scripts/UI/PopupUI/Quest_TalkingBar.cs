using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Quest_TalkingBar : UI_Scene
{
    //느낌표 같은 UI 추가하자
    private int _quesid;
    private bool isinit = false;
    [SerializeField]
    private int talkingcount = 3;
    public int QuestId
    {
        get { return _quesid; }
        set 
        { 
            _quesid = value;
            if (!isinit)
            {
                Init();
              
            }
        }
    }
    enum Texts
    {
        TalkingText,
        TitleText,
    }
    enum buttons
    {
        CancelButton,
        YesButton,
    }
    enum GameObjects
    {
        BottomPannel,
        ExpReward,
        DiaReward,
        ItemReward,

    }
    public override void Init()
    {
        if (!isinit)
        {
            base.Init();
            GetComponent<Canvas>().sortingOrder = (int)Define.SortingOrder.QuestInfo + 2;
            Bind<GameObject>(typeof(GameObjects));
            Bind<TextMeshProUGUI>(typeof(Texts));
            Bind<Button>(typeof(buttons));

            GetButton((int)buttons.YesButton).gameObject
                .BindEvent((PointerEventData data) => SetQuest());
            GetButton((int)buttons.CancelButton).gameObject
                .BindEvent((PointerEventData data) => Managers.Resource.Destroy(gameObject));
            GetObject((int)GameObjects.BottomPannel).SetActive(true);
            GetText((int)Texts.TitleText).text = Managers.Data.QuestData[QuestId].Name;
            GetText((int)Texts.TalkingText).text =$"{Managers.Data.QuestData[QuestId].script1}\n{Managers.Data.QuestData[QuestId].script2}\n{Managers.Data.QuestData[QuestId].script3}" ;

            if (Managers.Data.QuestData[QuestId].DiaReward == 0)
            {
                GetObject((int)GameObjects.DiaReward).SetActive(false);
            }
            if (Managers.Data.QuestData[QuestId].ExperienceReward == 0)
            {
                GetObject((int)GameObjects.ExpReward).SetActive(false);
            }
            if (Managers.Data.QuestData[QuestId].itemReward == 0)
            {
                GetObject((int)GameObjects.ItemReward).SetActive(false);
            }


            isinit = true;
        }
        return;
    }
    private void SetQuest()
    {
        if (Managers.Data.QuestData[QuestId].QuestType == Define.QuestType.DefeatEnemy)
        {
            DefeatEnemiesQuest _myquest = new DefeatEnemiesQuest(QuestId, Managers.Data.QuestData[QuestId].Name,
             Managers.Data.QuestData[QuestId].LevelRequirement, Managers.Data.QuestData[QuestId].ExperienceReward
             , Managers.Data.QuestData[QuestId].DiaReward, Managers.Data.QuestData[QuestId].itemReward,
             Managers.Data.QuestData[QuestId].enemyToTargetCode, Managers.Data.QuestData[QuestId].Amount
             ) ;
            Managers.Quest.StartQuest(_myquest);
           
        }
        else
        {
            CollectItemQuest _myquest = new CollectItemQuest(QuestId, Managers.Data.QuestData[QuestId].Name
                , Managers.Data.QuestData[QuestId].LevelRequirement, Managers.Data.QuestData[QuestId].ExperienceReward
             , Managers.Data.QuestData[QuestId].DiaReward, Managers.Data.QuestData[QuestId].itemReward,
             Managers.Data.QuestData[QuestId].objectItemCode, Managers.Data.QuestData[QuestId].Amount
                );
            Managers.Quest.StartQuest(_myquest);
        }
        Managers.Data.QuestData[QuestId].State = Define.QuestState.Active;

        Managers.Resource.Destroy(gameObject);
        Debug.Log("퀘스트 시작 UI 만들기");

    }


}
