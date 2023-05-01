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
public class EditorLevelsPanelViewModel : ViewModelMonoBehaviour
{
    [SerializeField]
    private Transform m_levelsContainer;

    [SerializeField]
    private EditorLevelEntryViewModel m_levelEntryPrefab;

    private PropertyChangedEventArgs m_levelsProp = new PropertyChangedEventArgs(nameof(Levels));
    private ObservableList<EditorLevelEntryViewModel> m_levels = new ObservableList<EditorLevelEntryViewModel>();

    [Binding]
    public ObservableList<EditorLevelEntryViewModel> Levels
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
                EditorLevelEntryViewModel newLevelEntry = Instantiate(m_levelEntryPrefab, m_levelsContainer);
                newLevelEntry.LevelData = levelData;
                newLevelEntry.LevelPath = level;
                
                newLevelEntry.OnRemovedEvent += RemoveLevelFromDisk;
                Levels.Add(newLevelEntry);
            }
        }
    }

    private void RemoveLevelFromDisk(EditorLevelEntryViewModel entry)
    {
        entry.OnRemovedEvent -= RemoveLevelFromDisk;
        
        RemoveLevel(entry);
        
        RemoveLevelFromDisk(entry.LevelPath);
    }

    private void RemoveLevelFromDisk(string path)
    {
        if(File.Exists(path))
            File.Delete(path);
    }

    private void RemoveLevel(EditorLevelEntryViewModel entry)
    {
        entry.OnRemovedEvent -= RemoveLevelFromDisk;
            
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
