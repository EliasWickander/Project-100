using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using Util.UnityMVVM;

public class TimelineFrameData
{
    public GridTileState[,] m_cachedTiles;
}

[Binding]
public class LevelEditorTimelineFrameViewModel : ViewModelMonoBehaviour, IPointerClickHandler
{
    private PropertyChangedEventArgs m_positionProp = new PropertyChangedEventArgs(nameof(Position));
    private Vector3 m_position;

    [Binding]
    public Vector3 Position
    {
        get
        {
            return m_position;
        }
        set
        {
            m_position = value;
            OnPropertyChanged(m_positionProp);
        }
    }

    private PropertyChangedEventArgs m_timeStampProp = new PropertyChangedEventArgs(nameof(TimeStamp));
    private float m_timeStamp;

    [Binding]
    public float TimeStamp
    {
        get
        {
            return m_timeStamp;
        }
        set
        {
            m_timeStamp = value;
            OnPropertyChanged(m_timeStampProp);
        }
    }

    private PropertyChangedEventArgs m_isSelectedProp = new PropertyChangedEventArgs(nameof(IsSelected));
    private bool m_isSelected = false;

    [Binding]
    public bool IsSelected
    {
        get
        {
            return m_isSelected;
        }
        set
        {
            m_isSelected = value;
            OnPropertyChanged(m_isSelectedProp);
        }
    }

    private TimelineFrameData m_savedState;
    public TimelineFrameData SavedState => m_savedState;
    
    public event Action<LevelEditorTimelineFrameViewModel> OnClicked;
    
    public void Init(Vector2 position, float timeStamp)
    {
        Position = position;
        TimeStamp = timeStamp;
        
        SetupSavedState();
    }

    public void Select(bool isSelected)
    {
        IsSelected = isSelected;
    }

    private void SetupSavedState()
    {
        TimelineFrameData frameData = new TimelineFrameData();
        
        frameData.m_cachedTiles = new GridTileState[LevelEditorGridViewModel.c_gridSizeX, LevelEditorGridViewModel.c_gridSizeY];

        m_savedState = frameData;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClicked?.Invoke(this);
    }
}
