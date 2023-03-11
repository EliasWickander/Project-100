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
		private EditorMenu m_menuPanel = null;
		private EditorMap m_mapPanel = null;
		private SettingsPanel m_settingsPanel = null;
		private GridPanel m_gridPanel;
		private WavesControlPanel m_wavesControlPanel;
		
		private EditorCamera m_roundManagementCam;
		public EditorCamera RoundManagementCam => m_roundManagementCam;
		
		public EditorMenu MenuPanel => m_menuPanel;
		public EditorMap MapPanel => m_mapPanel;
		public SettingsPanel SettingsPanel => m_settingsPanel;
		public WavesControlPanel WavesControlPanel => m_wavesControlPanel;

		public GridPanel GridPanel => m_gridPanel;

		private WorldGrid m_worldGrid;
		public WorldGrid WorldGrid => m_worldGrid;
		
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
				throw new Exception("No camera with EditorCamera component found. Map cannot be displayed");

			m_worldGrid = FindObjectOfType<WorldGrid>();

			if (m_worldGrid == null)
				throw new Exception("No world grid found");
			
			//Initialization of panels
			m_menuPanel = new EditorMenu(this);
			m_mapPanel = new EditorMap(this, m_roundManagementCam.Camera);
			m_settingsPanel = new SettingsPanel(this);
			m_wavesControlPanel = new WavesControlPanel(this);
			m_gridPanel = new GridPanel(this, m_worldGrid);
			
			m_menuPanel.SaveButton.OnClick += OnSaveButtonClicked;
			
			m_menuPanel.OnEnable();
			m_settingsPanel.OnEnable();
			m_mapPanel.OnEnable();
			m_wavesControlPanel.OnEnable();
			m_gridPanel.OnEnable();
			
			OnLoad();
		}

		private void OnDisable()
		{
			m_menuPanel.SaveButton.OnClick -= OnSaveButtonClicked;
			
			//Disable panels
			m_menuPanel.OnDisable();
			m_settingsPanel.OnDisable();
			m_mapPanel.OnDisable();
			m_wavesControlPanel.OnDisable();
			m_gridPanel.OnDisable();
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
			m_settingsPanel.Update();
			m_wavesControlPanel.Update();
			m_gridPanel.Update();
		}

		private void RenderPanels()
		{
			//Define rects in which the panels should be rendered
			Rect menuRect = new Rect(0, 0, position.width * 0.7f, position.height * 0.2f);
			Rect mapRect = new Rect(0, position.height * 0.2f, position.width * 0.7f, position.height * 0.8f);

			float wavesControlPanelHeight = position.height * 0.1f;
			Rect wavesControlPanelRect = new Rect(0, mapRect.y - wavesControlPanelHeight, mapRect.width, wavesControlPanelHeight);
			
			Rect toolbarRect = new Rect(mapRect.width, position.height, position.width - mapRect.width, position.height * 0.8f);
			
			toolbarRect.y -= toolbarRect.height;

			//Render panels
			m_menuPanel.Render(menuRect);
			m_mapPanel.Render(mapRect);
			m_settingsPanel.Render(toolbarRect);
			m_wavesControlPanel.Render(wavesControlPanelRect);
			m_gridPanel.Render(mapRect);
		}

		private void OnSaveButtonClicked(EditorButton button)
		{
			OnSave();
		}
		
		private void OnSave()
		{
			m_menuPanel.OnSave();
			m_mapPanel.OnSave();
            m_settingsPanel.OnSave();
            m_wavesControlPanel.OnSave();
            m_gridPanel.OnSave();
		}

		private void OnLoad()
		{
			m_menuPanel.OnLoad();
			m_mapPanel.OnLoad();
			m_settingsPanel.OnLoad();
			m_wavesControlPanel.OnLoad();
			m_gridPanel.OnLoad();
		}
	}
}
