using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyInputListener : MonoBehaviour
{
    public KeyCode m_key;
    
    public UnityEvent OnInvoked;

    private void Update()
    {
        if (Input.GetKeyDown(m_key))
        {
            OnInvoked?.Invoke();
        }
    }
}
