using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent : ScriptableObject
{
    
}
public abstract class GameEvent<T> : GameEvent
{
    protected List<IGameEventListener<T>> m_listeners = new List<IGameEventListener<T>>();

    public virtual void Raise(T value)
    {
        for (int i = 0; i < m_listeners.Count; i++)
            m_listeners[i].OnEventRaised(value);
    }

    public void RegisterListener(IGameEventListener<T> listener)
    {
        if(!m_listeners.Contains(listener))
            m_listeners.Add(listener);
    }

    public void UnregisterListener(IGameEventListener<T> listener)
    {
        if (m_listeners.Contains(listener))
            m_listeners.Remove(listener);
    }
}