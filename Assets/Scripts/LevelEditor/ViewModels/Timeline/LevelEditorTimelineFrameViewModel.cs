using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using Util.UnityMVVM;

public class TimelineFrameData
{
    public float m_timeStamp;
    public GridTileState[,] m_tileStates;
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

    private TimelineFrameData m_data;
    public TimelineFrameData Data => m_data;
    
    public event Action<LevelEditorTimelineFrameViewModel> OnClicked;

    private SaveFrameGameEvent m_saveFrameEvent;
    
    public void Init(Vector2 position, float timeStamp, SaveFrameGameEvent saveFrameEvent)
    {
        Position = position;
        TimeStamp = timeStamp;
        
        m_data = new TimelineFrameData()
        {
            m_timeStamp = TimeStamp,
            m_tileStates = new GridTileState[LevelEditorGridViewModel.s_gridSizeX, LevelEditorGridViewModel.s_gridSizeY]
        };
        
        m_saveFrameEvent = saveFrameEvent;

        if(m_saveFrameEvent != null)
            m_saveFrameEvent.RegisterListener(OnFrameSaved);
    }

    private void OnDisable()
    {
        if(m_saveFrameEvent != null)
            m_saveFrameEvent.UnregisterListener(OnFrameSaved);
    }

    public void Select(bool isSelected)
    {
        IsSelected = isSelected;
    }

    private void OnFrameSaved(LevelEditorTimelineFrameViewModel frame)
    {
        if(frame != this)
            return;

        SaveFrame();
    }

    public void SaveFrame()
    {
        m_data.m_timeStamp = TimeStamp;
        
        LevelEditorGridViewModel grid = LevelEditorGridViewModel.Instance;

        if (grid != null)
        {
            foreach (LevelEditorGridTileViewModel tile in grid.Tiles)
            {
                m_data.m_tileStates[tile.GridPos.x, tile.GridPos.y] = tile.TileState;
            }   
        }
    }

    public void Copy(TimelineFrameData data)
    {
        if(data == null)
            return;
        
        m_data = data;
    } 
    
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClicked?.Invoke(this);
    }
}
