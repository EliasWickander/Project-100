using System;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace wild
{
	
	//Level editor. Acts as a control panel for all panels
	[CreateAssetMenu(fileName = "New Wave Editor", menuName = "Project 100/Level/Wave Editor")]
	public sealed class WaveEditor : EditorWindow
	{
		private LevelEditorMenu m_menuPanel = null;
		private LevelEditorMap m_mapPanel = null;
		private LevelEditorToolbar m_toolbarPanel = null;

		private EditorCamera m_roundManagementCam;
		
		public LevelEditorMenu MenuPanel => m_menuPanel;
		public LevelEditorMap MapPanel => m_mapPanel;
		public LevelEditorToolbar ToolbarPanel => m_toolbarPanel;
		
		private static RoundData[] m_rounds;

		public static RoundData[] Rounds => m_rounds;

		[InitializeOnLoadMethod]
		private static void OnWindowLoad()
		{
			m_rounds = Resources.FindObjectsOfTypeAll<RoundData>();
		}
		
		[MenuItem("Project 100/Editor/Wave Editor")]
		private static void Init()
		{
			WaveEditor editor = (WaveEditor) GetWindow(typeof(WaveEditor));

			editor.autoRepaintOnSceneChange = true;
			editor.Show();
		}

		private void OnEnable()
		{
			m_roundManagementCam = FindObjectOfType<EditorCamera>();
			
			if (m_roundManagementCam == null || m_roundManagementCam.Camera == null)
				throw new Exception("No camera with LevelEditorCamera component found. Map cannot be displayed");

			//Initialization of panels
			m_menuPanel = new LevelEditorMenu(this);
			m_mapPanel = new LevelEditorMap(this, m_roundManagementCam.Camera);
			m_toolbarPanel = new LevelEditorToolbar(this);

			m_menuPanel.SaveButton.OnClick += OnSaveButtonClicked;
			
			m_menuPanel.OnEnable();
			m_toolbarPanel.OnEnable();
			m_mapPanel.OnEnable();
			
			OnLoad();
		}

		private void OnDisable()
		{
			m_menuPanel.SaveButton.OnClick -= OnSaveButtonClicked;
			
			//Disable panels
			m_menuPanel.OnDisable();
			m_toolbarPanel.OnDisable();
			m_mapPanel.OnDisable();
		}

		private void OnGUI()
		{
			UpdateAndRenderPanels();
		}

		private void UpdateAndRenderPanels()
		{
			UpdatePanels();
			RenderPanels();
		}
		
		private void UpdatePanels()
		{
			m_menuPanel.Update();
			m_mapPanel.Update();
			m_toolbarPanel.Update();
		}

		private void RenderPanels()
		{
			//Define rects in which the panels should be rendered
			Rect menuRect = new Rect(0, 0, position.width * 0.7f, position.height * 0.2f);
			Rect mapRect = new Rect(0, position.height * 0.2f, position.width * 0.7f, position.height * 0.8f);

			Rect toolbarRect = new Rect(mapRect.width, position.height, position.width - mapRect.width, position.height * 0.8f);

			toolbarRect.y -= toolbarRect.height;

			//Render panels
			m_menuPanel.Render(menuRect);
			m_mapPanel.Render(mapRect);
			m_toolbarPanel.Render(toolbarRect);
		}

		private void OnSaveButtonClicked(LevelEditorButton button)
		{
			OnSave();
		}
		
		private void OnSave()
		{
			m_menuPanel.OnSave();
			m_mapPanel.OnSave();
            m_toolbarPanel.OnSave();
		}

		private void OnLoad()
		{
			m_menuPanel.OnLoad();
			m_mapPanel.OnLoad();
			m_toolbarPanel.OnLoad();
		}
	}
}
