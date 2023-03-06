using UnityEngine;

namespace wild
{
    //Menu panel
    public class LevelEditorMenu : ILevelEditorPanel
    {
        private LevelEditorButtonNormal m_saveButton;
        public LevelEditorButtonNormal SaveButton => m_saveButton;
		
        public LevelEditorMenu(WaveEditor editor) : base(editor)
        {
            m_saveButton = new LevelEditorButtonNormal("Save", 0, 0, 100, 20);
        }
		
        public override void Render(Rect rect)
        {
            base.Render(rect);
			
            m_saveButton.Render(rect);
        }
    }
}