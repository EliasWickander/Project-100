using System.Reflection;
using UnityEngine;

namespace wild
{
    //Interface for buttons in the level editor
    public abstract class LevelEditorButton
    {
        //Pivots
        public enum Pivot
        {
            Center,
            Left,
            Right,
            TopLeft,
            TopCenter,
            TopRight,
            BottomLeft,
            BottomCenter,
            BottomRight
        }

        private string m_name;

        private float m_xCoord;
        private float m_yCoord;

        private float m_width;
        private float m_height;
        
        private Rect m_rect;
        public Rect Rect => m_rect;
        
        private LevelEditorButton m_anchorButton;
        private Pivot m_pivot;
        private Vector2 m_anchorPos;

        private bool m_isAnchored = false;

        private Rect m_panelRect;

        protected GUIStyle m_buttonGUIStyle = new GUIStyle();

        /// <summary>
        /// Init of button
        /// </summary>
        /// <param name="name">Button name</param>
        /// <param name="xCoord">X coordinate (relative to panel)</param>
        /// <param name="yCoord">Y coordinate (relative to panel)</param>
        /// <param name="width">Button width</param>
        /// <param name="height">Button height</param>
        public LevelEditorButton(string name, float xCoord, float yCoord, float width, float height)
        {
            m_name = name;
            m_xCoord = xCoord;
            m_yCoord = yCoord;
            m_width = width;
            m_height = height;
            
            m_buttonGUIStyle = GetGUIStyle();
        }

        /// <summary>
        /// Init of button
        /// </summary>
        /// <param name="name">Button name</param>
        /// <param name="width">Button width</param>
        /// <param name="height">Button height</param>
        /// <param name="pivot">Button anchor pivot</param>
        /// <param name="anchor">Other button to anchor to</param>
        /// <param name="anchorPos">Anchor position of other button</param>
        public LevelEditorButton(string name, float width, float height, Pivot pivot, LevelEditorButton anchor, Vector2 anchorPos)
        {
            m_name = name;
            m_width = width;
            m_height = height;
            m_anchorButton = anchor;
            
            Anchor(pivot, anchor, anchorPos);
            
            m_buttonGUIStyle = GetGUIStyle();
        }

        public void Anchor(Pivot pivot, LevelEditorButton anchor, Vector2 anchorPos)
        {
            m_pivot = pivot;
            m_anchorPos = new Vector2(Mathf.Clamp01(anchorPos.x), Mathf.Clamp01(anchorPos.y));

            m_isAnchored = true;
        }
        
        public void SetSize(float width, float height)
        {
            m_width = width;
            m_height = height;
        }

        public void SetAbsolutePosition(float x, float y)
        {
            m_xCoord = Mathf.InverseLerp(m_panelRect.xMin, m_panelRect.xMax, x);
            m_yCoord = Mathf.InverseLerp(m_panelRect.yMin, m_panelRect.yMax, y);
        }
        
        public void SetAbsoluteX(float x)
        {
            m_xCoord = Mathf.InverseLerp(m_panelRect.xMin, m_panelRect.xMax, x);
        }
        
        public void SetAbsoluteY(float y)
        {
            m_yCoord = Mathf.InverseLerp(m_panelRect.yMin, m_panelRect.yMax, y);
        }
        
        public void Render(Rect panelRect)
        {
            RenderButton(panelRect);
        }

        protected virtual GUIStyle GetGUIStyle()
        {
            return GUIStyle.none;
        }
        
        private void RenderButton(Rect panelRect)
        {
            m_panelRect = panelRect;
            if (m_isAnchored)
            {
                m_rect = GetAnchoredRect(m_anchorButton, m_anchorPos);
            }
            else
            {
                m_rect = new Rect(panelRect.xMin + (panelRect.width * m_xCoord), panelRect.yMin + (panelRect.height * m_yCoord), m_width, m_height);
            }
            
            if (GUI.Button(m_rect, m_name, m_buttonGUIStyle))
                OnClicked();
        }
        
        /// <summary>
        /// Get rect anchored to target
        /// </summary>
        /// <param name="anchorButton">Target button</param>
        /// <param name="anchorPos">Position on button to anchor to</param>
        /// <returns>Rect anchored to target button</returns>
        private Rect GetAnchoredRect(LevelEditorButton anchorButton, Vector2 anchorPos)
        {
            float minX = anchorButton.Rect.xMin;
            float maxX = anchorButton.Rect.xMax;
            float minY = anchorButton.Rect.yMin;
            float maxY = anchorButton.Rect.yMax;

            float posX = Mathf.Lerp(minX, maxX, anchorPos.x);
            float posY = Mathf.Lerp(minY, maxY, anchorPos.y);

            Rect anchoredRect = Rect.zero;
            
            switch (m_pivot)
            {
                case Pivot.Center:
                {
                    anchoredRect = new Rect(posX - m_width * 0.5f, posY - m_height * 0.5f, m_width, m_height);
                    break;
                }
                case Pivot.Left:
                {
                    anchoredRect = new Rect(posX, posY - m_height * 0.5f, m_width, m_height);
                    break;
                }
                case Pivot.Right:
                {
                    anchoredRect = new Rect(posX - m_width, posY - m_height * 0.5f, m_width, m_height);
                    break;
                }
                case Pivot.TopLeft:
                {
                    anchoredRect = new Rect(posX, posY, m_width, m_height);
                    break;
                }
                case Pivot.TopCenter:
                {
                    anchoredRect = new Rect(posX - m_width * 0.5f, posY, m_width, m_height);
                    break;
                }
                case Pivot.TopRight:
                {
                    anchoredRect = new Rect(posX - m_width, posY, m_width, m_height);
                    break;
                }
                case Pivot.BottomLeft:
                {
                    anchoredRect = new Rect(posX, posY - m_height, m_width, m_height);
                    break;
                }
                case Pivot.BottomCenter:
                {
                    anchoredRect = new Rect(posX - m_width * 0.5f, posY - m_height, m_width, m_height);
                    break;
                }
                case Pivot.BottomRight:
                {
                    anchoredRect = new Rect(posX - m_width, posY - m_height, m_width, m_height);
                    break;
                }
            }

            return anchoredRect;
        }
                
        protected virtual void OnClicked()
        {
            
        }

        /// <summary>
        /// Create and return color texture
        /// </summary>
        /// <param name="color">Color</param>
        /// <returns>Resulting color texture</returns>
        protected Texture2D GetColorTexture(Color color)
        {
            Texture2D newTex = new Texture2D(4, 4, TextureFormat.RGBA32, 1, false);

            Color[] fillColorArray = newTex.GetPixels();

            for (int i = 0; i < fillColorArray.Length; i++)
            {
                fillColorArray[i] = color;
            }
            
            newTex.SetPixels(fillColorArray);
            newTex.Apply();

            return newTex;
        }
    }
}