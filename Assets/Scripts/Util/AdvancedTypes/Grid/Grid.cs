using System.Collections.Generic;
using UnityEngine;

namespace Util.AdvancedTypes
{
	public class GridNode 
	{
		public Vector3 m_worldPosition;
		public Vector2Int m_gridPos;
	}
	
	public class Grid<TNode> where TNode : GridNode, new()
	{
		public delegate void OnNodeCreatedDelegate(GridNode node);

		private OnNodeCreatedDelegate OnNodeCreated;

		public Vector2 GridWorldSize => m_gridWorldSize;
		private Vector2 m_gridWorldSize = new Vector2(10, 10);

		public float NodeRadius => m_nodeRadius;
		private float m_nodeRadius = 0.2f;

		public TNode[,] Nodes => m_nodes;
		private TNode[,] m_nodes;
		
		public Vector2Int GridSize => m_gridSize;
		private Vector2Int m_gridSize;

		public Vector3 OriginPoint => m_originPoint;
		private Vector3 m_originPoint = Vector3.zero;

		public float NodeDiameter => m_nodeDiameter;
		private float m_nodeDiameter;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="originPoint">Grid's start point</param>
		/// <param name="worldSize">Grid's world size</param>
		/// <param name="nodeRadius">Radius of each node</param>
		/// <param name="createOnConstruct">If grid should be created upon construct</param>
		/// <param name="onNodeCreated">Method that should be called whenever upon node creation</param>
		public Grid(Vector3 originPoint, Vector2 worldSize, float nodeRadius, bool createOnConstruct = true, OnNodeCreatedDelegate onNodeCreated = null)
		{
			m_originPoint = originPoint;
			m_gridWorldSize = worldSize;
			m_nodeRadius = nodeRadius;
			
			m_nodeDiameter = m_nodeRadius*2;
			m_gridSize.x = Mathf.RoundToInt(m_gridWorldSize.x/m_nodeDiameter);
			m_gridSize.y = Mathf.RoundToInt(m_gridWorldSize.y/m_nodeDiameter);

			OnNodeCreated = onNodeCreated;
			
			if(createOnConstruct)
				CreateGrid();
		}

		/// <summary>
		/// Creates grid
		/// </summary>
		public void CreateGrid() 
		{
			m_nodes = new TNode[m_gridSize.x, m_gridSize.y];

			for (int x = 0; x < m_gridSize.x; x ++) 
			{
				for (int y = 0; y < m_gridSize.y; y ++) 
				{
					Vector3 worldPoint = m_originPoint + Vector3.right * (x * m_nodeDiameter + m_nodeRadius) + Vector3.forward * (y * m_nodeDiameter + m_nodeRadius);
					Vector2Int gridPos = new Vector2Int(x, y);

					TNode newNode = new TNode();
					newNode.m_worldPosition = worldPoint;
					newNode.m_gridPos = gridPos;
					
					m_nodes[x, y] = newNode;

					OnNodeCreated?.Invoke(m_nodes[x, y]);
				}
			}
		}

		/// <summary>
		/// Gets node by world position
		/// </summary>
		/// <param name="worldPosition">World position</param>
		/// <returns>Returns node closest to world position</returns>
		public TNode GetNode(Vector3 worldPosition)
		{
			worldPosition -= m_originPoint;
			
			float percentX = worldPosition.x / m_gridWorldSize.x;
			float percentY = worldPosition.z / m_gridWorldSize.y;
			percentX = Mathf.Clamp01(percentX);
			percentY = Mathf.Clamp01(percentY);

			int x = Mathf.RoundToInt((m_gridSize.x - 1) * percentX);
			int y = Mathf.RoundToInt((m_gridSize.y - 1) * percentY);
			return GetNode(x, y);
		}
		
		/// <summary>
		/// Gets node by grid position
		/// </summary>
		/// <param name="x">Grid X position</param>
		/// <param name="y">Grid Y position</param>
		/// <returns>Returns node in grid position</returns>
		public TNode GetNode(int x, int y)
		{
			if (x >= m_gridSize.x || y >= m_gridSize.y || x < 0 || y < 0)
				return null;

			return m_nodes[x, y];
		}
		
		/// <summary>
		/// Gets neighbour in a direction from node
		/// </summary>
		/// <param name="node">Node to get neighbour of</param>
		/// <param name="direction">Direction</param>
		/// <returns>Returns neighbour in direction</returns>
		public TNode GetNeighbour(TNode node, Vector2Int direction)
		{
			direction.x = Mathf.Clamp(direction.x, -1, 1);
			direction.y = Mathf.Clamp(direction.y, -1, 1);

			if (node == null || direction == Vector2Int.zero)
				return null;

			return GetNode(node.m_gridPos.x + direction.x, node.m_gridPos.y + direction.y);
		}
		
		/// <summary>
		/// Get neighbouring nodes
		/// </summary>
		/// <param name="node">Node to get neighbours of</param>
		/// <returns>Returns neighbouring nodes</returns>
		public List<TNode> GetNeighbours(TNode node)
		{
			List<TNode> neighbours = new List<TNode>();
	    
			if (node.m_gridPos.x > 0)
			{
				//Left
				neighbours.Add(GetNeighbour(node, new Vector2Int(-1, 0)));
	        
				//Left Down
				if(node.m_gridPos.y > 0)
					neighbours.Add(GetNeighbour(node, new Vector2Int(-1, -1)));
	        
				//Left Up
				if(node.m_gridPos.y < m_gridSize.y - 1)
					neighbours.Add(GetNeighbour(node, new Vector2Int(-1, 1)));
			}

			if (node.m_gridPos.x < m_gridSize.x - 1)
			{
				//Right
				neighbours.Add(GetNeighbour(node, new Vector2Int(1, 0)));
	        
				//Right Down
				if(node.m_gridPos.y > 0)
					neighbours.Add(GetNeighbour(node, new Vector2Int(1, -1)));
	        
				//Right Up
				if(node.m_gridPos.y < m_gridSize.y - 1)
					neighbours.Add(GetNeighbour(node, new Vector2Int(1, 1)));
			}
	    
			//Down
			if(node.m_gridPos.y > 0)
				neighbours.Add(GetNeighbour(node, new Vector2Int(0, -1)));
	    
			//Up
			if(node.m_gridPos.y < m_gridSize.y - 1)
				neighbours.Add(GetNeighbour(node, new Vector2Int(0, 1)));
	    
			return neighbours;
		}
	}
}
