using System;
using System.Collections;
using System.Collections.Generic;

public class EventMapBase<TId, TEvent> where TEvent : Delegate
{
    protected Dictionary<TId, TEvent> m_eventMap;

    public Dictionary<TId, TEvent> ListenerMap
    {
        get
        {
            if (m_eventMap == null)
            {
                m_eventMap = new Dictionary<TId, TEvent>();
            }

            return m_eventMap;
        }
    }
}

public class EventMap<TId> : EventMapBase<TId, Action>
{
    public void AddListener (TId id, Action listener)
    {
        Action found;

        if (m_eventMap.TryGetValue (id, out found))
        {
            found += listener;
        }
        else
        {
            m_eventMap.Add(id, listener);
        }
    }

    public void RemoveListener (TId id, Action listener)
    {
        Action found;

        if (ListenerMap.TryGetValue (id, out found))
        {
            found -= listener;
        }
    }
}

public class EventMap<TId, TParam1> : EventMapBase<TId, Action<TParam1>>
{
    public void AddListener(TId id, Action<TParam1> listener)
    {
        Action<TParam1> found;

        if (m_eventMap.TryGetValue(id, out found))
        {
            found += listener;
        }
        else
        {
            m_eventMap.Add(id, listener);
        }
    }

    public void RemoveListener(TId id, Action<TParam1> listener)
    {
        Action<TParam1> found;

        if (ListenerMap.TryGetValue(id, out found))
        {
            found -= listener;
        }
    }
}

public class EventMap<TId, TParam1, TParam2> : EventMapBase<TId, Action<TParam1, TParam2>>
{
    public void AddListener(TId id, Action<TParam1, TParam2> listener)
    {
        Action<TParam1, TParam2> found;

        if (m_eventMap.TryGetValue(id, out found))
        {
            found += listener;
        }
        else
        {
            m_eventMap.Add(id, listener);
        }
    }

    public void RemoveListener(TId id, Action<TParam1, TParam2> listener)
    {
        Action<TParam1, TParam2> found;

        if (ListenerMap.TryGetValue(id, out found))
        {
            found -= listener;
        }
    }
}
