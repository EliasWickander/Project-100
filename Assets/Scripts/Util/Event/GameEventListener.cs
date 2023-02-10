using UnityEngine;
using UnityEngine.Events;

public abstract class GameEventListener<TType, TEvent> : MonoBehaviour, IGameEventListener<TType> where TEvent : GameEvent<TType>
{
    public TEvent m_event;
    public UnityEvent<TType> m_response;

    private void OnEnable()
    {
        if(m_event != null)
            m_event.RegisterListener(this);
    }

    private void OnDisable()
    {
        if(m_event != null)
            m_event.UnregisterListener(this);
    }

    public void OnEventRaised(TType value)
    {
        m_response.Invoke(value);
    }
}