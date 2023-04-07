using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using Util.UnityMVVM;

[Binding]
public class SelectableUnitViewModel : ViewModelMonoBehaviour, IPointerClickHandler
{
    private PropertyChangedEventArgs m_unitDataProp = new PropertyChangedEventArgs(nameof(UnitData));
    private UnitData m_unitData;

    [Binding]
    public UnitData UnitData
    {
        get
        {
            return m_unitData;
        }
        set
        {
            m_unitData = value;
            OnPropertyChanged(m_unitDataProp);
        }
    }

    [SerializeField] 
    private UnitSelectedGameEvent m_unitSelectedEvent;

    public void OnSelection()
    {
        if(m_unitSelectedEvent != null)
            m_unitSelectedEvent.Raise(UnitData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSelection();
    }
}
