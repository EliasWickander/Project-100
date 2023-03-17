using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Util.UnityMVVM;

[Binding]
public class UnitSelectionViewModel : ViewModelMonoBehaviour
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

    private void Awake()
    {
        EnemyData[] enemies = Resources.LoadAll<EnemyData>("Characters/Enemies");

        for (int i = 0; i < enemies.Length; i++)
        {
            EnemyData enemyData = enemies[i];

            SelectableUnitViewModel selectableUnit = new SelectableUnitViewModel();
            selectableUnit.DisplayName = enemyData.m_displayName;
            selectableUnit.Icon = enemyData.m_icon;

            Units.Add(selectableUnit);
        }
    }
}
