using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum WObjectType
{
    Tree,
    Stone,
    Iron
}
public class WorldObjectBase : MonoBehaviour, IUseableObject
{
    [SerializeField] protected bool m_gatherable;
    [SerializeField] protected bool m_walkable;

    [SerializeField] protected string m_name;
    [SerializeField] protected string m_info;
    [SerializeField] protected int m_maxUseDistance = 1;

    //Tile Data
    protected List<WorldTile> myTiles = new List<WorldTile>();
    protected List<WorldTile> myNeighbors = new List<WorldTile>();

    virtual public string GetName() { return m_name; }
    virtual public string GetInfo() { return m_info; }

    virtual public bool Use(CharacterBase user) { return true; }

    virtual public bool Spawn(TileManager tileMan, Tilemap groundTilemap) 
    {
        Collider col = GetComponent<BoxCollider>();

        Bounds bounds = col.bounds;
        float width = bounds.size.x;
        float height = bounds.size.z;

        Collider[] cols = Physics.OverlapBox(bounds.center, bounds.size / 2, transform.rotation);
        foreach (var c in cols)
        {
            var colliders = Physics.OverlapSphere(groundTilemap.CellToWorld(new Vector3Int(0, 0, 0)), 0.5f);
            foreach (var co in colliders)
            {
                if (co.gameObject == this.gameObject)
                {
                    Despawn();
                    return false;
                }
            }

            if (c.gameObject != this.gameObject && c.tag != "Ground")
            {
                Despawn();
                return false;
            }
        }

        Vector3 topRight = bounds.center, topLeft = bounds.center, bottomRight = bounds.center, bottomLeft = bounds.center;

        topRight.x += width / 2;
        topRight.z += height / 2;

        topLeft.x -= width / 2;
        topLeft.z += height / 2;

        bottomRight.x += width / 2;
        bottomRight.z -= height / 2;

        bottomLeft.x -= width / 2;
        bottomLeft.z -= height / 2;

        //Top Left
        if (groundTilemap.HasTile(groundTilemap.WorldToCell(topLeft)))
        {
            if (!myTiles.Contains(tileMan.GetWorldTile(topLeft)))
            {
                myTiles.Add(tileMan.GetWorldTile(topLeft));
            }
        }
        //Top Right
        if (groundTilemap.HasTile(groundTilemap.WorldToCell(topRight)))
        {
            if (!myTiles.Contains(tileMan.GetWorldTile(topRight)))
            {
                myTiles.Add(tileMan.GetWorldTile(topRight));
            }
        }
        //Bottom Left
        if (groundTilemap.HasTile(groundTilemap.WorldToCell(bottomLeft)))
        {
            if (!myTiles.Contains(tileMan.GetWorldTile(bottomLeft)))
            {
                myTiles.Add(tileMan.GetWorldTile(bottomLeft));
            }
        }
        //Bottom Right
        if (groundTilemap.HasTile(groundTilemap.WorldToCell(bottomRight)))
        {
            if (!myTiles.Contains(tileMan.GetWorldTile(bottomRight)))
            {
                myTiles.Add(tileMan.GetWorldTile(bottomRight));
            }
        }

        if (!m_walkable)
        {
            foreach (var t in myTiles)
            {
                t.SetOccupier(this.gameObject);
            }
        }

        foreach (var t in myTiles)
        {
            foreach (var n in t.neighbors)
            {
                if (n.occupier != this.gameObject)
                {
                    if (!myNeighbors.Contains(n))
                        myNeighbors.Add(n);
                }
            }
        }

        return true;
    }

    protected bool IsANeighbor(GameObject obj)
    {
        bool canUse = false;
        foreach (var n in myNeighbors)
        {
            if (n.occupier == obj)
            {
                canUse = true;
                break;
            }
        }
        return canUse;
    }

    virtual protected void Despawn()
    {
        if (!m_walkable)
        {
            foreach (var t in myTiles)
            {
                t.RemoveOccupier();
            }
        }

        Destroy(gameObject);
    }
}
