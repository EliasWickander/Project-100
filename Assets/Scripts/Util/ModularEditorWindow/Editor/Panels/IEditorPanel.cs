using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace wild
{

	//Panel (designated part of the editor window)
	public abstract class IEditorPanel
	{
		protected Rect m_rect;
		public Rect Rect => m_rect;

		protected WaveEditor m_editor;
		public WaveEditor Editor => m_editor;

		public IEditorPanel(WaveEditor editor)
		{
			m_editor = editor;
		}
		
		//Called on window open
		public virtual void OnEnable()
		{
			
		}

		//Call on window closed
		public virtual void OnDisable()
		{

		}

		//Called every frame this is rendered
		public virtual void Update()
		{
			
		}

		//Called after update
		public virtual void Render(Rect rect)
		{
			m_rect = rect;
		}

		//Called when save command is triggered
		public virtual void OnSave()
		{
            
		}

		//Called when load command is triggered
		public virtual void OnLoad()
		{
            
		}
		
		/// <summary>
		/// Is point inside of this panel
		/// </summary>
		/// <param name="point">Point to check</param>
		/// <returns>True/False</returns>
		public bool IsPointInPanel(Vector2 point)
		{
			return point.x > 0 && point.x <= Rect.width && point.y > 0 && point.y <= Rect.height;
		}
	}
}
