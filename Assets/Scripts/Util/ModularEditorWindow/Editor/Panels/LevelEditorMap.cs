using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace wild
{
	//Map panel
	public class LevelEditorMap : ILevelEditorPanel
	{
		private Camera m_camera;

		private LevelEditorMapInfo m_mapInfoPanel;

		private LevelEditorCameraController m_cameraController;
		public LevelEditorCameraController CameraController => m_cameraController;

		public LevelEditorMap(WaveEditor editor, Camera camera) : base(editor)
		{
			m_mapInfoPanel = new LevelEditorMapInfo(editor, this);
			m_cameraController = new LevelEditorCameraController(this, camera, true);

			m_camera = camera;
		}
		

		public override void Render(Rect rect)
		{
			base.Render(rect);
			
			//Render camera within rect
			m_camera.pixelRect = new Rect(rect.position.x, 0, rect.width, rect.height);
			m_camera.Render();
			
			//Render map info panel
			Rect infoPanelRect = new Rect(rect.xMin, rect.yMin, rect.width, rect.height * 0.1f);
			m_mapInfoPanel.Render(infoPanelRect);
		}

		public override void Update()
		{
			base.Update();

			//Run map controls unless a tool is currently active
			if (m_editor.ToolbarPanel.CurrentTool == null)
			{
				m_cameraController.Update();
			}
		}
	}
}
