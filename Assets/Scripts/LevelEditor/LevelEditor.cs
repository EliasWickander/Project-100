using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using Util.AdvancedTypes;
using Util.UnityMVVM;

public class LevelData
{
    public string m_id;
    public string m_name;
    public List<TimelineFrameData> m_frames;
}

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

    private PropertyChangedEventArgs m_levelNameProp = new PropertyChangedEventArgs(nameof(LevelName));
    private string m_levelName;

    [Binding]
    public string LevelName
    {
        get
        {
            return m_levelName;
        }
        set
        {
            m_levelName = value;
            OnPropertyChanged(m_levelNameProp);
        }
    }
    
    private string m_loadedLevelPath;

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
        //Save current frame state first
        if(m_timeline.SelectedFrame != null)
            m_timeline.SelectedFrame.SaveFrame();

        LevelData levelData = new LevelData();
        
        //If not first time loading level, keep
        if (!string.IsNullOrEmpty(m_loadedLevelPath))
        {
            string oldLevelDataJson = File.ReadAllText(m_loadedLevelPath);

            LevelData oldLevelData = JsonConvert.DeserializeObject<LevelData>(oldLevelDataJson);

            if (oldLevelData != null)
            {
                levelData.m_id = oldLevelData.m_id;
            }
        }
        else
        {
            //Create unique level id
            levelData.m_id = Guid.NewGuid().ToString("N");
        }

        levelData.m_name = LevelName;
        
        //Convert all frames to json
        levelData.m_frames = new List<TimelineFrameData>();

        foreach (LevelEditorTimelineFrameViewModel frame in m_timeline.FramesOrdered)
        {
            levelData.m_frames.Add(frame.Data);
        }

        string levelDataJson = JsonConvert.SerializeObject(levelData, Formatting.None,
            new JsonSerializerSettings()
            { 
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        
        File.WriteAllText(Application.streamingAssetsPath + $"/Levels/Level_{levelData.m_id}.json", levelDataJson);
    }
}
