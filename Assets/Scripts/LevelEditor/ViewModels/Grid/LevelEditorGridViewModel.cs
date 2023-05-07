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
    
    private LevelEditorTileViewModel[,] m_gridTiles;

    public LevelEditorTileViewModel[,] GridTiles => m_gridTiles;

    private LevelEditorTileViewModel[] m_outsideTiles;
    public LevelEditorTileViewModel[] OutsideTiles => m_outsideTiles;
    
    private readonly PropertyChangedEventArgs m_selectedTileProp = new PropertyChangedEventArgs(nameof(SelectedTile));
    private LevelEditorTileViewModel m_selectedTile = null;

    [Binding]
    public LevelEditorTileViewModel SelectedTile
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

        m_gridSpawner.Spawn(s_gridSizeX, s_gridSizeY, out m_gridTiles, out m_outsideTiles, OnTileSpawned);
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
        foreach (LevelEditorTileViewModel tile in m_gridTiles)
        {
            tile.OnClicked -= OnTileClicked;
        }
        
        foreach (LevelEditorTileViewModel tile in m_outsideTiles)
        {
            tile.OnClicked -= OnTileClicked;
        }
    }

    private void OnTileSpawned(LevelEditorTileViewModel tile)
    {
        tile.OnClicked += OnTileClicked;
    }

    public void Reset()
    {
        foreach (LevelEditorTileViewModel gridTile in m_gridTiles)
            gridTile.Reset();

        foreach(LevelEditorTileViewModel outsideTile in m_outsideTiles)
            outsideTile.Reset();
        
        SelectedTile = null;
    }
    
    private void OnUnitSelected(UnitData selectedUnit)
    {
        if(m_selectedTile == null)
            return;
        
        m_selectedTile.AttachUnit(selectedUnit);
    }
    
    private void OnTileClicked(LevelEditorTileViewModel clickedTile)
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
        
        for (int y = 0; y < s_gridSizeY; y++)
        {
            for (int x = 0; x < s_gridSizeX; x++)
            {
                TileState gridTileCache = frameData.m_gridTileStates[x, y];

                LevelEditorTileViewModel gridTile = m_gridTiles[x, y];

                gridTile.LoadFromCache(gridTileCache);
            }
        }

        for(int i = 0; i < m_outsideTiles.Length; i++)
        {
            TileState outsideTileCache = frameData.m_outsideTileStates[i];

            LevelEditorTileViewModel outsideTile = m_outsideTiles[i];
            
            outsideTile.LoadFromCache(outsideTileCache);
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
