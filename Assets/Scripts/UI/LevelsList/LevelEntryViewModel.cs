using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Util.UnityMVVM;

[Binding]
public class LevelEntryViewModel : ViewModel
{
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

    public void Init(LevelData levelData)
    {
        LevelData = levelData;
    }
    
    private void UpdateVariables()
    {
        Name = LevelData != null ? LevelData.m_name : null;
    }
}
