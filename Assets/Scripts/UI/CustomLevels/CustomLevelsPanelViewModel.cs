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
public class CustomLevelsPanelViewModel : ViewModelMonoBehaviour
{
    [SerializeField]
    private Transform m_levelsContainer;

    [SerializeField]
    private CustomLevelEntryViewModel m_levelEntryPrefab;

    private PropertyChangedEventArgs m_levelsProp = new PropertyChangedEventArgs(nameof(Levels));
    private ObservableList<CustomLevelEntryViewModel> m_levels = new ObservableList<CustomLevelEntryViewModel>();

    [Binding]
    public ObservableList<CustomLevelEntryViewModel> Levels
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
                CustomLevelEntryViewModel newLevelEntry = Instantiate(m_levelEntryPrefab, m_levelsContainer);
                newLevelEntry.LevelData = levelData;

                Levels.Add(newLevelEntry);
            }
        }
    }
    
    private void RemoveLevel(CustomLevelEntryViewModel entry)
    {
        Destroy(entry.gameObject);
        m_levels.Remove(entry);
    }
    
    private void ClearLevels()
    {
        for (int i = m_levels.Count - 1; i >= 0; i--)
        {
            RemoveLevel(m_levels[i]);
        }
        
        m_levels.Clear();
    }
}
