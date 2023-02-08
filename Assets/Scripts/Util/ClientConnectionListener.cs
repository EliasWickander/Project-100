using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class ClientConnectionListener : MonoBehaviour
{
    public UnityEvent OnClientConnectedEvent;
    public UnityEvent OnClientDisconnectedEvent;

    public bool m_singleErrorListener = false;
    public TransportError m_errorTypeToListenFor;
    public UnityEvent OnClientErrorEvent;
    
    private void OnEnable()
    {
        NetworkManagerCustom.OnClientConnectedEvent += OnClientConnected;
        NetworkManagerCustom.OnClientDisconnectedEvent += OnClientDisconnected;
        NetworkManagerCustom.OnClientErrorEvent += OnClientError;
    }

    private void OnDisable()
    {
        NetworkManagerCustom.OnClientConnectedEvent -= OnClientConnected;
        NetworkManagerCustom.OnClientDisconnectedEvent -= OnClientDisconnected;
        NetworkManagerCustom.OnClientErrorEvent -= OnClientError;
    }

    private void OnClientConnected()
    {
        OnClientConnectedEvent?.Invoke();
    }
    
    private void OnClientDisconnected()
    {
        OnClientDisconnectedEvent?.Invoke();
    }
    
    private void OnClientError(TransportError errorType)
    {
        if (m_singleErrorListener)
        {
            if(m_errorTypeToListenFor == errorType)
                OnClientErrorEvent?.Invoke();   
        }
        else
        {
            OnClientErrorEvent?.Invoke();
        }
    }
}
