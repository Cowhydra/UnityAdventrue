using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EventManager
{
    // 이벤트 타입과 해당 이벤트에 대한 리스너 목록을 저장하는 딕셔너리
    private Dictionary<EVENT_TYPE, List<IListener>> Listeners = new Dictionary<EVENT_TYPE, List<IListener>>();

    //Sender와 관련이 없는 이벤트들
    public Action<int> AddItem;
    public Action<int> DifficultyChange;
    public Action<int> GoldChange;
    public Action<int> EquipItemChange;

    public Action BossProgress;

    public Action ExcuteActiveItem;

    public Action GameStateChange;

    // 이벤트 리스너를 추가하는 메서드
    public void AddListener(EVENT_TYPE Event_Type, IListener Listener)
    {

        List<IListener> ListenList = null;

        // 이미 해당 이벤트에 대한 리스너 목록이 있는 경우 기존 ListenList에 새로운 Listner를 추가해준 후 넘어감
        if (Listeners.TryGetValue(Event_Type, out ListenList))
        {
            ListenList.Add(Listener);
            return;
        }
        // 해당 이벤트에 대한 리스너 목록이 없는 경우 새로 생성하고 리스너 추가
        ListenList = new List<IListener>();
        ListenList.Add(Listener);
        Listeners.Add(Event_Type, ListenList);
    }

    // 이벤트를 발송하는 메서드
    public void PostNotification(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {

        List<IListener> ListenList = null;

        // 해당 이벤트에 대한 리스너 목록을 가져옴
        if (!Listeners.TryGetValue(Event_Type, out ListenList))
            return;

        // 각 리스너에게 이벤트를 전달
        for (int i = 0; i < ListenList.Count; i++)
        {
            // 리스너가 null이 아닌 경우에만 이벤트를 호출
            if (!ListenList[i].Equals(null))
                ListenList[i].OnEvent(Event_Type, Sender, Param);
        }
    }
    // 특정 이벤트에 대한 리스너를 제거하는 메서드
    public void RemoveEvent(EVENT_TYPE Event_Type)
    {
        Listeners.Remove(Event_Type);
    }

    // null인 리스너들을 제거하고 유효한 리스너들로만 이루어진 딕셔너리를 생성하는 메서드
    //비용이 많이 들기 떄문에 씬이 넘어가는 상황 등에서만  초기화 할 때 사용
    // 잘 해야함..  씬 넘어갈 때 모든 객체 파괴하면서 Event 없애던가 
    //아니면 이 방식 말고 기존 연동 끊어야함
    public void RemoveRedundancies()
    {
        Dictionary<EVENT_TYPE, List<IListener>> TmpListeners = new Dictionary<EVENT_TYPE, List<IListener>>();
        foreach (KeyValuePair<EVENT_TYPE, List<IListener>> Item in Listeners)
        {
            for (int i = Item.Value.Count - 1; i >= 0; i--)
            {
                // null인 리스너를 제거
                if (Item.Value[i].Equals(null))
                    Item.Value.RemoveAt(i);
            }

            // 유효한 리스너가 있는 경우만 새로운 딕셔너리에 추가
            if (Item.Value.Count > 0)
                TmpListeners.Add(Item.Key, Item.Value);
        }
        // 유효한 리스너들로 이루어진 딕셔너리로 교체
        Listeners = TmpListeners;
    }
    //모든 리스너들을 초기화
    public void ClearEventList()
    {
        Listeners.Clear();
    }
}