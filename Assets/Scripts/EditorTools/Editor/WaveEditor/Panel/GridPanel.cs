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

        RoundData[] rounds = WaveEditor.Rounds;
        
        
        DrawGrid();
    }

    private void DrawGrid()
    {
        if (m_worldGrid != null)
        {
            for (int x = 0; x < m_worldGrid.Grid.GridSize.x; x++)
            {
                for (int y = 0; y < m_worldGrid.Grid.GridSize.y; y++)
                {
                    GridNode node = m_worldGrid.Grid.GetNode(x, y);
                    
                    Vector2 nodeTopLeftScreenSpace = m_camera.WorldToScreenPoint(node.m_worldPosition + new Vector3(-m_worldGrid.Grid.NodeRadius, 0, m_worldGrid.Grid.NodeRadius));
                    Vector2 nodeBottomRightScreenSpace = m_camera.WorldToScreenPoint(node.m_worldPosition + new Vector3(m_worldGrid.Grid.NodeRadius, 0, -m_worldGrid.Grid.NodeRadius));

                    float nodeDiameterScreenSpace = (nodeBottomRightScreenSpace.x - nodeTopLeftScreenSpace.x) * 0.95f;
                    
                    Rect nodeRect = new Rect(nodeTopLeftScreenSpace.x, m_editor.position.height - nodeTopLeftScreenSpace.y, nodeDiameterScreenSpace, nodeDiameterScreenSpace);
                    
                    EditorGUI.DrawRect(nodeRect, Color.cyan);
                }
            }
        }
    }
}
