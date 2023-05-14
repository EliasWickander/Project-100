using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentLoader : MonoBehaviour
{
    [SerializeField] 
    private EnvironmentConfig m_defaultEnvironment;

    [SerializeField] 
    private EnvironmentChangedGameEvent m_environmentChangedEvent;

    public static event Action<EnvironmentConfig> OnEnvironmentChanged;
    
    private EnvironmentConfig m_activeEnvironmentConfig = null;
    private GameObject m_activeEnvironmentObject = null;
    
    private void Start()
    {
        Load(m_defaultEnvironment);
    }

    public void Load(LoadEnvironmentEventData eventData)
    {
        Load(eventData.m_environmentConfig);    
    }
    
    public void Load(EnvironmentConfig environment)
    {
        if (environment == null)
        {
            Debug.LogError("Failed to load environment. Environment was null");
            return;
        }

        if (environment.EnvironmentPrefab == null)
        {
            Debug.LogError($"Failed to load environment. Environment prefab of environment {environment} was null");
            return;
        }
        
        if (m_activeEnvironmentObject != null)
        {
            Destroy(m_activeEnvironmentObject);
        }
        
        m_activeEnvironmentObject = Instantiate(environment.EnvironmentPrefab);
        m_activeEnvironmentConfig = environment;
        
        if(m_environmentChangedEvent != null)
            m_environmentChangedEvent.Raise(new EnvironmentChangedEventData() {m_environmentConfig = m_activeEnvironmentConfig});
        
        OnEnvironmentChanged?.Invoke(environment);
    }
}
