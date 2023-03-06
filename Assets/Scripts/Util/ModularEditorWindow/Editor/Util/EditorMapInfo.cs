using UnityEditor;
using UnityEngine;

namespace wild
{
    //Map info panel
    public class EditorMapInfo : IEditorPanel
    {
        private EditorMap m_map;
        public EditorMapInfo(WaveEditor editor, EditorMap map) : base(editor)
        {
            m_map = map;
        }

        public override void Render(Rect rect)
        {
            base.Render(rect);

            //Render map info
            GUIStyle textStyle = GetGUIStyle();
			
            GUIContent speedLabel = new GUIContent($"Speed: {m_map.CameraController.MoveSpeed}");
            GUIContent zoomLabel = new GUIContent($"Zoom: {m_map.CameraController.CurrentZoom}x");

            float speedLabelWidth = textStyle.CalcSize(speedLabel).x;
            float zoomLabelWidth = textStyle.CalcSize(zoomLabel).x;
			
            Rect speedLabelRect = new Rect(rect.x, rect.y, speedLabelWidth, rect.height);
            Rect zoomLabelRect = new Rect(rect.xMax - zoomLabelWidth, speedLabelRect.y, zoomLabelWidth, rect.height);

            EditorGUI.LabelField(speedLabelRect, speedLabel, textStyle);
            EditorGUI.LabelField(zoomLabelRect, zoomLabel, textStyle);
        }

        private GUIStyle GetGUIStyle()
        {
            GUIStyle style = new GUIStyle
            {
                fontStyle = FontStyle.Italic,
                normal = {textColor = Color.gray},
                fontSize = 24
            };

            return style;
        }
    }
}