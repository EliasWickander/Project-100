using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Util.UnityMVVM;

[Binding]
public class UnitSelectorViewModel : ViewModelMonoBehaviour
{
    private ObservableList<SelectableUnitViewModel> m_units = new ObservableList<SelectableUnitViewModel>();
    private PropertyChangedEventArgs m_unitsProp = new PropertyChangedEventArgs(nameof(Units));

    [Binding]
    public ObservableList<SelectableUnitViewModel> Units
    {
        get
        {
            return m_units;
        }
        set
        {
            m_units = value;
            OnPropertyChanged(m_unitsProp);
        }
    }

    private void OnEnable()
    {
        Units.Add(new SelectableUnitViewModel());
        Units.Add(new SelectableUnitViewModel());
        Units.Add(new SelectableUnitViewModel());
    }
}
