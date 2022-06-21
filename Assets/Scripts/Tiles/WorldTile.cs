using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldTile
{
    //Tile Data
    public int moveCost;
    public int gridX, gridY;
    public bool isWalkable = true;

    //TilemapData
    public Tile tilePrefab;

    public Tile walkTile;
    public Tile groundTile;
    
    //Neighbors
    public List<WorldTile> neighbors;

    //Occupying GO
    public GameObject occupier;

    public WorldTile(bool walk, int gX, int gY, int cost = 1)
    {
        this.isWalkable = walk;

        moveCost = cost;
        gridX = gX;
        gridY = gY;

        neighbors = new List<WorldTile>();
    }

    public void RemoveOccupier()
    {
        occupier = null;
        isWalkable = true;
    }

    public void SetOccupier(GameObject obj)
    {
        if (obj == null) return;

        occupier = obj;
        isWalkable = false;
    }

    public List<WorldTile> GetWalkableNeighbors()
    {
        List<WorldTile> walkNeighbors = new List<WorldTile>();
        foreach (var n in neighbors)
        {
            if (n.isWalkable)
            {
                walkNeighbors.Add(n);
            }
        }

        return walkNeighbors;
    }

    public List<WorldTile> GetWalkableAndUnoccupiedNeighbors()
    {
        List<WorldTile> walkNeighbors = new List<WorldTile>();
        foreach (var n in neighbors)
        {
            if (n.isWalkable && n.occupier == null)
            {
                walkNeighbors.Add(n);
            }
        }
        return walkNeighbors;
    }
}
