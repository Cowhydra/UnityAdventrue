using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EventManager
{
    // �̺�Ʈ Ÿ�԰� �ش� �̺�Ʈ�� ���� ������ ����� �����ϴ� ��ųʸ�
    private Dictionary<EVENT_TYPE, List<IListener>> Listeners = new Dictionary<EVENT_TYPE, List<IListener>>();

    //Sender�� ������ ���� �̺�Ʈ��

    public Action<Define.Login_Event_Type> LoginProgess;



    // �̺�Ʈ �����ʸ� �߰��ϴ� �޼���
    public void AddListener(EVENT_TYPE Event_Type, IListener Listener)
    {

        List<IListener> ListenList = null;

        // �̹� �ش� �̺�Ʈ�� ���� ������ ����� �ִ� ��� ���� ListenList�� ���ο� Listner�� �߰����� �� �Ѿ
        if (Listeners.TryGetValue(Event_Type, out ListenList))
        {
            ListenList.Add(Listener);
            return;
        }
        // �ش� �̺�Ʈ�� ���� ������ ����� ���� ��� ���� �����ϰ� ������ �߰�
        ListenList = new List<IListener>();
        ListenList.Add(Listener);
        Listeners.Add(Event_Type, ListenList);
    }

    // �̺�Ʈ�� �߼��ϴ� �޼���
    public void PostNotification(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {

        List<IListener> ListenList = null;

        // �ش� �̺�Ʈ�� ���� ������ ����� ������
        if (!Listeners.TryGetValue(Event_Type, out ListenList))
            return;

        // �� �����ʿ��� �̺�Ʈ�� ����
        for (int i = 0; i < ListenList.Count; i++)
        {
            // �����ʰ� null�� �ƴ� ��쿡�� �̺�Ʈ�� ȣ��
            if (!ListenList[i].Equals(null))
                ListenList[i].OnEvent(Event_Type, Sender, Param);
        }
    }
    // Ư�� �̺�Ʈ�� ���� �����ʸ� �����ϴ� �޼���
    public void RemoveEvent(EVENT_TYPE Event_Type)
    {
        Listeners.Remove(Event_Type);
    }

    // null�� �����ʵ��� �����ϰ� ��ȿ�� �����ʵ�θ� �̷���� ��ųʸ��� �����ϴ� �޼���
    //����� ���� ��� ������ ���� �Ѿ�� ��Ȳ �����  �ʱ�ȭ �� �� ���
    // �� �ؾ���..  �� �Ѿ �� ��� ��ü �ı��ϸ鼭 Event ���ִ��� 
    //�ƴϸ� �� ��� ���� ���� ���� �������
    public void RemoveRedundancies()
    {
        Dictionary<EVENT_TYPE, List<IListener>> TmpListeners = new Dictionary<EVENT_TYPE, List<IListener>>();
        foreach (KeyValuePair<EVENT_TYPE, List<IListener>> Item in Listeners)
        {
            for (int i = Item.Value.Count - 1; i >= 0; i--)
            {
                // null�� �����ʸ� ����
                if (Item.Value[i].Equals(null))
                    Item.Value.RemoveAt(i);
            }

            // ��ȿ�� �����ʰ� �ִ� ��츸 ���ο� ��ųʸ��� �߰�
            if (Item.Value.Count > 0)
                TmpListeners.Add(Item.Key, Item.Value);
        }
        // ��ȿ�� �����ʵ�� �̷���� ��ųʸ��� ��ü
        Listeners = TmpListeners;
    }
    //��� �����ʵ��� �ʱ�ȭ
    public void ClearEventList()
    {
        Listeners.Clear();
    }
}