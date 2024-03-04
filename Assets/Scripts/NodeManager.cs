using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeManager : MonoBehaviour
{
    [SerializeField] private Tilemap wallMap;
    [SerializeField] private Tilemap floorMap;
    public List<Vector2> wallPositions = new();
    public List<Vector2> floorPositions = new();
    private BoundsInt bounds;

    private void Awake()
    {
        wallMap.CompressBounds();
        bounds = wallMap.cellBounds;
        TileBase[] allWallTiles = wallMap.GetTilesBlock(bounds);
        for (int x = bounds.x; x < bounds.x + bounds.size.x; x++)
        {
            for (int y = bounds.y; y < bounds.y + bounds.size.y; y++)
            {
                Vector3Int wallTilePosition = new Vector3Int(x, y, 0);
                TileBase wallTile = allWallTiles[(x - bounds.x) + (y - bounds.y) * bounds.size.x];

                if (wallTile != null)
                {
                    Vector3 wallTileWorldPos = wallMap.GetCellCenterWorld(wallTilePosition);
                    wallPositions.Add(wallTileWorldPos);
                }
                else
                {
                    Vector3 floorTileWorldPos = wallMap.GetCellCenterWorld(wallTilePosition);
                    floorPositions.Add(floorTileWorldPos);
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        foreach (var tile in wallPositions)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(tile, new Vector2(0.2f, 0.2f));
        }
        foreach (var tile in floorPositions)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(tile, new Vector2(0.2f, 0.2f));
        }
    }
}
