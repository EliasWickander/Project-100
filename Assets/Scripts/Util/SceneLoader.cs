using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util.Attributes;

public class SceneLoader : MonoBehaviour
{
    [Scene]
    public string m_sceneToLoad;
    
    public void Load()
    {
        SceneManager.LoadScene(m_sceneToLoad);
    }
}