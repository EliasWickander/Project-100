using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEditor;

namespace wild
{
	public class SettingsVariables : ScriptableObject
	{
		[RoundData]
		public RoundData m_selectedRound;
	}
	
	//Toolbar panel
	public sealed class SettingsPanel : IEditorPanel
	{
		private static SettingsVariables Settings => WaveEditor.Settings;

		public event Action<RoundData> onSelectedRoundChanged;

		private RoundData m_selectedRoundLastFrame = null;

		public SettingsPanel(WaveEditor editor) : base(editor)
		{
			WaveEditor.Settings = ScriptableObject.CreateInstance<SettingsVariables>();
		}

		public override void OnEnable()
		{
			base.OnEnable();
		}

		public override void OnDisable()
		{
			base.OnDisable();
		}

		public override void Update()
		{
			if (m_selectedRoundLastFrame != Settings.m_selectedRound)
			{
				onSelectedRoundChanged?.Invoke(Settings.m_selectedRound);
			}

			m_selectedRoundLastFrame = Settings.m_selectedRound;
		}

		public override void Render(Rect rect)
		{
			base.Render(rect);

			GUI.Label(new Rect(rect.x, rect.y, 100, 20), "Settings", EditorStyles.boldLabel);

			RenderVariables(rect);
		}

		private void RenderVariables(Rect rect)
		{
			SerializedObject so = new SerializedObject(Settings);

			rect = new Rect(rect.x, rect.y + 20, 250, 20);
			
			SerializedProperty selectedRound = so.FindProperty(nameof(SettingsVariables.m_selectedRound));

			EditorGUI.PropertyField(rect, selectedRound);
		}

		public override void OnSave()
		{
			base.OnSave();
		}

		public override void OnLoad()
		{
			base.OnLoad();
		}
	}
}
