using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WoodWall : StaticBO
{
    [System.Serializable]
    struct WallData
    {
        public StructureSection section;
        public Mesh mesh;
        public float rotation;
    }

    [SerializeField] WallData[] m_wallSections;

    Dictionary<StructureSection, WallData> m_sectionToSpriteMap = new Dictionary<StructureSection, WallData>();

    private void Start()
    {
        isWall = true;
    }

    public override void Build(TileManager tileMan, Tilemap groundTM)
    {
        base.Build(tileMan, groundTM);

        isWall = true;

       InitSpriteMap();

        Reform(groundTM);
        ReformNeighbors(groundTM);
    }

    public override void Reform(Tilemap groundTM)
    {
        var mesh = GetComponent<MeshFilter>();
        if (mesh != null)
        {
            var pos = groundTM.WorldToCell(GetComponent<BoxCollider>().bounds.center);
            var section = GetMySectionType(tileManager.GetTileDict(), pos);
            if (m_sectionToSpriteMap.ContainsKey(section))
            {
                mesh.mesh = m_sectionToSpriteMap[section].mesh;
                transform.eulerAngles = new Vector3(0.0f, m_sectionToSpriteMap[section].rotation, 0.0f);
            }
            else
                Debug.Log("Structure Section Not Found in SectionMap");
        }
    }

    public override void BeforeDestroy()
    {
        base.BeforeDestroy();
        isWall = false;
        ReformNeighbors(GameObject.Find("Ground").GetComponent<Tilemap>());
    }

    private void ReformNeighbors(Tilemap groundTM)
    {
        foreach (var tile in myTiles)
        {
            foreach (var neighbor in tile.neighbors)
            {
                if (neighbor.occupier == null)
                    continue;

                BuildableObject BOScript = neighbor.occupier.GetComponent<BuildableObject>();
                if (BOScript == null)
                    continue;

                if (BOScript.GetIsWall())
                {
                    BOScript.gameObject.GetComponent<StaticBO>().Reform(groundTM);
                }

            }
        }
    }

    private StructureSection GetMySectionType(Dictionary<Vector3Int, WorldTile> map, Vector3Int pos)
    {
        int mask = 0;

        //Get Surrounding Tiles
        //Above
        Vector3Int temp = pos;
        temp.y += 1;
        if (map.ContainsKey(temp))
        {
            if (map[temp].occupier != null)
            {
                var bo = map[temp].occupier.GetComponent<BuildableObject>();
                if (bo != null && bo.GetIsWall())
                    mask += 1;
            }
        }
        else
            Debug.Log("WorldTile Map Doesn't contain Position Key Given");
        //Below
        temp = pos;
        temp.y -= 1;
        if (map.ContainsKey(temp))
        {
            if (map[temp].occupier != null)
            {
                var bo = map[temp].occupier.GetComponent<BuildableObject>();
                if (bo != null && bo.GetIsWall())
                    mask += 8;
            }
        }
        else
            Debug.Log("WorldTile Map Doesn't contain Position Key Given");
        //Left
        temp = pos;
        temp.x -= 1;
        if (map.ContainsKey(temp))
        {
            if (map[temp].occupier != null)
            {
                var bo = map[temp].occupier.GetComponent<BuildableObject>();
                if (bo != null && bo.GetIsWall())
                    mask += 2;
            }
        }
        else
            Debug.Log("WorldTile Map Doesn't contain Position Key Given");
        //Right
        temp = pos;
        temp.x += 1;
        if (map.ContainsKey(temp))
        {
            if (map[temp].occupier != null)
            {
                var bo = map[temp].occupier.GetComponent<BuildableObject>();
                if (bo != null && bo.GetIsWall())
                    mask += 4;
            }
        }
        else
            Debug.Log("WorldTile Map Doesn't contain Position Key Given");

        switch (mask)
        {
            case 0:
                return StructureSection.Alone;
            case 1:
                return StructureSection.Vertical;
            case 2:
                return StructureSection.Horizontal;
            case 3:
                return StructureSection.BR;
            case 4:
                return StructureSection.Horizontal;
            case 5:
                return StructureSection.BL;
            case 6:
                return StructureSection.Horizontal;
            case 7:
                return StructureSection.ThreeUp;
            case 8:
                return StructureSection.Vertical;
            case 9:
                return StructureSection.Vertical;
            case 10:
                return StructureSection.TR;
            case 11:
                return StructureSection.ThreeLeft;
            case 12:
                return StructureSection.TL;
            case 13:
                return StructureSection.ThreeRight;
            case 14:
                return StructureSection.ThreeDown;
            case 15:
                return StructureSection.FourWay;

            default:
                return StructureSection.Alone;
        }
    }

    private void InitSpriteMap()
    {
        foreach (WallData wall in m_wallSections)
        {
            var section = wall.section;
            if (!m_sectionToSpriteMap.ContainsKey(section))
            {
                m_sectionToSpriteMap.Add(section, wall);
            }
        }
    }
}
