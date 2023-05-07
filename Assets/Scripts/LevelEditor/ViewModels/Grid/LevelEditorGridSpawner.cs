using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorGridSpawner : MonoBehaviour
{
    [SerializeField] 
    private LevelEditorTileViewModel m_gridTilePrefab;

    [SerializeField] 
    private LevelEditorTileViewModel m_outsideTilePrefab;
    
    [SerializeField] 
    private RectTransform m_tilesContainer;

    [SerializeField] 
    private GridLayoutGroup m_tilesGridLayoutGroup;

    public delegate void OnTileSpawnedDelegate(LevelEditorTileViewModel tile);

    public void Spawn(int gridSizeX, int gridSizeY, out LevelEditorTileViewModel[,] outGridTiles, out LevelEditorTileViewModel[] outOutsideTiles, OnTileSpawnedDelegate onTileSpawned = null)
    {
        outGridTiles = new LevelEditorTileViewModel[gridSizeX, gridSizeY];
        outOutsideTiles = new LevelEditorTileViewModel[(gridSizeX + 1) * 2 + (gridSizeY + 1) * 2];
        
        //Make space for outside tiles
        gridSizeX += 2;
        gridSizeY += 2;
        
        FitToGridLayout(gridSizeX, gridSizeY);

        int outsideTilesCount = 0;
        
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                if (x == 0 || x == gridSizeX - 1 || y == 0 || y == gridSizeY - 1)
                {
                    LevelEditorTileViewModel outsideTile = Instantiate(m_outsideTilePrefab, m_tilesContainer);

                    outsideTile.IsOutsideTile = true;

                    outOutsideTiles[outsideTilesCount] = outsideTile;
                    
                    onTileSpawned?.Invoke(outOutsideTiles[outsideTilesCount]);

                    outsideTilesCount++;
                }
                else
                {
                    LevelEditorTileViewModel gridTile = Instantiate(m_gridTilePrefab, m_tilesContainer);

                    gridTile.IsOutsideTile = false;
                
                    Vector2Int gridPos = new Vector2Int(x - 1, y - 1);

                    outGridTiles[gridPos.x, gridPos.y] = gridTile;
                    
                    onTileSpawned?.Invoke(outGridTiles[gridPos.x, gridPos.y]);   
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
