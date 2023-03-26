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
    
    private LevelEditorGridTileViewModel[] m_tiles;

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
        m_tiles = GetComponentsInChildren<LevelEditorGridTileViewModel>();
    }

    private void OnEnable()
    {
        m_unitSelectedEvent.RegisterListener(OnUnitSelected);
        SetupTiles();
    }

    private void OnDisable()
    {
        m_unitSelectedEvent.UnregisterListener(OnUnitSelected);
        ResetTiles();
    }

    private void SetupTiles()
    {
        foreach (LevelEditorGridTileViewModel tile in m_tiles)
        {
            tile.OnClicked += OnTileClicked;
        }
    }

    private void ResetTiles()
    {
        foreach (LevelEditorGridTileViewModel tile in m_tiles)
        {
            tile.OnClicked -= OnTileClicked;
            
            tile.Select(false);
        }
    }

    private void OnUnitSelected(SelectableUnitViewModel selectedUnit)
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
}
