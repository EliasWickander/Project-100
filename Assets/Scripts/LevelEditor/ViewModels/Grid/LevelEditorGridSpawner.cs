using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorGridSpawner : MonoBehaviour
{
    [SerializeField] 
    private LevelEditorGridTileViewModel m_gridTilePrefab;

    [SerializeField] 
    private LevelEditorGridTileViewModel m_outsideTilePrefab;
    
    [SerializeField] 
    private RectTransform m_tilesContainer;

    [SerializeField] 
    private GridLayoutGroup m_tilesGridLayoutGroup;

    public delegate void OnGridTileSpawnedDelegate(LevelEditorGridTileViewModel tile);
    public delegate void OnOutsideTileSpawnedDelegate(LevelEditorGridTileViewModel tile);

    public void Spawn(int gridSizeX, int gridSizeY, OnGridTileSpawnedDelegate onGridTileSpawned = null, OnOutsideTileSpawnedDelegate onOutsideTileSpawned = null)
    {
        //Make space for outside tiles
        gridSizeX += 2;
        gridSizeY += 2;
        
        FitToGridLayout(gridSizeX, gridSizeY);

        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                if (x == 0 || x == gridSizeX - 1 || y == 0 || y == gridSizeY - 1)
                {
                    LevelEditorGridTileViewModel outsideTile = Instantiate(m_outsideTilePrefab, m_tilesContainer);

                    outsideTile.IsOutsideTile = true;
                    
                    onOutsideTileSpawned?.Invoke(outsideTile);
                }
                else
                {
                    LevelEditorGridTileViewModel gridTile = Instantiate(m_gridTilePrefab, m_tilesContainer);

                    gridTile.IsOutsideTile = false;
                
                    gridTile.GridPos = new Vector2Int(x - 1, y - 1);

                    onGridTileSpawned?.Invoke(gridTile);   
                }
            }
        }
    }

    private void FitToGridLayout(int gridSizeX, int gridSizeY)
    {
        Rect gridRect = m_tilesContainer.rect;

        float cellSizeX = gridRect.width / gridSizeX;
        float cellSizeY = gridRect.height / gridSizeY;

        cellSizeX -= m_tilesGridLayoutGroup.spacing.x;
        cellSizeY -= m_tilesGridLayoutGroup.spacing.y;

        m_tilesGridLayoutGroup.cellSize = new Vector2(cellSizeX, cellSizeY);
    }
}
