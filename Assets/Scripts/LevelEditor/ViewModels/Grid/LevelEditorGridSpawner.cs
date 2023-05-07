using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorGridSpawner : MonoBehaviour
{
    [SerializeField] 
    private LevelEditorGridTileViewModel m_gridTilePrefab;

    [SerializeField] 
    private LevelEditorOutsideTileViewModel m_outsideTilePrefab;
    
    [SerializeField] 
    private RectTransform m_tilesContainer;

    [SerializeField] 
    private GridLayoutGroup m_tilesGridLayoutGroup;

    public delegate void OnTileSpawnedDelegate(LevelEditorTileViewModel tile);

    public void Spawn(int gridSizeX, int gridSizeY, out LevelEditorGridTileViewModel[,] outGridTiles, out LevelEditorOutsideTileViewModel[] outOutsideTiles, OnTileSpawnedDelegate onTileSpawned = null)
    {
        outGridTiles = new LevelEditorGridTileViewModel[gridSizeX, gridSizeY];
        outOutsideTiles = new LevelEditorOutsideTileViewModel[(gridSizeX + 1) * 2 + (gridSizeY + 1) * 2];
        
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
                    LevelEditorOutsideTileViewModel outsideTile = Instantiate(m_outsideTilePrefab, m_tilesContainer);
                    
                    outOutsideTiles[outsideTilesCount] = outsideTile;
                    
                    onTileSpawned?.Invoke(outOutsideTiles[outsideTilesCount]);

                    outsideTilesCount++;
                }
                else
                {
                    LevelEditorGridTileViewModel gridTile = Instantiate(m_gridTilePrefab, m_tilesContainer);

                    gridTile.GridPos = new Vector2Int(x - 1, y - 1);

                    outGridTiles[gridTile.GridPos.x, gridTile.GridPos.y] = gridTile;
                    
                    onTileSpawned?.Invoke(outGridTiles[gridTile.GridPos.x, gridTile.GridPos.y]);   
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
