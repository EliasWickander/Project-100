using UnityEngine;

namespace wild
{
    //Menu panel
    public class EditorMenu : IEditorPanel
    {
        private EditorButtonNormal m_saveButton;
        public EditorButtonNormal SaveButton => m_saveButton;
		
        public EditorMenu(WaveEditor editor) : base(editor)
        {
            m_saveButton = new EditorButtonNormal("Save", 0, 0, 100, 20);
        }
		
        public override void Render(Rect rect)
        {
            base.Render(rect);
			
            m_saveButton.Render(rect);
        }
    }
}