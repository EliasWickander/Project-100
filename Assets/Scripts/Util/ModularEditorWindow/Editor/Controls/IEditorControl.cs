
namespace wild
{
    //Controls for manipulating the editor
    public abstract class IEditorControl
    {
        protected IEditorPanel m_boundsPanel;

        public IEditorControl(IEditorPanel boundsPanel)
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