using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace wild
{
    //Control that handles selection by mouse drag
    public class EditorSelector : IEditorControl
    {
        public delegate void OnSelectionStartedDelegate(EditorSelector selector);

        public OnSelectionStartedDelegate OnSelectionStarted;
        
        public delegate void OnSelectionFinishedDelegate(Rect selectionRect);

        public OnSelectionFinishedDelegate OnSelectionFinished;
        
        public delegate void OnSelectingDelegate(Rect selectionRect);

        public OnSelectingDelegate OnSelecting;

        private Vector2 m_startSelectionPos;
        
        private bool m_isDragging = false;

        private Rect m_selectionRect;

        private Vector2 m_mousePointOnMap;
        
        private int m_mouseButton = -1;
        private Color m_selectionColor = Color.red;

        public EditorSelector(IEditorPanel boundsPanel, int mouseButton, Color color) : base(boundsPanel)
        {
            if (mouseButton > 2)
            {
                Debug.LogError("Invalid mouse button " + mouseButton + ". Cannot create selector");
                return;
            }
            
            m_mouseButton = mouseButton;
            m_selectionColor = color;
        }

        public override void Update()
        {
            CheckSelectionEvents();
        }

        public override void Render()
        {
            UpdateSelectionDraw();
        }

        /// <summary>
        /// Update drawing of selection rectangle
        /// </summary>
        private void UpdateSelectionDraw()
        {
            m_mousePointOnMap = HandleUtility.GUIPointToScreenPixelCoordinate(Event.current.mousePosition);

            if (m_boundsPanel.IsPointInPanel(m_mousePointOnMap))
            {
                if(m_isDragging)
                    DrawSelection();   
            }
            else
            {
                //If dragging outside bounds, abort selection
                if (m_isDragging)
                    AbortSelection();
            }
        }
        
        /// <summary>
        /// Update selection start/stop based on input
        /// </summary>
        private void CheckSelectionEvents()
        {
            Event e = Event.current;

            if (Event.current.button == m_mouseButton)
            {
                //On first frame mouse button is held down
                if (e.type == EventType.MouseDown)
                {
                    //Start drawing selection if mouse point is within bounds
                    if (m_boundsPanel.IsPointInPanel(m_mousePointOnMap))
                        StartDrawSelection(Event.current.button);
                }
                //On first frame mouse button is released
                else if (e.type == EventType.MouseUp)
                {
                    //Stop drawing selection
                    if(m_isDragging)
                        StopDrawSelection(true);
                }   
            }
        }
        
        /// <summary>
        /// Start drawing selection
        /// </summary>
        /// <param name="mouseButton">Mouse button pressed</param>
        private void StartDrawSelection(int mouseButton)
        {
            if(m_isDragging)
               StopDrawSelection(false);

            m_startSelectionPos = m_mousePointOnMap;

            m_isDragging = true;

            OnSelectionStarted?.Invoke(this);
        }

        /// <summary>
        /// Stop drawing selection
        /// </summary>
        /// <param name="finishedSuccessfully">If stop was due to successful selection or if it was aborted</param>
        private void StopDrawSelection(bool finishedSuccessfully)
        {
            m_isDragging = false;

            if(finishedSuccessfully)
                OnSelectionFinished?.Invoke(m_selectionRect);
        }
        
        /// <summary>
        /// Draw selection
        /// </summary>
        private void DrawSelection()
        {
            Vector2 delta = m_mousePointOnMap - m_startSelectionPos;

            float rectStartPosX = m_startSelectionPos.x;
            float rectStartPosY = m_boundsPanel.Editor.position.height - m_startSelectionPos.y;

            if (delta.x < 0)
                rectStartPosX += delta.x;
            
            if (delta.y > 0)
                rectStartPosY -= delta.y;

            m_selectionRect = new Rect(rectStartPosX, rectStartPosY, Mathf.Abs(delta.x), Mathf.Abs(delta.y));
            EditorGUI.DrawRect(m_selectionRect, m_selectionColor);
            m_boundsPanel.Editor.Repaint();

            OnSelecting?.Invoke(m_selectionRect);
        }
        
        /// <summary>
        /// Abort selection
        /// </summary>
        private void AbortSelection()
        {
            m_startSelectionPos = Vector2.zero;
            m_isDragging = false;
        }
    }
}