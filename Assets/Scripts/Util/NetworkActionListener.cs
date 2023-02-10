using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class NetworkActionListener : MonoBehaviour
{
    public UnityEvent OnStartHostAttemptEvent;
    public UnityEvent OnStartHostEvent;
    public UnityEvent OnClientConnectionAttemptEvent;
    public UnityEvent OnClientConnectedEvent;
    public UnityEvent OnClientDisconnectedEvent;

    public bool m_singleErrorListener = false;
    public TransportError m_errorTypeToListenFor;
    public UnityEvent OnClientErrorEvent;
    
    private void OnEnable()
    {
        NetworkManagerCustom.OnStartHostAttemptEvent += OnStartHostAttempt;
        NetworkManagerCustom.OnStartHostEvent += OnStartHost;
        NetworkManagerCustom.OnClientConnectionAttemptEvent += OnClientConnectionAttempt;
        NetworkManagerCustom.OnClientConnectedEvent += OnClientConnected;
        NetworkManagerCustom.OnClientDisconnectedEvent += OnClientDisconnected;
        NetworkManagerCustom.OnClientErrorEvent += OnClientError;
    }

    private void OnDisable()
    {
        NetworkManagerCustom.OnStartHostAttemptEvent -= OnStartHostAttempt;
        NetworkManagerCustom.OnStartHostEvent -= OnStartHost;
        NetworkManagerCustom.OnClientConnectionAttemptEvent -= OnClientConnectionAttempt;
        NetworkManagerCustom.OnClientConnectedEvent -= OnClientConnected;
        NetworkManagerCustom.OnClientDisconnectedEvent -= OnClientDisconnected;
        NetworkManagerCustom.OnClientErrorEvent -= OnClientError;
    }

    private void OnStartHostAttempt()
    {
        OnStartHostAttemptEvent?.Invoke();   
    }
    
    private void OnStartHost()
    {
        OnStartHostEvent?.Invoke();
    }
    
    private void OnClientConnectionAttempt()
    {
        OnClientConnectionAttemptEvent?.Invoke();
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
