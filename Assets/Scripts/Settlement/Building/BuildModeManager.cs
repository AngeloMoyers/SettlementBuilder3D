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
            GameObject spawnedObj = Instantiate(m_activeObject, new Vector3(pos.x, 0, pos.y), Quaternion.identity, m_BOContainer.transform);
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
        Vector3Int dims = m_visual.GetComponentInChildren<BuildableObject>().dimensions;
        Vector3 startPos = m_visual.position;
        //check tiles right
        for (int i = dims.y - 1; i >= 0; i--)
        {
            for (int j = 0; j < dims.x; j++)
            {
                var currentOffset = new Vector3(j, 0, -i);
                if (m_tileManager.GetWorldTile(startPos + currentOffset).occupier != null)
                {
                    MessagePrinter.DisplayMessage("Another object is obstructing building here", Color.red);
                    return false;
                }
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
        m_ghostTargetPos = new Vector3(pos.x, 0, pos.y);
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
