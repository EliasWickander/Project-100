using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEditor;

namespace wild
{
	public class EditorCameraController : IEditorControl
    {
        private Camera m_camera;
        
        private float m_moveSpeedMin = 25;
        private float m_moveSpeedMax = 250;
        private float m_moveSpeed;
        public float MoveSpeed => m_moveSpeed;
        
        private bool m_isMoving = false;

        private Vector2 m_lastMousePosOnMap;
        private Vector2 m_mousePointOnMap;
        
        private bool m_inverseMove = false;
		
        //Clamp values for zoom
        private float m_minOrthoSize = 5;
        private float m_maxOrthoSize = 200;
        
        //Speed of zoom
        private float m_zoomStep = 1;

        private float m_currentZoom = 1;
        
        public float CurrentZoom => m_currentZoom;
        
        public EditorCameraController(IEditorPanel boundsPanel, Camera camera, bool inverseMove = false) : base(boundsPanel)
        {
            m_camera = camera;
            m_inverseMove = inverseMove;
        }

        public override void Update()
        {
            m_mousePointOnMap = HandleUtility.GUIPointToScreenPixelCoordinate(Event.current.mousePosition);

            CheckInputEvents();

            m_moveSpeed = Mathf.Lerp(m_moveSpeedMin, m_moveSpeedMax, Mathf.InverseLerp(m_minOrthoSize, m_maxOrthoSize, m_camera.orthographicSize));
            
            if(m_isMoving)
                Move(m_mousePointOnMap);

            m_lastMousePosOnMap = m_mousePointOnMap;
        }
        
        /// <summary>
        /// Update move start/stop events based on input
        /// </summary>
        private void CheckInputEvents()
        {
            Event e = Event.current;
            
            if(!m_boundsPanel.IsPointInPanel(m_mousePointOnMap) && m_isMoving)
                OnMoveStop();

            switch (e.type)
            {
                case EventType.MouseDown:
                {
                    if (e.button == 2)
                    {
                        if (m_boundsPanel.IsPointInPanel(m_mousePointOnMap) && m_isMoving == false)
                            OnMoveStart();
                    }
                    
                    break;
                }
                case EventType.MouseUp:
                {
                    if (e.button == 2)
                    {
                        if(m_isMoving)
                            OnMoveStop();
                    }
                    
                    break;
                }
                case EventType.ScrollWheel:
                {
                    Zoom(e.delta);
                    
                    break;
                }
            }
        }
        
        /// <summary>
        /// Move camera in direction of drag
        /// </summary>
        /// <param name="mousePos">Current mouse position</param>
        private void Move(Vector2 mousePos)
        {
            Vector2 delta = mousePos - m_lastMousePosOnMap;

            Vector3 moveDir = new Vector3(delta.x, 0, delta.y);
            moveDir.Normalize();

            if (m_inverseMove)
                moveDir = -moveDir;
            
            m_camera.transform.position += moveDir * MoveSpeed * 0.01f;
        }
        
        /// <summary>
        /// Zoom camera
        /// </summary>
        /// <param name="delta">Scroll delta</param>
        private void Zoom(Vector2 delta)
        {
            if (m_camera.orthographic == false)
            {
                Debug.LogError("Camera must be set to orthographic for zoom to work");
                return;
            }
            
            delta.y = Mathf.Clamp(delta.y, -m_zoomStep, m_zoomStep);

            float currentSize = m_camera.orthographicSize;

            //Clamp zoom value by min/max size
            m_camera.orthographicSize = Mathf.Clamp(currentSize + delta.y, m_minOrthoSize, m_maxOrthoSize);

            //Round final zoom value by one decimal
            m_currentZoom = (Mathf.Round(m_maxOrthoSize / m_camera.orthographicSize * 10)) / 10;
        }
        
        /// <summary>
        /// Called before first frame of camera movement
        /// </summary>
        private void OnMoveStart()
        {
            m_isMoving = true;
        }

        /// <summary>
        /// Called after last frame of camera movement
        /// </summary>
        private void OnMoveStop()
        {
            m_isMoving = false;
        }
    }
}
