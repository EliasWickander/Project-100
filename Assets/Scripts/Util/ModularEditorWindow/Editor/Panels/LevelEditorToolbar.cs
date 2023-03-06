using System.Collections.Generic;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEditor;

namespace wild
{
	//Toolbar panel
	public sealed class LevelEditorToolbar : ILevelEditorPanel
	{
		private LevelEditorToolStateMachine m_toolStateMachine = new LevelEditorToolStateMachine();

		public ILevelEditorTool CurrentTool => m_toolStateMachine.ActiveTool;

		public LevelEditorToolbar(WaveEditor editor) : base(editor)
		{
			// LevelEditorGridTool gridTool = new LevelEditorGridTool(
			// 	editor,
			// 	new LevelEditorButtonToggle("Grid", 0, 0, 100, 20),
			// 	null);
			//
			// m_toolStateMachine.AddToolStates(new List<ILevelEditorTool>() {gridTool});
		}

		public override void OnEnable()
		{
			base.OnEnable();

			m_toolStateMachine.OnEnter();
		}

		public override void OnDisable()
		{
			base.OnDisable();
			
			m_toolStateMachine.OnDisable();
		}

		public override void Update()
		{
			m_toolStateMachine.Update();
		}

		public override void Render(Rect rect)
		{
			base.Render(rect);

			GUI.Label(new Rect(rect.x, rect.y, 100, 20), "Tools", EditorStyles.boldLabel);
			
			m_toolStateMachine.Render(rect);
		}

		public override void OnSave()
		{
			base.OnSave();
			
			m_toolStateMachine.OnSave();
		}

		public override void OnLoad()
		{
			base.OnLoad();
			
			m_toolStateMachine.OnLoad();
		}
	}
}
