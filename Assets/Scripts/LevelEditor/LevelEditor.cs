using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.AdvancedTypes;
using Util.UnityMVVM;

[Binding]
public class LevelEditor : ViewModelMonoBehaviour
{
    public static LevelEditor Instance { get; private set; }
    public LevelEditorGridViewModel m_grid;
    public LevelEditorTimelineViewModel m_timeline;

    private UnitData[] m_units;
    public UnitData[] Units => m_units;

    private static Dictionary<string, UnitData> m_unitsMap;
    public static Dictionary<string, UnitData> UnitsMap => m_unitsMap;

    private void Awake()
    {
        Instance = this;
        
        if (m_grid == null)
            m_grid = FindObjectOfType<LevelEditorGridViewModel>();

        if (m_timeline)
            m_timeline = FindObjectOfType<LevelEditorTimelineViewModel>();

        m_unitsMap = new Dictionary<string, UnitData>();

        m_units = Resources.LoadAll<UnitData>("Characters/Enemies");
    
        SetupUnitMap();
    }

    private void SetupUnitMap()
    {
        foreach (UnitData unit in m_units)
        {
            if (m_unitsMap.ContainsKey(unit.m_id))
            {
                Debug.LogError("Duplicate unit id:s. Cannot add to map");
                continue;
            }
            
            m_unitsMap.Add(unit.m_id, unit);
        }
    }

    public void SaveLevel()
    {
        // List<TimelineFrameData> 
        // Newtonsoft.Json.JsonConvert.SerializeObject()
    }
}
