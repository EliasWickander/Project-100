using UnityEngine;
using Util.Attributes;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] 
    private SceneTransitionEvent m_sceneTransitionEvent;
    
    [Scene]
    public string m_gameScene;

    public void LoadLevel(LoadLevelEventData loadLevel)
    {
        if(m_sceneTransitionEvent != null)
            m_sceneTransitionEvent.Raise(new SceneTransitionEventData(){m_targetScene = m_gameScene});
    }
}