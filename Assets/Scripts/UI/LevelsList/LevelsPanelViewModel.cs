using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;
using Util.UnityMVVM;

[Binding]
public class LevelsPanelViewModel : ViewModelMonoBehaviour
{
    [SerializeField] 
    private LoadLevelEditorEvent m_loadLevelEditorEvent;
    
    private PropertyChangedEventArgs m_levelsProp = new PropertyChangedEventArgs(nameof(Levels));
    private ObservableList<LevelEntryViewModel> m_levels = new ObservableList<LevelEntryViewModel>();

    [Binding]
    public ObservableList<LevelEntryViewModel> Levels
    {
        get
        {
            return m_levels;
        }
        set
        {
            m_levels = value;
            OnPropertyChanged(m_levelsProp);
        }
    }
    
    private string LevelsPath => Application.streamingAssetsPath + "/Levels";
    
    private void OnEnable()
    {
        if (Directory.Exists(LevelsPath))
        {
            SyncLevelsFromDisk();
        }
    }

    private void SyncLevelsFromDisk()
    {
        ClearLevels();
        
        string[] levels = Directory.GetFiles(LevelsPath, "*.json");

        foreach (string level in levels)
        {
            string levelDataAsJson = File.ReadAllText(level);
            LevelData levelData = JsonConvert.DeserializeObject<LevelData>(levelDataAsJson);

            if (levelData != null)
            {
                LevelEntryViewModel newLevelEntry = new LevelEntryViewModel();

                newLevelEntry.Init(levelData);

                newLevelEntry.OnEntryPressed += OnEntryPressed;
                
                Levels.Add(newLevelEntry);
            }
        }
    }

    private void ClearLevels()
    {
        foreach (var level in Levels)
        {
            level.OnEntryPressed -= OnEntryPressed;
        }
        
        Levels.Clear();
    }

    private void OnEntryPressed(LevelEntryViewModel entry)
    {
        if(entry == null)
            return;

        if(m_loadLevelEditorEvent != null)
            m_loadLevelEditorEvent.Raise(new LoadLevelEditorEventData() {m_levelToLoad = entry.LevelData});
    }
}
