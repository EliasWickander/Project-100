using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
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
    
    private List<LevelEditorTimelineFrameViewModel> m_framesOrdered = new List<LevelEditorTimelineFrameViewModel>();
    
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
            m_selectedFrame = value;
            OnPropertyChanged(m_selectedFrameProp);

            IsFrameSelected = m_selectedFrame != null;
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

        m_slider.onValueChanged.AddListener(OnHandleMoved);
    }

    [Binding]
    private void OnDisable()
    {
        ResetFrames();
        
        m_slider.onValueChanged.RemoveListener(OnHandleMoved);
    }

    [Binding]
    public void AddFrame()
    {
        if(m_slider == null)
            return;

        LevelEditorTimelineFrameViewModel newFrame = Instantiate(m_timelineFramePrefab, m_entriesContainer);

        newFrame.OnClicked += OnFrameButtonClicked;
        
        newFrame.Init(new Vector2(m_slider.handleRect.position.x, m_entriesContainer.position.y), m_slider.value);
        
        m_framesOrdered.Add(newFrame);

        m_framesOrdered = m_framesOrdered.OrderBy(item => item.TimeStamp).ToList();

        SelectFrame(newFrame);
    }

    [Binding]
    public void RemoveFrame()
    {
        if (m_selectedFrame == null)
            return;
        
        RemoveFrame(m_selectedFrame);
        m_selectedFrame = null;
    }
    
    private void RemoveFrame(LevelEditorTimelineFrameViewModel frame)
    {
        if(!m_framesOrdered.Contains(frame))
            return;
            
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
        if (m_slider.value < m_selectedFrame.TimeStamp - epsilon && selectedFrameIndex > 0)
        {
            LevelEditorTimelineFrameViewModel prevFrame = m_framesOrdered[selectedFrameIndex - 1];
                
            SelectFrame(prevFrame);
            return;
        }

        //Select next frame if value is approximately time stamp of next frame
        if (selectedFrameIndex < m_framesOrdered.Count - 1)
        {
            LevelEditorTimelineFrameViewModel nextFrame = m_framesOrdered[selectedFrameIndex + 1];

            if (Mathf.Abs(nextFrame.TimeStamp - m_slider.value) < epsilon)
            {
                SelectFrame(nextFrame);
                return;
            }
        }
    }

    private void OnFrameButtonClicked(LevelEditorTimelineFrameViewModel frame)
    {
        SelectFrame(frame);
        
        m_slider.SetValue(frame.TimeStamp);
    }
    
    private void SelectFrame(LevelEditorTimelineFrameViewModel frame)
    {
        if(SelectedFrame == frame)
            return;
        
        if(SelectedFrame != null)
            SelectedFrame.Select(false);

        SelectedFrame = frame;
        SelectedFrame.Select(true);
    }
}
