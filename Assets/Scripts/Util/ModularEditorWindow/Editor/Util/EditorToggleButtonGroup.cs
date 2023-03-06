using System.Collections.Generic;
using UnityEngine;

namespace wild
{
    //Button group. Acts as a manager for a group of buttons
    public class EditorToggleButtonGroup
    {
        private List<EditorButtonToggle> m_buttons = new List<EditorButtonToggle>();
        public List<EditorButtonToggle> Buttons => m_buttons;

        private EditorButtonToggle m_selectedButton = null;

        private EditorToggleButtonGroup m_parent;
        private EditorToggleButtonGroup m_child;
        
        public EditorToggleButtonGroup(List<EditorButtonToggle> buttons)
        {
            foreach (EditorButtonToggle button in m_buttons)
                 AddButton(button);
        }

        public EditorToggleButtonGroup()
        {
            
        }
        
        public void OnEnter()
        {

        }

        public void OnExit()
        {
            if (m_selectedButton != null)
            {
                m_selectedButton.OnDeselection();
                m_selectedButton = null;
            }
        }

        public void OnDisable()
        {
            foreach (EditorButtonToggle button in m_buttons)
            {
                button.OnSelected -= OnButtonSelected;
                button.OnDeselected -= OnButtonDeselected;
            }
        }
        
        public void Render(Rect panelRect)
        {
            for (int i = 0; i < m_buttons.Count; i++)
            {
                EditorButtonToggle button = m_buttons[i];
                EditorButtonToggle prevButton = i > 0 ? m_buttons[i - 1] : null;

                if (prevButton == null)
                {
                    if (m_parent != null)
                    {
                        button.SetAbsolutePosition(m_parent.m_selectedButton.Rect.center.x, m_parent.m_selectedButton.Rect.yMax);
                    }
                }
                else
                {
                    //Pushes down buttons when needed
                    if (m_child != null && m_child.Buttons.Count > 0 && prevButton == m_selectedButton)
                    {
                        EditorButton childLowestButton = m_child.GetLowestButton();
                        
                        button.SetAbsolutePosition(prevButton.Rect.xMin, childLowestButton.Rect.yMax);
                    }
                    else
                    {
                        button.SetAbsolutePosition(prevButton.Rect.xMin, prevButton.Rect.yMax);   
                    }
                }

                button.Render(panelRect);   
            }
        }
        
        /// <summary>
        /// Add button to include in group
        /// </summary>
        /// <param name="button">Button</param>
        public void AddButton(EditorButtonToggle button)
        {
            if(m_buttons.Contains(button))
                return;
            
            m_buttons.Add(button);
            button.OnSelected += OnButtonSelected;
            button.OnDeselected += OnButtonDeselected;
        }

        /// <summary>
        /// When a button in the group is selected
        /// </summary>
        /// <param name="button">Button</param>
        private void OnButtonSelected(EditorButtonToggle button)
        {
            //When a button is selected, deselect previous one
            if (button != m_selectedButton && m_selectedButton != null)
            {
                m_selectedButton.OnDeselection();
            }
            
            m_selectedButton = button;
        }

        /// <summary>
        /// When a button in the group is deselected
        /// </summary>
        /// <param name="button">Button</param>
        private void OnButtonDeselected(EditorButton button)
        {
            m_selectedButton = null;
        }

        public void AddChild(EditorToggleButtonGroup child)
        {
            m_child = child;
            child.m_parent = this;
        }

        public void RemoveChild(EditorToggleButtonGroup child)
        {
            m_child = null;
            child.m_parent = null;
        }

        /// <summary>
        /// Get button furthest down in group
        /// </summary>
        /// <returns>Lowest button in group</returns>
        public EditorButton GetLowestButton()
        {
            if (m_buttons.Count <= 0)
                return null;

            EditorButton result = m_buttons[0];
            
            foreach (EditorButton button in m_buttons)
            {
                if (button.Rect.y > result.Rect.y)
                    result = button;
            }

            return result;
        }
    }
}