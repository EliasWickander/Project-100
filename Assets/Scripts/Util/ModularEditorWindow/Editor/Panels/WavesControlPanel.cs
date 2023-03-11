using System.Collections.Generic;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEditor;

namespace wild
{
	//Waves control panel
	public sealed class WavesControlPanel : IEditorPanel
	{
		private SettingsVariables m_settings;

		private EditorToggleButtonGroup m_waveButtonGroup = new EditorToggleButtonGroup();

		public WavesControlPanel(WaveEditor editor) : base(editor)
		{
			m_settings = editor.SettingsPanel.GetSettings();
		}

		public override void OnEnable()
		{
			base.OnEnable();

			m_editor.SettingsPanel.onSelectedRoundChanged += OnSelectedRoundChanged;
			
			m_waveButtonGroup.OnEnter();
		}

		public override void OnDisable()
		{
			base.OnDisable();
			
			m_editor.SettingsPanel.onSelectedRoundChanged -= OnSelectedRoundChanged;
			
			m_waveButtonGroup.OnExit();
		}

		public override void Update()
		{
			
		}

		public override void Render(Rect rect)
		{
			base.Render(rect);

			GUI.Label(new Rect(rect.x, rect.y, 100, 20), "Waves", EditorStyles.boldLabel);
			
			m_waveButtonGroup.Render(rect);
		}

		public override void OnSave()
		{
			base.OnSave();
		}

		public override void OnLoad()
		{
			base.OnLoad();
		}
		
		private void OnSelectedRoundChanged(RoundData selectedRound)
		{
			SyncWaveButtons();
		}

		private void SyncWaveButtons()
		{
			RoundData selectedRound = m_settings.m_selectedRound;

			if (selectedRound != null)
			{
				for (int i = 0; i < selectedRound.m_waves.Count; i++)
				{
					string name = $"Wave {i + 1}";

					if (i == 0)
					{
						m_waveButtonGroup.AddButton(new EditorButtonToggle(name, 0, 0.5f, 100, 20));
					}
					else
					{
						EditorButtonToggle prevButton = m_waveButtonGroup.Buttons[i - 1];
						m_waveButtonGroup.AddButton(new EditorButtonToggle(name, 100, 20, EditorButton.Pivot.Left, prevButton, new Vector2(1, 0.5f)));
					}
				}
			}
			else
			{
				m_waveButtonGroup.ClearButtons();
			}
		}
	}
}
