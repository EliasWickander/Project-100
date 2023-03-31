using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.UnityMVVM;

public class LevelSaverViewModel : ViewModelMonoBehaviour
{
    [Binding]
    public LevelEditorTimelineFrameViewModel SelectedFrame { get; set; }
    
    [SerializeField] 
    private LevelEditorGridViewModel m_levelGrid;

    public void SaveFrame()
    {
        if(SelectedFrame == null || m_levelGrid == null)
            return;

        for (int i = 0; i < m_levelGrid.Tiles.Length; i++)
        {
            
        }
    }
}
