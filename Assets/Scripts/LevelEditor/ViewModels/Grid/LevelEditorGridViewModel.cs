using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Util.AdvancedTypes;
using Util.UnityMVVM;

[Binding]
public class LevelEditorGridViewModel : ViewModelMonoBehaviour
{
    public static LevelEditorGridViewModel Instance { get; private set; }

    [SerializeField] 
    private ArenaConfig m_arenaConfig;

    [SerializeField] 
    private LevelEditorGridSpawner m_gridSpawner;

    [SerializeField] 
    private UnitSelectedGameEvent m_unitSelectedEvent;

    public static int s_gridSizeX = 10;
    public static int s_gridSizeY = 10;
    
    private LevelEditorGridTileViewModel[,] m_tiles;

    public LevelEditorGridTileViewModel[,] Tiles => m_tiles;

    private readonly PropertyChangedEventArgs m_selectedTileProp = new PropertyChangedEventArgs(nameof(SelectedTile));
    private LevelEditorGridTileViewModel m_selectedTile = null;

    [Binding]
    public LevelEditorGridTileViewModel SelectedTile
    {
        get
        {
            return m_selectedTile;
        }
        set
        {
            m_selectedTile = value;
            OnPropertyChanged(m_selectedTileProp);
            OnPropertyChanged(m_hasTileSelectedProp);
        }
    }

    private readonly PropertyChangedEventArgs m_hasTileSelectedProp = new PropertyChangedEventArgs(nameof(HasTileSelected));

    [Binding]
    public bool HasTileSelected => SelectedTile != null;

    public LoadFrameGameEvent m_loadFrameEvent;

    private void Awake()
    {
        s_gridSizeX = m_arenaConfig.TilesPerRow;
        s_gridSizeY = m_arenaConfig.TilesPerRow;
        
        Instance = this;

        m_tiles = m_gridSpawner.Spawn(s_gridSizeX, s_gridSizeY, OnTileSpawned);
    }

    private void OnEnable()
    {
        m_loadFrameEvent.RegisterListener(OnFrameLoaded);
        m_unitSelectedEvent.RegisterListener(OnUnitSelected);
    }

    private void OnDisable()
    {
        m_loadFrameEvent.UnregisterListener(OnFrameLoaded);
        m_unitSelectedEvent.UnregisterListener(OnUnitSelected);
    }

    private void OnDestroy()
    {
        foreach (LevelEditorGridTileViewModel tile in m_tiles)
        {
            tile.OnClicked -= OnTileClicked;
        }
    }

    private void OnTileSpawned(LevelEditorGridTileViewModel tile)
    {
        tile.OnClicked += OnTileClicked;
    }
    
    public void Reset()
    {
        foreach (LevelEditorGridTileViewModel tile in m_tiles)
        {
            tile.Reset();
        }

        SelectedTile = null;
    }
    
    private void OnUnitSelected(UnitData selectedUnit)
    {
        if(m_selectedTile == null)
            return;
        
        m_selectedTile.AttachUnit(selectedUnit);
    }
    
    private void OnTileClicked(LevelEditorGridTileViewModel clickedTile)
    {
        if(SelectedTile != null)
            SelectedTile.Select(false);

        if (clickedTile == SelectedTile)
        {
            SelectedTile = null;
            return;
        }

        clickedTile.Select(true);
        SelectedTile = clickedTile;
    }
    
    private void OnFrameLoaded(LevelEditorTimelineFrameViewModel frame)
    {
        TimelineFrameData frameData = frame.Data;
        
        Reset();
        
        for (int y = 0; y < s_gridSizeY + 2; y++)
        {
            for (int x = 0; x < s_gridSizeX + 2; x++)
            {
                GridTileState cache = frameData.m_tileStates[x, y];

                LevelEditorGridTileViewModel tile = m_tiles[x, y];

                tile.LoadFromCache(cache);
            }
        }
    }

    public void OnFrameSelected(FrameSelectedEventData data)
    {
        if (data.m_newFrame == null)
        {
            Reset();
        }
    }
}
