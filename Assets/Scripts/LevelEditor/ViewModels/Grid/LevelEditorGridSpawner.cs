using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorGridSpawner : MonoBehaviour
{
    [SerializeField] 
    private LevelEditorGridTileViewModel m_tilePrefab;

    [SerializeField] 
    private RectTransform m_tilesContainer;

    [SerializeField] 
    private GridLayoutGroup m_tilesGridLayoutGroup;

    public delegate void OnTileSpawnedDelegate(LevelEditorGridTileViewModel tile);

    public LevelEditorGridTileViewModel[,] Spawn(int gridSizeX, int gridSizeY, OnTileSpawnedDelegate onTileSpawned = null)
    {
        FitToGridLayout(gridSizeX, gridSizeY);
        var tiles = new LevelEditorGridTileViewModel[gridSizeX, gridSizeY];

        int count = 0;

        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                LevelEditorGridTileViewModel tile = Instantiate(m_tilePrefab, m_tilesContainer);

                tile.name = $"Tile {count + 1}";
                
                tile.GridPos = new Vector2Int(x, y);
                
                tiles[x, y] = tile;

                count++;

                onTileSpawned?.Invoke(tiles[x, y]);
            }
        }

        return tiles;
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
