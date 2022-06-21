using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public enum ResourceType
{
    Wood,
    Stone,
    Iron
}

[System.Serializable]
public class ResourceCost
{
    public ResourceType type;
    public int cost;
}
public class BuildableObject : MonoBehaviour
{
    [SerializeField] protected string name;
    [SerializeField] protected string info;
    [SerializeField] protected GameObject prefab;
    [SerializeField] public ResourceCost[] resources;
    [SerializeField] public Vector3Int dimensions;
    [SerializeField] public Texture2D sprite;

    public int ID = -1;
    protected bool isWall = false;

    CharacterBase owner;

    //TileData
    protected List<WorldTile> myTiles = new List<WorldTile>();

    //Managers
    protected TileManager tileManager;


    //Helpers
    public virtual string GetName() { return name; }
    public virtual string GetInfo() { return info; }
    public virtual bool GetIsWall() { return isWall; }

    public virtual CharacterBase GetOwner() { return owner; }
    public virtual void SetOwner(CharacterBase cBase) { owner = cBase; }

    public virtual void Build(TileManager tileMan, Tilemap groundTM)
    {
        tileManager = tileMan;

        Collider col = GetComponent<BoxCollider>();

        Bounds bounds = col.bounds;
        float width = bounds.size.x;
        float height = bounds.size.z;

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
        if (groundTM.HasTile(groundTM.WorldToCell(topLeft)))
        {
            if (!myTiles.Contains(tileMan.GetWorldTile(topLeft)))
            {
                myTiles.Add(tileMan.GetWorldTile(topLeft));
            }
        }
        //Top Right
        if (groundTM.HasTile(groundTM.WorldToCell(topRight)))
        {
            if (!myTiles.Contains(tileMan.GetWorldTile(topRight)))
            {
                myTiles.Add(tileMan.GetWorldTile(topRight));
            }
        }
        //Bottom Left
        if (groundTM.HasTile(groundTM.WorldToCell(bottomLeft)))
        {
            if (!myTiles.Contains(tileMan.GetWorldTile(bottomLeft)))
            {
                myTiles.Add(tileMan.GetWorldTile(bottomLeft));
            }
        }
        //Bottom Right
        if (groundTM.HasTile(groundTM.WorldToCell(bottomRight)))
        {
            if (!myTiles.Contains(tileMan.GetWorldTile(bottomRight)))
            {
                myTiles.Add(tileMan.GetWorldTile(bottomRight));
            }
        }

        foreach (var t in myTiles)
        {
            t.SetOccupier(this.gameObject);
        }
    }

    public virtual void BeforeDestroy()
    {
        foreach (var t in myTiles)
        {
            t.RemoveOccupier();
        }
    }
}
