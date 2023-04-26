using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using Util.UnityMVVM;

[Binding]
public class LevelsPanelViewModel : ViewModelMonoBehaviour
{
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
        Levels.Clear();
        
        string[] levels = Directory.GetFiles(LevelsPath, "*.json");

        foreach (string level in levels)
        {
            string levelDataAsJson = File.ReadAllText(level);
            LevelData levelData = JsonConvert.DeserializeObject<LevelData>(levelDataAsJson);

            if (levelData != null)
            {
                LevelEntryViewModel newLevelEntry = new LevelEntryViewModel();

                newLevelEntry.Init(levelData);
                
                Levels.Add(newLevelEntry);
            }
        }
    }
}
