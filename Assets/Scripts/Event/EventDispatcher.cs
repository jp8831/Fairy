using System;
using System.Collections;
using System.Collections.Generic;

public class EventMapBase<TId, TEvent> where TEvent : Delegate
{
    private Dictionary<TId, TEvent> m_listenerMap;

    public Dictionary<TId, TEvent> ListenerMap
    {
        get
        {
            if (m_listenerMap == null)
            {
                m_listenerMap = new Dictionary<TId, TEvent>();
            }

            return m_listenerMap;
        }
    }
}

public class EventMap<TId> : EventMapBase<TId, Action>
{
    public void AddListener (TId id, Action listener)
    {
        if (ListenerMap.ContainsKey (id))
        {
            ListenerMap[id] += listener;
        }
        else
        {
            ListenerMap.Add(id, listener);
        }
    }

    public void RemoveListener (TId id, Action listener)
    {
        if (ListenerMap.ContainsKey (id))
        {
            ListenerMap[id] -= listener;
        }
    }
}

public class EventMap<TId, TParam1> : EventMapBase<TId, Action<TParam1>>
{
    public void AddListener(TId id, Action<TParam1> listener)
    {
        if (ListenerMap.ContainsKey (id))
        {
            ListenerMap[id] += listener;
        }
        else
        {
            ListenerMap.Add (id, listener);
        }
    }

    public void RemoveListener(TId id, Action<TParam1> listener)
    {
        if (ListenerMap.ContainsKey (id))
        {
            ListenerMap[id] -= listener;
        }
    }
}

public class EventMap<TId, TParam1, TParam2> : EventMapBase<TId, Action<TParam1, TParam2>>
{
    public void AddListener(TId id, Action<TParam1, TParam2> listener)
    {
        if (ListenerMap.ContainsKey (id))
        {
            ListenerMap[id] += listener;
        }
        else
        {
            ListenerMap.Add (id, listener);
        }
    }

    public void RemoveListener(TId id, Action<TParam1, TParam2> listener)
    {
        if (ListenerMap.ContainsKey (id))
        {
            ListenerMap[id] -= listener;
        }
    }
}
