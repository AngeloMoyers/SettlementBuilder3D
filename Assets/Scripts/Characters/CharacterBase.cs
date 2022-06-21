using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//Setup:
/*
 * Ever character must have scripts for each input mode, as well
 * as this script:
 * - CharacterSettlementInput
 * - CharacterBuildInput
 * - CharacterCombatInput
 */

public enum SettlementRole
{
    Farmer,
    Blacksmith,
    Leatherworker,
    Fletcher,
    Doctor,

    None
}

public enum CombatRole
{
    Warrior,
    Archer,
    Alchemist,
    Rogue,

    None
}

public class CharacterBase : MonoBehaviour
{
    CharacterData m_data;

    CharacterInputMode m_currentInputMode;
    CharacterSettlementInput m_settlementInputMode;

    AICharacterBase m_agent;

    //Resource Management
    float m_lastEatTime;
    float m_timeTilHungry = 20f;

    //Location/Movement
    Tilemap m_groundTilemap;

    float m_speed = 5;
    bool m_isMoving;
    float m_characterFloorOffset = 0.5f;
    float m_spawnY = 1f;
    
    Vector3Int m_currentCellPosition;
    Vector3 m_currentCellCenterPosition;
    Vector3Int m_targetCellPosition;
    Vector3 m_targetCellCenterPosition;

    public WorldTile m_currentWorldTile { get; private set; }
    public WorldTile m_targetWorldTile { get; private set; }
    protected List<WorldTile> m_currentPath { get; set; }

    //Managers
    CharacterManager m_characterManager;
    TileManager m_tileManager;
    SettlementModeManager m_settlementManager;

    public List<WorldTile> GetCurrentPath() { return m_currentPath; }
    public void SetCurrentPath(List<WorldTile> p)
    {
        if (m_currentPath != null)
        {
            foreach (WorldTile t in m_currentPath)
            {
                t.RemoveOccupier();
            }
        }

        m_currentPath = p;
    } 

    virtual protected void Awake()
    {
        m_groundTilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
        m_characterManager = GameObject.Find("CharacterManager").GetComponent<CharacterManager>();
        m_tileManager = GameObject.Find("TileManager").GetComponent<TileManager>();
        m_settlementManager = GameObject.Find("SettlementManager").GetComponent<SettlementModeManager>();

        m_settlementInputMode = GetComponent<CharacterSettlementInput>();
        m_settlementInputMode.SetOwner(this);

        //TEMP TODO
        GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
        m_spawnY = transform.position.y;
    }

    virtual protected void Start()
    {
        m_characterManager.AddCharacter(this);

        SetPositionToCellCenter();

        m_lastEatTime = Time.time;
    }

    virtual protected void Update()
    {
        UpdatePosition();

        //if at settlement TODO
        Eat();
    }

    virtual protected void Eat()
    {
        if (Time.time > m_lastEatTime + m_timeTilHungry)
        {
            if (!m_settlementManager.AddFood(-1))
                Damage(-1);
            m_lastEatTime = Time.time;
        }
    }

    virtual public void Damage(int amount)
    {
        m_data.health += amount;

        if (m_data.health < 0)
            Die();
        else if (m_data.health > m_data.healthMax)
            m_data.health = m_data.healthMax;
    }

    virtual protected void Die()
    {
        MessagePrinter.DisplayMessage(m_data.name + " died of Hunger!", Color.red, 5f);
        m_characterManager.RemoveCharacter(this);
        Destroy(this.gameObject);
    }
    

    virtual public void SetData(CharacterData data)
    { m_data = data; }

    virtual public CharacterData GetData()
    { return m_data; }

    
    #region Movement
    virtual public CharacterInputMode GetCurrentInputMode()
    { return m_currentInputMode; }

    virtual public Vector3Int GetCurrentCellPosition()
    { return m_currentCellPosition; }

    virtual public void SetCellPosition(Vector3Int pos, Vector3 pos2)
    { 
        m_currentCellPosition = pos;
        m_currentCellCenterPosition = pos2;

        m_currentWorldTile = m_tileManager.GetWorldTile(pos);
        m_currentWorldTile.SetOccupier(this.gameObject);
    }

    virtual public Vector3Int GetTargetCellPosition()
    { return m_targetCellPosition; }

