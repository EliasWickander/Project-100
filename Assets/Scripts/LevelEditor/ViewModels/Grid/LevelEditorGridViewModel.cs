using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using Util.AdvancedTypes;
using Util.UnityMVVM;

[Binding]
public class LevelEditorGridViewModel : ViewModelMonoBehaviour
{
    [SerializeField] 
    private UnitSelectedGameEvent m_unitSelectedEvent;

    public const int c_gridSizeX = 9;
    public const int c_gridSizeY = 9;
    
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

    private void Awake()
    {
        SetupGrid();
    }

    private void OnEnable()
    {
        LevelEditor.OnFrameLoaded += OnFrameLoaded;
        m_unitSelectedEvent.RegisterListener(OnUnitSelected);
        SetupTiles();
    }

    private void OnDisable()
    {
        LevelEditor.OnFrameLoaded -= OnFrameLoaded;
        m_unitSelectedEvent.UnregisterListener(OnUnitSelected);
       
        foreach (LevelEditorGridTileViewModel tile in m_tiles)
        {
            tile.OnClicked -= OnTileClicked;
            
            tile.Select(false);
        }
    }

    private void SetupGrid()
    {
        LevelEditorGridTileViewModel[] tiles = GetComponentsInChildren<LevelEditorGridTileViewModel>();

        m_tiles = new LevelEditorGridTileViewModel[c_gridSizeX, c_gridSizeY];

        int count = 0;

        for (int y = 0; y < c_gridSizeY; y++)
        {
            for (int x = 0; x < c_gridSizeX; x++)
            {
                LevelEditorGridTileViewModel tile = tiles[count];

                tile.GridPos = new Vector2Int(x, y);
                
                m_tiles[x, y] = tile;

                count++;
            }
        }
    }
    
    private void SetupTiles()
    {
        foreach (LevelEditorGridTileViewModel tile in m_tiles)
        {
            tile.OnClicked += OnTileClicked;
        }
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
    
    private void OnFrameLoaded(TimelineFrameData frameData)
    {
        Reset();
        
        for (int y = 0; y < c_gridSizeY; y++)
        {
            for (int x = 0; x < c_gridSizeX; x++)
            {
                GridTileState cache = frameData.m_cachedTiles[x, y];

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
