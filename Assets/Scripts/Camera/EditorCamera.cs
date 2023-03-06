using System;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace wild
{
	//Editor camera. This is what renders to the level editor map
	public sealed class EditorCamera : MonoBehaviour
	{
		[SerializeField] 
		private Camera m_camera;

		public Camera Camera => m_camera;

		private void OnValidate()
		{
			if(m_camera == null)
				return;

			m_camera.orthographic = true;
		}
	}
}
