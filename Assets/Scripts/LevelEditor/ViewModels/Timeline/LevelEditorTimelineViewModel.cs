using System;
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

    private List<float> m_timeStampsUsed = new List<float>();
    
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

    private void Awake()
    {
        if(!m_timeStampsUsed.Contains(m_slider.Value))
            AddFrame();
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
        m_slider.OnValueChanged.RemoveListener(OnHandleMoved);
    }

    //Add frame at current timeline selection
    [Binding]
    public void AddFrame()
    {
        if(m_slider == null)
            return;

        float timeStamp = m_slider.Value;
        
        if(m_timeStampsUsed.Contains(timeStamp))
            return;
        
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
        
        m_timeStampsUsed.Add(timeStamp);
        
        m_framesOrdered.Add(newFrame);
        m_framesOrdered = m_framesOrdered.OrderBy(item => item.TimeStamp).ToList();

        return newFrame;
    }

    [Binding]
    public void RemoveSelectedFrame()
    {
        if (SelectedFrame == null)
            return;
        
        //Cannot remove root frame
        if(SelectedFrame == m_framesOrdered[0])
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
        {
            //Select previous frame
            int selectedFrameIndex = m_framesOrdered.IndexOf(SelectedFrame);
            
            SelectFrame(m_framesOrdered[selectedFrameIndex - 1]);
        }

        frame.OnClicked -= OnFrameButtonClicked;

        m_timeStampsUsed.Remove(frame.TimeStamp);
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

        var firstFrame = m_framesOrdered[0];
        
        //No frame selected if slider handle is before first frame, should never happen though
        if (m_slider.Value < firstFrame.TimeStamp)
        {
            SelectFrame(null);
            return;
        }
        
        var selectedFrame = m_selectedFrame != null ? m_selectedFrame : firstFrame;
        
        int selectedFrameIndex = m_framesOrdered.IndexOf(selectedFrame);
        
        if (m_slider.Value >= selectedFrame.TimeStamp)
        {
            //If selected frame is last frame, keep it selected
            if (selectedFrameIndex >= m_framesOrdered.Count - 1)
                return;
            
            LevelEditorTimelineFrameViewModel nextFrame = m_framesOrdered[selectedFrameIndex + 1];

            //If timeline timestamp still hasn't reached next frame, keep this frame selected
            if (m_slider.Value < nextFrame.TimeStamp)
                return;

            //If reached next frame is the last one in timeline, select it right away
            if (selectedFrameIndex >= m_framesOrdered.Count - 2)
            {
                SelectFrame(nextFrame);
                return;
            }
            
            LevelEditorTimelineFrameViewModel frameAfterNext = m_framesOrdered[selectedFrameIndex + 2];

            //Select reached next frame if timeline timestamp hasn't reached the frame after that one
            if (m_slider.Value < frameAfterNext.TimeStamp)
            {
                SelectFrame(nextFrame);
                return;
            }

            //Find closest frame after currently selected frame that timeline timestamp has reached 
            List<LevelEditorTimelineFrameViewModel> nextFrames = new List<LevelEditorTimelineFrameViewModel>(m_framesOrdered);
            
            for(int i = m_framesOrdered.Count - 1; i >= 0; i--)
            {
                if (i < selectedFrameIndex)
                    break;
                
                nextFrames.Add(m_framesOrdered[i]);
            }
            
            var closestPassedFrame = GetClosestPassedFrame(nextFrames);

            SelectFrame(closestPassedFrame);

        }
        else
        {
            if (selectedFrameIndex > 0)
            {
                LevelEditorTimelineFrameViewModel prevFrame = m_framesOrdered[selectedFrameIndex - 1];

                //If timeline timestamp has reached previous frame, select it right away
                if (m_slider.Value >= prevFrame.TimeStamp)
                {
                    SelectFrame(prevFrame);
                    return;
                }
            }

            //Find closest frame previous to currently selected frame that timeline timestamp has reached 
            List<LevelEditorTimelineFrameViewModel> previousFrames = new List<LevelEditorTimelineFrameViewModel>();

            for(int i = 0; i < m_framesOrdered.Count; i++)
            {
                if (i >= selectedFrameIndex)
                    break;
                
                previousFrames.Add(m_framesOrdered[i]);
            }

            var closestPassedFrame = GetClosestPassedFrame(previousFrames);

            SelectFrame(closestPassedFrame);
        }
    }

    private LevelEditorTimelineFrameViewModel GetClosestPassedFrame(List<LevelEditorTimelineFrameViewModel> frames)
    {
        if (frames == null || frames.Count <= 0)
            return null;
        
        LevelEditorTimelineFrameViewModel closestFrame = frames[0];
        float closestDist = Mathf.Infinity;
        
        foreach (var frame in frames)
        {
            if (frame.TimeStamp <= m_slider.Value)
            {
                float dist = m_slider.Value - frame.TimeStamp;

                if (dist < closestDist)
                {
                    closestFrame = frame;
                    closestDist = dist;
                }
            }
        }

        return closestFrame;
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

        if (SelectedFrame != null)
        {
            SelectedFrame.Select(true);

            if(m_loadFrameEvent != null)
                m_loadFrameEvent.Raise(SelectedFrame);   
        }
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
