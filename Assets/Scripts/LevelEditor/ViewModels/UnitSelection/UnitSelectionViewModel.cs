using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Util.UnityMVVM;

[Binding]
public class UnitSelectionViewModel : ViewModelMonoBehaviour
{
    [SerializeField] 
    private SelectableUnitViewModel m_selectableUnitPrefab;

    [SerializeField] 
    private Transform m_contentContainer;
    
    private List<SelectableUnitViewModel> m_units = new List<SelectableUnitViewModel>();

    private void Start()
    {
        LevelEditor levelEditor = LevelEditor.Instance;
        
        for (int i = 0; i < levelEditor.Units.Length; i++)
        {
            UnitData unitData = levelEditor.Units[i];
            
            SelectableUnitViewModel spawnedUnit = Instantiate(m_selectableUnitPrefab, m_contentContainer);

            spawnedUnit.UnitData = unitData;

            m_units.Add(spawnedUnit);
        }
    }
}
