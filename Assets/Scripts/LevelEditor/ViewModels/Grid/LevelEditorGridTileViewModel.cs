using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using Util.UnityMVVM;

public struct GridTileState
{
    public string m_unitId;
    public Vector2 m_direction;
}

[Binding]
public class LevelEditorGridTileViewModel : ViewModelMonoBehaviour, IPointerClickHandler
{
    [SerializeField] 
    private LevelEditorGridTileItemViewModel m_itemViewModel;
    
    [SerializeField] 
    private UnitDirectionArrowViewModel[] m_directionArrows;
    
    public event Action<LevelEditorGridTileViewModel> OnClicked;

    public Vector2Int GridPos { get; set; }
    
    private readonly PropertyChangedEventArgs m_selectedProp = new PropertyChangedEventArgs(nameof(Selected));
    private bool m_selected = false;

    [Binding]
    public bool Selected
    {
        get
        {
            return m_selected;
        }
        set
        {
            m_selected = value;
            OnPropertyChanged(m_selectedProp);
        }
    }

    private readonly PropertyChangedEventArgs m_hasUnitAttachedProp = new PropertyChangedEventArgs(nameof(HasUnitAttached));

    private bool m_hasUnitAttached;

    [Binding]
    public bool HasUnitAttached
    {
        get
        {
            return m_hasUnitAttached;
        }
        set
        {
            m_hasUnitAttached = value;
            OnPropertyChanged(m_hasUnitAttachedProp);
        }
    }

    private UnitDirectionArrowViewModel m_selectedDirectionArrow;

    public UnitDirectionArrowViewModel SelectedDirectionArrow
    {
        get
        {
            return m_selectedDirectionArrow;
        }
        set
        {
            m_selectedDirectionArrow = value;

            m_tileState.m_direction = m_selectedDirectionArrow != null ? m_selectedDirectionArrow.m_direction : Vector3.zero;
        }
    }

    private PropertyChangedEventArgs m_isOutsideTileProp = new PropertyChangedEventArgs(nameof(IsOutsideTile));
    private bool m_isOutsideTile = false;

    [Binding]
    public bool IsOutsideTile
    {
        get
        {
            return m_isOutsideTile;
        }
        set
        {
            m_isOutsideTile = value;
            OnPropertyChanged(m_isOutsideTileProp);
        }
    }
    
    private GridTileState m_tileState = new GridTileState();

    public GridTileState TileState => m_tileState;

    private void OnEnable()
    {
        foreach (UnitDirectionArrowViewModel arrow in m_directionArrows)
        {
            arrow.OnClickedEvent += OnSelectedDirection;
        }
    }

    private void OnDisable()
    {
        foreach (UnitDirectionArrowViewModel arrow in m_directionArrows)
        {
            arrow.OnClickedEvent -= OnSelectedDirection;
        }
    }

    public void Select(bool isSelected)
    {
        Selected = isSelected;
    }

    public void AttachUnit(UnitData unit)
    {
        if(m_itemViewModel.Unit == unit || unit == null)
            return;
        
        m_itemViewModel.Unit = unit;
        
        HasUnitAttached = true;
        
        m_tileState.m_unitId = unit.Id;
    }

    [Binding]
    public void DetachUnit()
    {
        if(m_itemViewModel.Unit == null)
            return;

        HasUnitAttached = false;
        m_itemViewModel.Unit = null;

        m_tileState.m_unitId = null;
        
        if (SelectedDirectionArrow != null)
        {
            SelectedDirectionArrow.Select(false);
            SelectedDirectionArrow = null;
        }
    }

    private void OnSelectedDirection(UnitDirectionArrowViewModel selectedArrow)
    {
        if (SelectedDirectionArrow != null)
            SelectedDirectionArrow.Select(false);

        if (selectedArrow == SelectedDirectionArrow)
        {
            SelectedDirectionArrow = null;
            return;
        }

        SelectedDirectionArrow = selectedArrow;
        SelectedDirectionArrow.Select(true);
    }

    public void Reset()
    {
        DetachUnit();
        Select(false);
    }

    public void LoadFromCache(GridTileState cache)
    {
        //Attach unit from cache data
        if (!string.IsNullOrEmpty(cache.m_unitId) && LevelEditor.UnitsMap.ContainsKey(cache.m_unitId))
        {
            UnitData cachedUnit = LevelEditor.UnitsMap[cache.m_unitId];
                    
            AttachUnit(cachedUnit);
        }

        //Set direction from cache data
        foreach (UnitDirectionArrowViewModel arrow in m_directionArrows)
        {
            if (arrow.m_direction == cache.m_direction)
            {
                arrow.Select(true);
            }
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClicked?.Invoke(this);
    }
}
