using UnityEngine;

namespace wild
{
    //Normal button
    public class EditorButtonNormal : EditorButton
    {
        public delegate void OnClickDelegate(EditorButtonNormal button);

        public OnClickDelegate OnClick;
        
        public EditorButtonNormal(string name, float xCoord, float yCoord, float width, float height) : base(name, xCoord, yCoord, width, height)
        {
        }

        protected override GUIStyle GetGUIStyle()
        {
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.background = GetColorTexture(new Color(0.663f, 0.663f, 0.663f, 1));
            style.active.background = GetColorTexture(new Color(0.514f, 0.514f, 0.514f, 1));
            return style;
        }

        
        protected override void OnClicked()
        {
            base.OnClicked();

            OnClick?.Invoke(this);
        }
    }
}