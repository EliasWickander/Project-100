using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Util.AdvancedTypes;

public class LevelEditorGridNode : GridNode
{
    public bool m_selected;
    public MeshRenderer m_visualRenderer;
}

public class LevelEditorGrid : MonoBehaviour
{
    [SerializeField] 
    private bool m_debug = true;

    [SerializeField]
    private Material m_defaultMaterial;
    [SerializeField] 
    private Material m_selectedMaterial;
    
    [SerializeField] 
    private Vector2 m_worldSize = new Vector2(10, 10);
    
    [SerializeField] 
    private float m_nodeRadius = 0.2f;

    private Grid<LevelEditorGridNode> m_grid = null;
    public Grid<LevelEditorGridNode> Grid => m_grid;

    private LevelEditorGridNode m_selectedNode = null;
    public LevelEditorGridNode SelectedNode { get; private set; }

    private void Start()
    {
        CreateGrid(true);
    }

    private void CreateGrid(bool createVisual)
    {
        m_grid = new Grid<LevelEditorGridNode>(transform.position, m_worldSize, m_nodeRadius, true, createVisual ? OnNodeCreated : null);
    }

    private void OnNodeCreated(LevelEditorGridNode node)
    {
        CreateNodeVisual(node);
    }

    public void SelectNode(LevelEditorGridNode node)
    {
        if(node == m_selectedNode)
            return;

        if (m_selectedNode != null)
            m_selectedNode.m_visualRenderer.material = m_defaultMaterial;

        if (node != null)
            node.m_visualRenderer.material = m_selectedMaterial;

        m_selectedNode = node;
    }
    
    private void CreateNodeVisual(LevelEditorGridNode node)
    {
        Vector3 offset = new Vector3(m_grid.NodeRadius, 0, m_grid.NodeRadius) * 0.9f;
        
        Vector3[] vertices = {
            node.m_worldPosition + new Vector3(-offset.x, 0, -offset.z),
            node.m_worldPosition + new Vector3(-offset.x, 0, offset.z),
            node.m_worldPosition + new Vector3 (offset.x, 0, offset.z),
            node.m_worldPosition + new Vector3 (offset.x, 0, -offset.z),
        };

        int[] triangles = new int[6]
        {
            0, 1, 2,
            0, 2, 3
        };

        GameObject nodeObject = new GameObject($"Node {m_grid.AmountNodesGenerated}");
        MeshRenderer meshRenderer = nodeObject.AddComponent<MeshRenderer>();
        Mesh mesh = nodeObject.AddComponent<MeshFilter>().mesh;
        
        mesh.Clear ();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.Optimize ();
        mesh.RecalculateNormals ();

        meshRenderer.material = m_defaultMaterial;
        
        node.m_visualRenderer = meshRenderer;
    }
    
    private void OnDrawGizmos()
    {
        if (EditorApplication.isPlaying)
            return;

        //Show preview of grid
        CreateGrid(false);   
        Gizmos.color = Color.red;
        
        for (int x = 0; x < m_grid.GridSize.x; x++)
        {
            for (int y = 0; y < m_grid.GridSize.y; y++)
            {
                LevelEditorGridNode node = m_grid.GetNode(x, y);

                Gizmos.DrawWireCube(node.m_worldPosition, new Vector3(m_grid.NodeDiameter, 0.1f, m_grid.NodeDiameter));
            }
        }
        
        Gizmos.color = Color.white;
    }
}
