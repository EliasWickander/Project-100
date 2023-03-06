using UnityEngine;

namespace wild
{
    //Toggleable button 
    public class LevelEditorButtonToggle : LevelEditorButton
    {
        public delegate void OnSelectedDelegate(LevelEditorButtonToggle button);

        public OnSelectedDelegate OnSelected;

        public delegate void OnDeselectedDelegate(LevelEditorButtonToggle button);

        public OnSelectedDelegate OnDeselected;
        
        private bool m_isSelected = false;
        
        private Texture2D m_defaultTexture;
        private Texture2D m_selectedTexture;
        
        public LevelEditorButtonToggle(string name, float xCoord, float yCoord, float width, float height) : base(name, xCoord, yCoord, width, height)
        {

        }

        public LevelEditorButtonToggle(string name, float width, float height, Pivot pivot, LevelEditorButton anchor, Vector2 anchorPos) : base(name, width, height, pivot, anchor, anchorPos)
        {

        }

        protected override GUIStyle GetGUIStyle()
        {
            m_defaultTexture = GetColorTexture(new Color(0.663f, 0.663f, 0.663f, 1));
            m_selectedTexture = GetColorTexture(new Color(0, 1, 0, 1));
                
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.background = m_defaultTexture;
            style.active.background = GetColorTexture(new Color(0.514f, 0.514f, 0.514f, 1));
            return style;
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            
            //Toggle button
            if (!m_isSelected)
            {
                OnSelection();
            }
            else
            {
                OnDeselection(); 
            }
        }
        
        public void OnSelection()
        {
            m_isSelected = true;
            OnSelected?.Invoke(this);   
            
            m_buttonGUIStyle.normal.background = m_selectedTexture;
        }

        public void OnDeselection()
        {
            m_isSelected = false;
            OnDeselected?.Invoke(this);  
            
            m_buttonGUIStyle.normal.background = m_defaultTexture;
        }
    }
}