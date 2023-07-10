using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestInfo : UI_Scene
{
    private int _questid;

    public int QuestID
    {
        get { return _questid; }
        set
        {
            _questid = value;
            Init();
        }
    }

    enum Texts
    {
        QuestLevelInfo_Text,
        QuestTitleInfo_Text,
        QuestTypeInfo_Text,

    }



    public override void Init()
    {
        base.Init();
        Managers.Event.ActiveQuest -= WhenActiveQUest;
        Managers.Event.ActiveQuest += WhenActiveQUest;
        GetComponent<Canvas>().overrideSorting = false;
        gameObject.GetOrAddComponent<GraphicRaycaster>();
        Bind<TextMeshProUGUI>(typeof(Texts));
        GetText((int)Texts.QuestLevelInfo_Text).text = $"제한 레벨 : Lv.{Managers.Data.QuestData[QuestID].LevelRequirement}";
        GetText((int)Texts.QuestTitleInfo_Text).text = $"{Managers.Data.QuestData[QuestID].Name}";
        GetText((int)Texts.QuestTypeInfo_Text).text = $"유형 : {Managers.Data.QuestData[QuestID].QuestType}";

        gameObject.BindEvent((PointerEventData data) => Managers.UI.ShowSceneUI<Quest_TalkingBar>().QuestId=QuestID);
    }
    private void OnDestroy()
    {
        Managers.Event.ActiveQuest -= WhenActiveQUest;
    }
    private void WhenActiveQUest(Quest quest)
    {
        if (quest.UniqueId == _questid)
        {
            Managers.Resource.Destroy(gameObject);
        }
    }

}
