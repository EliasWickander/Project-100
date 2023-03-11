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
		private SettingsVariables m_variables;

		public event Action<RoundData> onSelectedRoundChanged;

		private RoundData m_selectedRoundLastFrame = null;

		public SettingsPanel(WaveEditor editor) : base(editor)
		{
			m_variables = ScriptableObject.CreateInstance<SettingsVariables>();
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
			if (m_selectedRoundLastFrame != m_variables.m_selectedRound)
			{
				onSelectedRoundChanged?.Invoke(m_variables.m_selectedRound);
			}

			m_selectedRoundLastFrame = m_variables.m_selectedRound;
		}

		public override void Render(Rect rect)
		{
			base.Render(rect);

			GUI.Label(new Rect(rect.x, rect.y, 100, 20), "Settings", EditorStyles.boldLabel);

			RenderVariables(rect);
		}

		private void RenderVariables(Rect rect)
		{
			SerializedObject so = new SerializedObject(m_variables);

			rect = new Rect(rect.x, rect.y + 20, 250, 20);
			
			SerializedProperty selectedRound = so.FindProperty(nameof(SettingsVariables.m_selectedRound));

			EditorGUI.PropertyField(rect, selectedRound);
		}

		public SettingsVariables GetSettings()
		{
			return m_variables;
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
