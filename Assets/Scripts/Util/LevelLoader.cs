using UnityEngine;
using Util.Attributes;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] 
    private SceneTransitionEvent m_sceneTransitionEvent;
    
    [Scene]
    public string m_gameScene;

    [Scene] 
    public string m_levelEditorScene;

    public void LoadLevel(LoadLevelEventData loadLevel)
    {
        string sceneToLoad = "";
        
        if (loadLevel.m_editMode)
        {
            LevelEditorContext.s_currentEditedLevel = loadLevel.m_levelData;

            sceneToLoad = m_levelEditorScene;
        }
        
        if(m_sceneTransitionEvent != null)
            m_sceneTransitionEvent.Raise(new SceneTransitionEventData(){m_targetScene = sceneToLoad});
    }
}