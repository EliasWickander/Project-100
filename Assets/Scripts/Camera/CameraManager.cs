using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.AdvancedTypes;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] 
    private Camera m_defaultCamera = null;
    
    private Camera m_currentCamera = null;
    public Camera CurrentCamera => m_currentCamera;

    protected override void OnSingletonAwake()
    {
        base.OnSingletonAwake();
        
        m_currentCamera = m_defaultCamera != null ? m_defaultCamera : Camera.main;
    }
}
