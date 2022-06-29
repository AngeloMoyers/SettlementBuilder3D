using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildModeManager : MonoBehaviour
{
    //Tilemaps
    Tilemap m_groundTilemap;

    //Managers
    SettlementModeManager m_settlementManager;
    TileManager m_tileManager;

    //Object Handling
    GameObject m_activeObject;
    GameObject m_BOContainer;

    //Hover
    GameObject m_ghost;
    Transform m_visual;
    Vector3 m_ghostTargetPos;

    public void SetActive(bool newState)
    {
        if (newState)
        {
            RefreshGhostVisual();
        }
        else
        {
            DestroyGhost();
        }
    }

    public void SetActiveObject(GameObject obj)
    { 
        m_activeObject = obj;
        RefreshGhostVisual();
    }

    private void Awake()
    {
        m_groundTilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
        
        m_settlementManager = GameObject.Find("SettlementManager").GetComponent<SettlementModeManager>();
        m_tileManager = GameObject.Find("TileManager").GetComponent<TileManager>();
    }

    void Start()
    {
        m_BOContainer = new GameObject("BOContainer");
        m_BOContainer.transform.parent = transform;

        RefreshGhostVisual();
    }

    private void LateUpdate()
    {
        UpdateGhostPosition();
    }

    #region ObjectControls
    public void BuildObject(Vector3Int pos)
    {
        if (m_activeObject == null)
            return;

        SnapGhostToTargetPosition();

        BuildableObject BOScript = m_activeObject.GetComponentInChildren<BuildableObject>();
        if (CanBuild(BOScript))
        {
            foreach (ResourceCost r in BOScript.resources)
            {
                ChargeBuildCost(r.type, r.cost);
            }

            //Instantiate at rotation
            var dims = m_visual.GetComponentInChildren<BuildableObject>().dimensions;
            //if dimensions are even, spawn at edge of tile, else spawn in center
            Vector3 spawnLocation = new Vector3(dims.x % 2 == 0 ? pos.x : m_groundTilemap.GetCellCenterWorld(pos).x, 0, dims.y % 2 == 0 ? pos.y : m_groundTilemap.GetCellCenterWorld(pos).z);

            GameObject spawnedObj = Instantiate(m_activeObject, spawnLocation, m_visual.transform.rotation, m_BOContainer.transform);
            BuildableObject spawnedBO = spawnedObj.GetComponentInChildren<BuildableObject>();
            spawnedBO.Build(m_tileManager, m_groundTilemap);
        }
    }

    public void DestroyBOObject(Vector3Int pos)
    {
        Vector3 cellCenter = m_groundTilemap.GetCellCenterWorld(pos);

        Collider[] col = Physics.OverlapSphere(cellCenter, 0.5f);

        foreach (var c in col)
        {
            if (c != null)
            {
                //if has parent
                if (c.transform.parent.GetComponent<BOGhost>() != null) //If this is the ghost, skip
                    continue;

                GameObject obj = c.gameObject;
                BuildableObject BO = obj.GetComponent<BuildableObject>();
                if (BO != null)
                {
                    BO.BeforeDestroy();
                    Destroy(obj);
                }
            }
        }
    }

    public void RotateObject()
    {
        if (m_visual == null) return;

        Quaternion curRot = m_visual.transform.rotation;
        m_visual.transform.rotation = Quaternion.Euler(curRot.eulerAngles.x, curRot.eulerAngles.y + 90f, curRot.eulerAngles.z);

        //swamp dimension on rotate
        var dims = m_visual.GetComponentInChildren<BuildableObject>().dimensions;
        m_visual.GetComponentInChildren<BuildableObject>().dimensions = new Vector3Int(dims.y, dims.x, dims.z);

        var pos = Vector3Int.FloorToInt(m_visual.transform.position);
        Vector3Int swappedPos = new Vector3Int(pos.x, pos.z, pos.y);
        HoverCurrentTile(swappedPos);
    }

    #endregion
    private void ChargeBuildCost(ResourceType type, int cost)
    {
        m_settlementManager.ChargeResource(type, cost);
    }

    private bool CanBuild(BuildableObject obj)
    {
        //If have resources
        foreach (ResourceCost r in obj.resources)
        {
            if (m_settlementManager.HasEnoughResources(r.type, r.cost))
            {
                continue;
            }
            else
            {
                MessagePrinter.DisplayMessage("Not enough resources to build", Color.red);
                return false;
            }
        }
        //If no other object is in the way

        List<WorldTile> tiles = m_visual.GetComponentInChildren<BuildableObject>().GetOverlappingTiles(m_tileManager);
        foreach (var t in tiles)
        {
            if (t.occupier != null)
            {
                MessagePrinter.DisplayMessage("Another object is obstructing building here", Color.red);
                return false;
            }
        }
        return true;
    }

    private void UpdateGhostPosition()
    {
        if (m_visual == null || m_visual.position == m_ghostTargetPos)
            return;
        m_visual.position = Vector3.Lerp(m_visual.position, m_ghostTargetPos, Time.deltaTime * 25f);
    }

    private void SnapGhostToTargetPosition()
    {
        m_visual.position = m_ghostTargetPos;
    }

    public void HoverCurrentTile(Vector3Int pos)
    {
        if (m_visual == null) return;
        var visBO = m_visual.GetComponentInChildren<BuildableObject>();
        Vector3Int dims;
        if (visBO != null)
        {
            dims = m_visual.GetComponentInChildren<BuildableObject>().dimensions;
        }
        else return;

        Vector3 spawnLocation = new Vector3(dims.x % 2 == 0 ? pos.x : m_groundTilemap.GetCellCenterWorld(pos).x, 0, dims.y % 2 == 0 ? pos.y : m_groundTilemap.GetCellCenterWorld(pos).z);

        m_ghostTargetPos = spawnLocation; //TODO, set up prefabs and code to spawn at center of tile
        //use dims to test, if %2, top of tile, else, center
    }

    private void DestroyGhost()
    {
        if (m_visual != null)
        {
            Destroy(m_visual.gameObject);
            m_visual = null;
        }
    }

    private void RefreshGhostVisual()
    {
        if (m_activeObject == null) return;
        if (m_visual != null)
        {
            Destroy(m_visual.gameObject);
            m_visual = null;
        }

        BuildableObject BOScript = m_activeObject.GetComponentInChildren<BuildableObject>();
        if (BOScript != null)
        {
            m_visual = Instantiate(m_activeObject.transform, Vector3.zero, Quaternion.identity);
            m_visual.name = "Ghost";
            m_visual.gameObject.AddComponent<BOGhost>();
            m_visual.parent = transform;
            m_visual.localPosition = Vector3.zero;
            m_visual.localEulerAngles = Vector3.zero;
            SetGhostLayerRecursive(m_visual.gameObject, 11);
        }
    }

    private void SetGhostLayerRecursive(GameObject target, int layer)
    {
        target.layer = layer;
        foreach (Transform child in target.transform)
        {
            SetGhostLayerRecursive(child.gameObject, layer);
        }
    }
}