    virtual public void SetTargetCellPosition(Vector3Int pos, Vector3 pos2)
    {
        if (m_isMoving) return;

        m_targetCellPosition = pos;
        m_targetCellCenterPosition = pos2;
        if (!m_targetCellPosition.Equals(m_currentCellPosition))
        {
            m_isMoving = true;
        }
    }
    virtual public void SetTargetWorldTile(Vector3 pos)
    {
        m_targetWorldTile = m_tileManager.GetWorldTile(pos);
    }
    virtual public List<WorldTile> GetMapTiles() { return m_tileManager.GetMapTiles(); }

    virtual public bool GetIsMoving()
    { return m_isMoving; }

 
    virtual protected void UpdatePosition()
    {
        if (m_currentPath != null)
        {
            if (!m_targetWorldTile.Equals(m_currentWorldTile))
            {
                if (m_currentPath[0].Equals(m_currentWorldTile))
                    m_currentPath.RemoveAt(0);

                Move(m_currentPath[0]);
            }
        }
        if (m_isMoving && !m_currentCellPosition.Equals(m_targetCellPosition))
        {
            //lerp movement
            transform.position = Vector3.MoveTowards(gameObject.transform.position, m_targetCellCenterPosition, m_speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, m_targetCellCenterPosition) < 0.01f)
            {
                m_isMoving = false;
                SetWalkableTiles();

                m_currentCellPosition = m_targetCellPosition;
                m_currentCellCenterPosition = m_targetCellCenterPosition;

                m_currentWorldTile = m_tileManager.GetWorldTile(m_targetCellCenterPosition);

                if (m_currentPath != null)
                {
                    m_currentPath.RemoveAt(0);
                    if (m_currentPath.Count == 0)
                        m_currentPath = null;
                }
            }
        }
    }

    virtual public void Move(Vector2Int move)
    {
        if (m_isMoving) return; 
        var cur = m_currentCellPosition;
        Vector3Int target = new Vector3Int(cur.x + move.x, cur.y + move.y, cur.z);

        var targTile = m_tileManager.GetTileDict()[target];
        if (targTile.isWalkable)
        {
            var swappedTarget = new Vector3Int(target.x, target.z, target.y);
            m_targetWorldTile = m_tileManager.GetWorldTile(swappedTarget);

            Vector3 targetPosition =m_groundTilemap.GetCellCenterWorld(target);
            targetPosition.y = m_spawnY + m_characterFloorOffset;
            SetTargetCellPosition(target, targetPosition);
        }
    }

    virtual public void Move(WorldTile targ)
    {
        Vector3Int targetCellPos = new Vector3Int(targ.gridX, targ.gridY, 0);
        Vector3 targetPosition = m_groundTilemap.GetCellCenterWorld(targetCellPos);
        targetPosition.y = m_spawnY + m_characterFloorOffset;
        SetTargetCellPosition(targetCellPos, targetPosition);
    }

    virtual protected void SetPositionToCellCenter()
    {
        Vector3 center = m_groundTilemap.GetCellCenterWorld(m_groundTilemap.WorldToCell(gameObject.transform.position));
        center.y = m_spawnY + m_characterFloorOffset; //Temp todo
        gameObject.transform.position = center;

        SetCellPosition(m_groundTilemap.WorldToCell(gameObject.transform.position), center);
    }

    virtual protected void SetWalkableTiles()
    {
        if (m_currentWorldTile != null)
            m_currentWorldTile.RemoveOccupier();
        
        if (m_targetWorldTile != null)
            m_targetWorldTile.SetOccupier(this.gameObject);
    }
    #endregion

    #region WorldInteraction

    public bool UseObject(IUseableObject obj)
    {
        return obj.Use(this);
    }

    #endregion

    #region ManagerHelpers
    public void SetAsActiveCharacter()
    {
        //TODO, show overhead indicator
    }

    virtual public void SetInputMode(InputModeType type)
    {
        if (m_currentInputMode != null)
            m_currentInputMode.SetActive(false);
        switch (type)
        {
            case InputModeType.Settlement:
                m_currentInputMode = m_settlementInputMode;
                break;
            case InputModeType.Build:
                m_currentInputMode = null;
                break;
            case InputModeType.Combat:
                m_currentInputMode = null;
                break;
        }
        if (m_currentInputMode != null)
            m_currentInputMode.SetActive(true);
    }

    #endregion
    virtual protected void GetRandomCharacterData()
    {
        CharacterData data = new CharacterData();

        data.name = "Some Name";
        data.level = Random.Range(1, 10);
        data.health = data.healthMax;
        data.healthMax = Random.Range(1, 20);
        data.energy = data.energyMax;
        data.energyMax = Random.Range(1, 20);

        data.stats = new CharacterStats(5, 5, 5, 5, 5);

        SetData(data);
    }
}