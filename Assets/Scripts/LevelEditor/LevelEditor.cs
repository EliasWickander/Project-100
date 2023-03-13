using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.AdvancedTypes;

public class LevelEditor : MonoBehaviour
{
    public LevelEditorGrid m_grid;

    private void Awake()
    {
        if (m_grid == null)
            m_grid = FindObjectOfType<LevelEditorGrid>();
    }

    private void Update()
    {
        //TODO: Do timeline (slider) based approach for wave management
        if (Input.GetMouseButtonDown(0))
        {
            Camera camera = CameraManager.Instance.CurrentCamera;

            Vector3 mousePosWorld = camera.ScreenToWorldPoint(Input.mousePosition);

            if (m_grid.Grid.IsInGridBounds(mousePosWorld))
            {
                LevelEditorGridNode selectedNode = m_grid.Grid.GetNode(mousePosWorld);
                m_grid.SelectNode(selectedNode);   
            }
            else
            {
                m_grid.SelectNode(null);
            }
        } 
    }
}
