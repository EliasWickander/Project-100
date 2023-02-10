using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEventTrigger<TType, TEvent> : MonoBehaviour where TEvent : GameEvent<TType>
{
    public TEvent m_event;
    
    public TType m_value;
    
    public void Trigger()
    {
        if(m_event != null)
            m_event.Raise(m_value);
    }
}
