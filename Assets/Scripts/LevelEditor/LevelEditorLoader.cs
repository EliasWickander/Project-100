using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.Attributes;

public class LevelEditorLoader : MonoBehaviour
{
    [Scene] 
    [SerializeField]
    private string m_levelEditorScene;

    [SerializeField] 
    private SceneTransitionEvent m_sceneTransitionEvent;

    public void Load(LoadLevelEditorEventData data)
    {
        LevelEditorContext.s_currentEditedLevel = data.m_levelToLoad;
        
        if(m_sceneTransitionEvent != null)
            m_sceneTransitionEvent.Raise(new SceneTransitionEventData() {m_targetScene = m_levelEditorScene});
    }
}
