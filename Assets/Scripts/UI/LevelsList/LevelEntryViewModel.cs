using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using Util.UnityMVVM;

[Binding]
public class LevelEntryViewModel : ViewModelMonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] 
    private LoadLevelEditorEvent m_loadLevelEditorEvent;
    
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

    private PropertyChangedEventArgs m_levelPathProp = new PropertyChangedEventArgs(nameof(LevelPath));
    private string m_levelPath;

    [Binding]
    public string LevelPath
    {
        get
        {
            return m_levelPath;
        }
        set
        {
            m_levelPath = value;
            OnPropertyChanged(m_levelPath);
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

    public event Action<LevelEntryViewModel> OnRemovedEvent;
    
    private void UpdateVariables()
    {
        Name = LevelData != null ? LevelData.m_name : null;
    }
    
    [Binding]
    public void OnLoadButtonPressed()
    {
        if(m_loadLevelEditorEvent != null)
            m_loadLevelEditorEvent.Raise(new LoadLevelEditorEventData() {m_levelToLoad = LevelData});
    }

    [Binding]
    public void OnRemoveButtonPressed()
    {
        OnRemovedEvent?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsHovered = false;
    }
}
