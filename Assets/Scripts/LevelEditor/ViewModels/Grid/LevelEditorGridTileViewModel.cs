using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using Util.UnityMVVM;

public class UnitTileState
{
    public string m_unitId;
    public Vector3 m_direction;
}

[Binding]
public class LevelEditorGridTileViewModel : ViewModelMonoBehaviour, IPointerClickHandler
{
    [SerializeField] 
    private UnitDirectionArrowViewModel[] m_directionArrows;
    
    public event Action<LevelEditorGridTileViewModel> OnClicked;

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

    private readonly PropertyChangedEventArgs m_unitProp = new PropertyChangedEventArgs(nameof(Unit));
    private SelectableUnitViewModel m_unit = null;

    [Binding]
    public SelectableUnitViewModel Unit
    {
        get
        {
            return m_unit;
        }
        set
        {
            m_unit = value;
            OnPropertyChanged(m_unitProp);

            m_tileState.m_unitId = m_unit != null ? m_unit.Id : null;
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

    public UnitTileState m_tileState = new UnitTileState();

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

    public void AttachUnit(SelectableUnitViewModel unit)
    {
        if(Unit == unit || unit == null)
            return;
        
        Unit = unit;
        HasUnitAttached = true;
    }

    [Binding]
    public void DetachUnit()
    {
        if(Unit == null)
            return;

        HasUnitAttached = false;
        Unit = null;

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
    
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClicked?.Invoke(this);
    }
}
