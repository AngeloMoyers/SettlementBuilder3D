using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] Tile m_walkTile;

    Tilemap m_groundTilemap;

    Dictionary<Vector3Int, WorldTile> m_coordToWorldTileMap = new Dictionary<Vector3Int, WorldTile>();
    List<WorldTile> m_map = new List<WorldTile>();
    public Tilemap GetGroundTilemap() { return m_groundTilemap; }
    public List<WorldTile> GetMapTiles() { return m_map; }

    public Dictionary<Vector3Int, WorldTile>  GetTileDict()
    {
        return m_coordToWorldTileMap;
    }

    private void Awake()
    {
        m_groundTilemap = GameObject.Find("Ground").GetComponent<Tilemap>();

        foreach (var pos in m_groundTilemap.cellBounds.allPositionsWithin)
        {
            var position = new Vector3Int(pos.x, pos.y, pos.z);
            var wTile = new WorldTile(true, pos.x, pos.y);
            m_coordToWorldTileMap.Add(position, wTile);
            wTile.walkTile = m_groundTilemap.GetTile(m_groundTilemap.WorldToCell(pos)) as Tile;
            wTile.tilePrefab = m_walkTile;
        }

        foreach (var tPair in m_coordToWorldTileMap)
        {
            Vector3Int left = new Vector3Int(tPair.Key.x - 1, tPair.Key.y, tPair.Key.z);
            Vector3Int right = new Vector3Int(tPair.Key.x + 1, tPair.Key.y, tPair.Key.z);
            Vector3Int up = new Vector3Int(tPair.Key.x, tPair.Key.y + 1, tPair.Key.z);
            Vector3Int down = new Vector3Int(tPair.Key.x, tPair.Key.y - 1, tPair.Key.z);

            if (m_coordToWorldTileMap.ContainsKey(left))
                tPair.Value.neighbors.Add(m_coordToWorldTileMap[left]);
            if (m_coordToWorldTileMap.ContainsKey(right))
                tPair.Value.neighbors.Add(m_coordToWorldTileMap[right]);
            if (m_coordToWorldTileMap.ContainsKey(up))
                tPair.Value.neighbors.Add(m_coordToWorldTileMap[up]);
            if (m_coordToWorldTileMap.ContainsKey(down))
                tPair.Value.neighbors.Add(m_coordToWorldTileMap[down]);

            m_map.Add(tPair.Value);
        }
    }

    //Returns Tile at World Position
    public WorldTile GetWorldTile(Vector3 pos)
    {
        var tile = m_coordToWorldTileMap[m_groundTilemap.WorldToCell(pos)]; //always tile at zero for Y
        return tile;
    }

}
