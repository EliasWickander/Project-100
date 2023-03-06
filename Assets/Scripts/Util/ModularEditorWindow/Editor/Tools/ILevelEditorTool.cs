using System;
using System.Collections.Generic;
using UnityEngine;

namespace wild
{
    //Level Editor Tool
    public abstract class ILevelEditorTool
    {
        protected WaveEditor m_editor;
        
        private LevelEditorButtonToggle m_button = null;
        protected ILevelEditorVariableGroup m_controls = null;
        protected LevelEditorToolStateMachine m_subToolStateMachine = new LevelEditorToolStateMachine();
        
        //Toggle-button that's connected to this tool
        public LevelEditorButtonToggle Button => m_button;
        
        //Variables that control this behavior
        public ILevelEditorVariableGroup Controls => m_controls;
        
        //State machine that manages sub tools
        public LevelEditorToolStateMachine SubToolStateMachine => m_subToolStateMachine;

        public ILevelEditorTool(WaveEditor editor, LevelEditorButtonToggle button, ILevelEditorVariableGroup controls)
        {
            m_editor = editor;
            m_button = button;
            m_controls = controls;
        }

        public virtual void OnEnter()
        {
            if (m_controls != null)
            {
                m_controls.OnUpdated += OnControlsUpdated;
            }
            
            m_subToolStateMachine.OnEnter();
        }

        public virtual void Update()
        {
            m_subToolStateMachine.Update();
        }

        public virtual void Render()
        {
            m_subToolStateMachine.Render(m_editor.ToolbarPanel.Rect);
        }

        public virtual void OnExit()
        {
            if (m_controls != null)
            {
                m_controls.OnUpdated -= OnControlsUpdated;
            }
            
            m_subToolStateMachine.OnExit();
        }

        //Called when the entire editor closes down, regardless of if this tool is active or not
        public virtual void OnDisable()
        {
            m_subToolStateMachine.OnDisable();
        }

        public virtual void OnSave()
        {
            m_subToolStateMachine.OnSave();
        }

        public virtual void OnLoad()
        {
            m_subToolStateMachine.OnLoad();
        }
        
        protected virtual void OnControlsUpdated()
        {
            
        }
    }
}