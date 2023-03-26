using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using Util.UnityMVVM;

[Binding]
public class LevelEditorGridTileViewModel : ViewModelMonoBehaviour, IPointerClickHandler
{
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
    
    public void Select(bool isSelected)
    {
        Selected = isSelected;
    }

    public void AttachUnit(SelectableUnitViewModel unit)
    {
        if(m_unit == unit || unit == null)
            return;
        
        Unit = unit;
        HasUnitAttached = true;
    }

    [Binding]
    public void DetachUnit()
    {
        if(m_unit == null)
            return;

        HasUnitAttached = false;
        Unit = null;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClicked?.Invoke(this);
    }
}
