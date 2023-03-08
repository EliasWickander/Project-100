using System;
using System.Collections.Generic;
using UnityEngine;

namespace wild
{
    //Level Editor Tool
    public abstract class IEditorTool
    {
        protected WaveEditor m_editor;
        
        private EditorButtonToggle m_button = null;
        protected IEditorVariableGroup m_controls = null;
        protected EditorToolStateMachine m_subToolStateMachine = new EditorToolStateMachine();
        
        //Toggle-button that's connected to this tool
        public EditorButtonToggle Button => m_button;
        
        //Variables that control this behavior
        public IEditorVariableGroup Controls => m_controls;
        
        //State machine that manages sub tools
        public EditorToolStateMachine SubToolStateMachine => m_subToolStateMachine;

        public IEditorTool(WaveEditor editor, EditorButtonToggle button, IEditorVariableGroup controls)
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
            m_subToolStateMachine.Render(m_editor.SettingsPanel.Rect);
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