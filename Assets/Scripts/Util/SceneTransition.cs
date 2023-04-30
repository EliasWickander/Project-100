using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util.AdvancedTypes;

public class SceneTransition : Singleton<SceneTransition>
{
    public void Transition(SceneTransitionEventData sceneTransitionData)
    {
        SceneManager.LoadScene(sceneTransitionData.m_targetScene);
    }
}
