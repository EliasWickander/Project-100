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
    public TileState[,] m_gridTileStates;
    public TileState[] m_outsideTileStates;
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
            m_gridTileStates = new TileState[LevelEditorGridViewModel.s_gridSizeX, LevelEditorGridViewModel.s_gridSizeY],
            m_outsideTileStates = new TileState[(LevelEditorGridViewModel.s_gridSizeX + 1) * 2 + (LevelEditorGridViewModel.s_gridSizeY + 1) * 2]
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
            for (int x = 0; x < LevelEditorGridViewModel.s_gridSizeX; x++)
            {
                for (int y = 0; y < LevelEditorGridViewModel.s_gridSizeY; y++)
                {
                    LevelEditorTileViewModel gridTile = grid.GridTiles[x, y];
                    
                    m_data.m_gridTileStates[x, y] = gridTile.TileState;
                }
            }

            for (int i = 0; i < grid.OutsideTiles.Length; i++)
            {
                LevelEditorTileViewModel outsideTile = grid.OutsideTiles[i];
                
                m_data.m_outsideTileStates[i] = outsideTile.TileState;
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
