using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using Util.UnityMVVM;

[Binding]
public class CustomLevelEntryViewModel : ViewModelMonoBehaviour
{
    [SerializeField] 
    private LoadLevelGameEvent m_loadLevelEvent;
    
    private LevelData m_levelData;

    public LevelData LevelData
    {
        get
        {
            return m_levelData;
        }
        set
        {
            m_levelData = value;
            UpdateVariables();
        }
    }

    private PropertyChangedEventArgs m_nameProp = new PropertyChangedEventArgs(nameof(Name));
    private string m_name;

    [Binding]
    public string Name
    {
        get
        {
            return m_name;
        }
        set
        {
            m_name = value;
            OnPropertyChanged(m_nameProp);
        }
    }

    private PropertyChangedEventArgs m_isHoveredProp = new PropertyChangedEventArgs(nameof(IsHovered));
    private bool m_isHovered = false;

    [Binding]
    public bool IsHovered
    {
        get
        {
            return m_isHovered;
        }
        set
        {
            m_isHovered = value;
            OnPropertyChanged(m_isHoveredProp);
        }
    }

    private void UpdateVariables()
    {
        Name = LevelData != null ? LevelData.m_name : null;
    }
    
    [Binding]
    public void OnLoadButtonPressed()
    {
        if(m_loadLevelEvent != null)
            m_loadLevelEvent.Raise(new LoadLevelEventData() {m_levelData = LevelData});
    }
}
