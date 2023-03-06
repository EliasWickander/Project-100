using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using wild;
using Util.AdvancedTypes;

public class GridPanel : IEditorPanel
{
    private WorldGrid m_worldGrid = null;

    private Camera m_camera;
    
    public GridPanel(WaveEditor editor, WorldGrid worldGrid) : base(editor)
    {
        m_worldGrid = worldGrid;

        m_camera = m_editor.RoundManagementCam.Camera;
    }

    public override void Render(Rect rect)
    {
        base.Render(rect);

        if (m_worldGrid != null)
        {
            for (int x = 0; x < m_worldGrid.Grid.GridSize.x; x++)
            {
                for (int y = 0; y < m_worldGrid.Grid.GridSize.y; y++)
                {
                    GridNode node = m_worldGrid.Grid.GetNode(x, y);

                    Vector2 nodePosScreenSpace = m_camera.WorldToScreenPoint(node.m_worldPosition);
                    Vector3 diameterScreenSpace = m_camera.WorldToScreenPoint(new Vector3(m_worldGrid.Grid.NodeDiameter, 0, m_worldGrid.Grid.NodeDiameter));
                    
                    Rect nodeRect = new Rect(nodePosScreenSpace.x, m_editor.position.height - nodePosScreenSpace.y, 10, 10);
                    
                    EditorGUI.DrawRect(nodeRect, Color.cyan);

                }
            }
        }
    }
}
