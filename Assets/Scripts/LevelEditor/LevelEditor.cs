using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;
using Util.AdvancedTypes;
using Util.UnityMVVM;

public class LevelData
{
    public string m_id;
    public string m_name;
    public string m_environmentPath;
    public List<TimelineFrameData> m_frames;
}

[Binding]
public class LevelEditor : ViewModelMonoBehaviour
{
    public static LevelEditor Instance { get; private set; }

    [SerializeField] 
    private EditorLevelSavedGameEvent m_editorLevelSavedEvent;

    [SerializeField]
    private EditorLoadLevelGameEvent m_editorLoadLevelEvent;
    
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

    private EnvironmentConfig m_activeEnvironment;
    
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

    private void OnEnable()
    {
        EnvironmentLoader.OnEnvironmentChanged += OnEnvironmentChanged;
    }

    private void OnDisable()
    {
        EnvironmentLoader.OnEnvironmentChanged -= OnEnvironmentChanged;
    }

    private void Start()
    {
        if(LevelEditorContext.s_currentEditedLevel != null)
            LoadLevel(LevelEditorContext.s_currentEditedLevel);
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

    private void OnEnvironmentChanged(EnvironmentConfig environment)
    {
        m_activeEnvironment = environment;
    }
    
    public void LoadLevel(LevelData level)
    {
        if(level == null)
            return;

        LevelName = level.m_name;
        
        if (m_editorLoadLevelEvent != null)
            m_editorLoadLevelEvent.Raise(level);
    }
    public void SaveLevel()
    {
        //Save current frame state first
        if(m_timeline.SelectedFrame != null)
            m_timeline.SelectedFrame.SaveFrame();

        LevelData levelData = new LevelData();
        
        //Keep id if editing existing level, if not make a new id
        levelData.m_id = LevelEditorContext.s_currentEditedLevel != null ? LevelEditorContext.s_currentEditedLevel.m_id : Guid.NewGuid().ToString("N");

        levelData.m_name = LevelName;
        levelData.m_environmentPath = m_activeEnvironment.m_resourcePath;
        
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
        
        if(m_editorLevelSavedEvent != null)
            m_editorLevelSavedEvent.Raise(levelData);
    }
}
