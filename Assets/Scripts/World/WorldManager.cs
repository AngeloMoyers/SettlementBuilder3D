using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct WObjectToPrefab
{
    public WObjectType type;
    public GameObject prefab;
    public int numToSpawn;
}
public class WorldManager : MonoBehaviour
{
    [SerializeField] WObjectToPrefab[] m_worldObjects;
    Dictionary<WObjectType, GameObject> m_wObjectTypeToPrefab;

    [SerializeField] int m_treesToRespawn = 2;
    [SerializeField] float m_treeRespawnRate = 60f;
    float m_lastTreeRespawnTime;

    //Container
    GameObject m_worldObjectContainer;
    //Tilemaps
    Tilemap m_groundTilemap;
    //Managers
    TileManager m_tileManager;

    private void Awake()
    {
        m_tileManager = GameObject.Find("TileManager").GetComponent<TileManager>();
        

        m_wObjectTypeToPrefab = new Dictionary<WObjectType, GameObject>();
        foreach (WObjectToPrefab obj in m_worldObjects)
        {
            m_wObjectTypeToPrefab.Add(obj.type, obj.prefab);
        }

        m_worldObjectContainer = new GameObject("WorldObjectContainer");
        m_worldObjectContainer.transform.parent = this.transform;

        m_lastTreeRespawnTime = Time.time;
    }

    private void Start()
    {
        m_groundTilemap = m_tileManager.GetGroundTilemap();
        SpawnWorldObjects();
    }

    private void Update()
    {
        HandleResourceRespawns();
    }

    private void SpawnWorldObjects()
    {
        var bounds = m_groundTilemap.cellBounds;
        foreach (WObjectToPrefab obj in m_worldObjects)
        {
            for (int i = 0; i < obj.numToSpawn; ++i)
            {
                if (!SpawnWorldObject(obj.type, new Vector3(Random.Range(bounds.xMin, bounds.xMax), 0, Random.Range(bounds.yMin, bounds.yMax))))
                    i--;
            }
        }
    }

    public bool SpawnWorldObject(WObjectType type, Vector3 pos)
    {
        if (m_tileManager.GetWorldTile(pos).occupier != null)
            return false;

        var spawnPos = m_groundTilemap.GetCellCenterWorld(m_groundTilemap.WorldToCell(pos));

        var spawnedObj = Instantiate(m_wObjectTypeToPrefab[type], spawnPos, Quaternion.identity, m_worldObjectContainer.transform);
        WorldObjectBase wObj = spawnedObj.GetComponentInChildren<WorldObjectBase>();
        return wObj.Spawn(m_tileManager, m_groundTilemap);
    }

    void HandleResourceRespawns()
    {
        var bounds = m_groundTilemap.cellBounds;

        if (Time.time > m_lastTreeRespawnTime + m_treeRespawnRate)
        {
            m_lastTreeRespawnTime = Time.time;

            for (int i = 0; i < m_treesToRespawn; ++i)
            {
                if (!SpawnWorldObject(WObjectType.Tree, new Vector3(Random.Range(bounds.xMin, bounds.xMax), Random.Range(bounds.yMin, bounds.yMax), 0)))
                    i--;
            }
        }
    }
}
