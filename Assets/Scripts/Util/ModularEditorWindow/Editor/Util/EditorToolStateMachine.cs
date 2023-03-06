using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace wild
{
    //Tool state machine
    public class EditorToolStateMachine
    {
        public delegate void OnToolActivatedDelegate(IEditorTool tool);
        public OnToolActivatedDelegate OnToolActivatedEvent;

        public delegate void OnToolDeactivatedDelegate(IEditorTool tool);
        public OnToolDeactivatedDelegate OnToolDeactivatedEvent;
        
        private Dictionary<EditorButtonToggle, IEditorTool> m_tools = new Dictionary<EditorButtonToggle, IEditorTool>();

        private IEditorTool m_activeTool = null;
        public IEditorTool ActiveTool => m_activeTool;

        private EditorToggleButtonGroup m_buttonGroup = new EditorToggleButtonGroup();
        public EditorToggleButtonGroup ButtonGroup => m_buttonGroup;
        
        public void OnEnter()
        {
            OnToolActivatedEvent += OnToolActivated;
            OnToolDeactivatedEvent += OnToolDeactivated;
            m_buttonGroup.OnEnter();
        }

        public void Update()
        {
            if(m_activeTool != null)
                m_activeTool.Update();
        }

        public void Render(Rect panelRect)
        {
            m_buttonGroup.Render(panelRect);
            
            if (m_activeTool != null)
            {
                m_activeTool.Render();   
                
                RenderControls(panelRect, m_activeTool);
            }
        }
        
        public void OnExit()
        {
            if(m_activeTool != null)
                m_activeTool.OnExit();
            
            OnToolActivatedEvent -= OnToolActivated;
            OnToolDeactivatedEvent -= OnToolDeactivated;
            m_buttonGroup.OnExit();
        }

        //When the entire level editor closes
        public void OnDisable()
        {
            m_buttonGroup.OnDisable();
            
            foreach (KeyValuePair<EditorButtonToggle, IEditorTool> pair in m_tools)
            {
                pair.Value.OnDisable();
                
                pair.Key.OnSelected -= OnButtonSelected;   
                pair.Key.OnDeselected -= OnButtonDeselected;   
            }
        }
        
        public virtual void OnSave()
        {
            foreach(KeyValuePair<EditorButtonToggle, IEditorTool> pair in m_tools)
                pair.Value.OnSave();
        }

        public virtual void OnLoad()
        {
            foreach(KeyValuePair<EditorButtonToggle, IEditorTool> pair in m_tools)
                pair.Value.OnLoad();
        }

        /// <summary>
        /// Set active tool to run
        /// </summary>
        /// <param name="tool">Tool</param>
        private void SetActiveTool(IEditorTool tool)
        {
            //Exit old active tool
            if (m_activeTool != null)
            {
                m_activeTool.OnExit();
                OnToolDeactivatedEvent?.Invoke(m_activeTool);
            }
                
            m_activeTool = tool;

            //Enter new activate tool
            if (m_activeTool != null)
            {
                m_activeTool.OnEnter();   
                OnToolActivatedEvent?.Invoke(m_activeTool);   
            }

        }
        /// <summary>
        /// Add tool states to manage
        /// </summary>
        /// <param name="tools">Tools</param>
        public void AddToolStates(List<IEditorTool> tools)
        {
            foreach(IEditorTool tool in tools)
                AddToolState(tool);
        }
        
        /// <summary>
        /// Add tool state to manage
        /// </summary>
        /// <param name="tool">Tool</param>
        public void AddToolState(IEditorTool tool)
        { ;
            m_tools.Add(tool.Button, tool);   
            m_buttonGroup.AddButton(tool.Button);
            tool.Button.OnSelected += OnButtonSelected;
            tool.Button.OnDeselected += OnButtonDeselected;
        }

        /// <summary>
        /// Render tool controls
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="tool"></param>
        private void RenderControls(Rect rect, IEditorTool tool)
        {
            if (tool.Controls != null)
            {
                EditorButton lowestSubButton = ButtonGroup.GetLowestButton();

                if (lowestSubButton != null)
                {
                    //Render controls below lowest button in button group
                    Rect controlsRect = new Rect(rect.x, rect.y + (lowestSubButton.Rect.y - rect.y + lowestSubButton.Rect.height), rect.width, rect.height);

                    tool.Controls.Render(controlsRect);		
                }
            }
        }
        
        /// <summary>
        /// When a tool button is selected
        /// </summary>
        /// <param name="button">Button</param>
        private void OnButtonSelected(EditorButtonToggle button)
        {
            if (button == null)
            {
                Debug.LogError("Tried to set to a state that was connected to a null button. This shouldn't be possible. Aborting...");
                return;
            }
            
            if (!m_tools.TryGetValue(button, out IEditorTool newTool))
            {
                Debug.LogError("Tried to set to an invalid state. Aborting.");
                return;   
            }
            
            SetActiveTool(newTool);
        }
        
        /// <summary>
        /// When a tool button is deselected
        /// </summary>
        /// <param name="button">Button</param>
        private void OnButtonDeselected(EditorButtonToggle button)
        {
            SetActiveTool(null);
        }
        
        private void OnToolActivated(IEditorTool tool)
        {
            ButtonGroup.AddChild(tool.SubToolStateMachine.ButtonGroup);
        }
        
        private void OnToolDeactivated(IEditorTool tool)
        {
            ButtonGroup.RemoveChild(tool.SubToolStateMachine.ButtonGroup);
        }
    }
}