
namespace wild
{
    //Controls for manipulating the editor
    public abstract class ILevelEditorControl
    {
        protected ILevelEditorPanel m_boundsPanel;

        public ILevelEditorControl(ILevelEditorPanel boundsPanel)
        {
            m_boundsPanel = boundsPanel;
        }

        public virtual void Update()
        {

        }

        public virtual void Render()
        {

        }
    }
}