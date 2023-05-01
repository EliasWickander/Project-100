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
    private Transform m_levelsContainer;

    [SerializeField]
    private LevelEntryViewModel m_levelEntryPrefab;

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
                LevelEntryViewModel newLevelEntry = Instantiate(m_levelEntryPrefab, m_levelsContainer);
                newLevelEntry.LevelData = levelData;

                Levels.Add(newLevelEntry);
            }
        }
    }

    private void ClearLevels()
    {
        for (int i = m_levels.Count - 1; i >= 0; i--)
        {
            Destroy(m_levels[i].gameObject);
            m_levels.RemoveAt(i);
        }
        
        m_levels.Clear();
    }
}
