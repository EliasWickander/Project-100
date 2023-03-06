using System;
using System.Collections;
using System.Collections.Generic;
using Util.AdvancedTypes;
using UnityEditor;
using UnityEngine;

public class WorldGrid : MonoBehaviour
{
    [SerializeField] 
    private bool m_debug = true;
    
    [SerializeField] 
    private Vector2 m_worldSize = new Vector2(10, 10);
    
    [SerializeField] 
    private float m_nodeRadius = 0.2f;

    private Grid<GridNode> m_grid = null;
    public Grid<GridNode> Grid => m_grid;

    private void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        m_grid = new Grid<GridNode>(transform.position, m_worldSize, m_nodeRadius);
    }
    
    private void OnDrawGizmos()
    {
        if (!EditorApplication.isPlaying)
        {
            //Show preview of grid
            CreateGrid();   
            Gizmos.color = Color.cyan;
        }
        else
        {
            //Show grid that has been created
            Gizmos.color = Color.green;
        }

        for (int x = 0; x < m_grid.GridSize.x; x++)
        {
            for (int y = 0; y < m_grid.GridSize.y; y++)
            {
                GridNode node = m_grid.GetNode(x, y);

                Gizmos.DrawWireCube(node.m_worldPosition, new Vector3(m_grid.NodeDiameter, 0.1f, m_grid.NodeDiameter));
            }
        }
        
        Gizmos.color = Color.white;
    }
}
