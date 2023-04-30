using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using Util.UnityMVVM;

[Binding]
public class LevelEditorTimelineViewModel : ViewModelMonoBehaviour
{
    [SerializeField]
    private TimelineSlider m_slider;

    [SerializeField] 
    private LevelEditorTimelineFrameViewModel m_timelineFramePrefab;

    [SerializeField] 
    private RectTransform m_entriesContainer;

    [SerializeField] 
    private SaveFrameGameEvent m_saveFrameEvent;
    
    [SerializeField] 
    private LoadFrameGameEvent m_loadFrameEvent;

    [SerializeField] 
    private FrameSelectedGameEvent m_frameSelectedEvent;

    private List<LevelEditorTimelineFrameViewModel> m_framesOrdered = new List<LevelEditorTimelineFrameViewModel>();
    public List<LevelEditorTimelineFrameViewModel> FramesOrdered => m_framesOrdered;
    
    private PropertyChangedEventArgs m_selectedFrameProp = new PropertyChangedEventArgs(nameof(SelectedFrame));
    private LevelEditorTimelineFrameViewModel m_selectedFrame = null;

    [Binding]
    public LevelEditorTimelineFrameViewModel SelectedFrame
    {
        get
        {
            return m_selectedFrame;
        }
        set
        {
            LevelEditorTimelineFrameViewModel oldFrame = SelectedFrame;
            
            m_selectedFrame = value;
            OnPropertyChanged(m_selectedFrameProp);

            IsFrameSelected = m_selectedFrame != null;
            
            if(m_frameSelectedEvent != null)
                m_frameSelectedEvent.Raise(new FrameSelectedEventData() {m_oldFrame = oldFrame, m_newFrame = value});
        }
    }

    private PropertyChangedEventArgs m_isFrameSelectedProp = new PropertyChangedEventArgs(nameof(IsFrameSelected));
    private bool m_isFrameSelected = false;

    [Binding]
    public bool IsFrameSelected
    {
        get
        {
            return m_isFrameSelected;
        }
        set
        {
            m_isFrameSelected = value;
            OnPropertyChanged(m_isFrameSelectedProp);
        }
    }
    private void OnEnable()
    {
        if (m_slider == null)
        {
            Debug.LogError("Slider is null. Disabling object. Click to locate", gameObject);
            enabled = false;
            return;
        }

        m_slider.OnValueChanged.AddListener(OnHandleMoved);
    }

    [Binding]
    private void OnDisable()
    {
        ResetFrames();
        
        m_slider.OnValueChanged.RemoveListener(OnHandleMoved);
    }

    //Add frame at current timeline selection
    [Binding]
    public void AddFrame()
    {
        if(m_slider == null)
            return;

        float timeStamp = m_slider.Value;
        
        LevelEditorTimelineFrameViewModel addedFrame = AddFrame(timeStamp);

        SelectFrame(addedFrame);
    }

    //Add frame at position
    public LevelEditorTimelineFrameViewModel AddFrame(float timeStamp)
    {
        Vector2 positionAtTimeStamp = m_slider.GetHandlePositionFromTimestamp(timeStamp);
        positionAtTimeStamp.y = m_entriesContainer.position.y;
        
        LevelEditorTimelineFrameViewModel newFrame = Instantiate(m_timelineFramePrefab, m_entriesContainer);

        newFrame.OnClicked += OnFrameButtonClicked;
        
        newFrame.Init(positionAtTimeStamp, timeStamp, m_saveFrameEvent);
        
        m_framesOrdered.Add(newFrame);

        m_framesOrdered = m_framesOrdered.OrderBy(item => item.TimeStamp).ToList();

        return newFrame;
    }

    [Binding]
    public void RemoveSelectedFrame()
    {
        if (SelectedFrame == null)
            return;
        
        RemoveFrame(SelectedFrame);
    }
    
    private void RemoveFrame(LevelEditorTimelineFrameViewModel frame)
    {
        if(frame == null)
            return;
        
        if(!m_framesOrdered.Contains(frame))
            return;

        if (SelectedFrame == frame)
            SelectedFrame = null;
        
        frame.OnClicked -= OnFrameButtonClicked;

        m_framesOrdered.Remove(frame);
        
        Destroy(frame.gameObject);
    }

    [Binding]
    public void ResetFrames()
    {
        List<LevelEditorTimelineFrameViewModel> frames = new List<LevelEditorTimelineFrameViewModel>(m_framesOrdered);
        
        foreach (var frame in frames)
        {
            RemoveFrame(frame);
        }
        
        m_framesOrdered.Clear();
    }
    
    private void OnHandleMoved(float value)
    {
        UpdateFrameSelection();
    }

    private void UpdateFrameSelection()
    {
        if(m_framesOrdered.Count <= 0)
            return;
        
        if (m_selectedFrame == null)
            SelectFrame(m_framesOrdered[0]);

        int selectedFrameIndex = m_framesOrdered.IndexOf(m_selectedFrame);

        float epsilon = 0.01f;
        
        //Select previous frame if value has not yet reached currently selected frame
        if (m_slider.Value < m_selectedFrame.TimeStamp - epsilon && selectedFrameIndex > 0)
        {
            LevelEditorTimelineFrameViewModel prevFrame = m_framesOrdered[selectedFrameIndex - 1];
                
            SelectFrame(prevFrame);
            return;
        }

        //Select next frame if value is approximately time stamp of next frame
        if (selectedFrameIndex < m_framesOrdered.Count - 1)
        {
            LevelEditorTimelineFrameViewModel nextFrame = m_framesOrdered[selectedFrameIndex + 1];

            if (Mathf.Abs(nextFrame.TimeStamp - m_slider.Value) < epsilon)
            {
                SelectFrame(nextFrame);
                return;
            }
        }
    }

    private void OnFrameButtonClicked(LevelEditorTimelineFrameViewModel frame)
    {
        SelectFrame(frame);
        
        m_slider.Set(frame.TimeStamp);
    }
    
    private void SelectFrame(LevelEditorTimelineFrameViewModel frame)
    {
        if(SelectedFrame == frame)
            return;

        if (SelectedFrame != null)
        {
            if(m_saveFrameEvent != null)
                m_saveFrameEvent.Raise(SelectedFrame);
            
            SelectedFrame.Select(false);
        }
        
        SelectedFrame = frame;
        SelectedFrame.Select(true);

        if(m_loadFrameEvent != null)
            m_loadFrameEvent.Raise(SelectedFrame);
    }

    public void LoadLevel(LevelData level)
    {
        ResetFrames();

        if (level.m_frames.Count > 0)
        {
            foreach (TimelineFrameData frameData in level.m_frames)
            {
                LevelEditorTimelineFrameViewModel frame = AddFrame(frameData.m_timeStamp);
            
                frame.Copy(frameData);
            }
            
            SelectFrame(m_framesOrdered[0]);
        }
    }
}
