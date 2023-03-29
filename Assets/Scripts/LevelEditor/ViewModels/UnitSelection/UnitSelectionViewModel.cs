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

    private void Awake()
    {
        EnemyData[] enemies = Resources.LoadAll<EnemyData>("Characters/Enemies");

        for (int i = 0; i < enemies.Length; i++)
        {
            EnemyData enemyData = enemies[i];
            
            SelectableUnitViewModel spawnedUnit = Instantiate(m_selectableUnitPrefab, m_contentContainer);
            spawnedUnit.DisplayName = enemyData.m_displayName;
            spawnedUnit.Icon = enemyData.m_icon;

            m_units.Add(spawnedUnit);
        }
    }
}
