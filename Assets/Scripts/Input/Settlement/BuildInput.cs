using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;


public class BuildInput : InputMode
{
    [SerializeField] Tile m_hoverTile;

    //UI
    BuildModeUI m_buildModeUI;

    //Tilemap
    Tilemap m_groundTileMap;

    //Tile Highlighting
    Vector3Int m_currentCellPos;
    Vector3Int m_previousCellPos;

    Vector3 m_currentCellCenter;
    Vector3 m_previousCellCenter;

    bool m_tilePosChangedThisFrame = false;

    //Managers
    InputManager m_inputManager;
    BuildModeManager m_buildModeManager;

    void Start()
    {
        m_buildModeUI = GameObject.Find("BuildModeUI").GetComponent<BuildModeUI>();

        m_groundTileMap = GameObject.Find("Ground").GetComponent<Tilemap>();

        m_inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        m_buildModeManager = GameObject.Find("BuildManager").GetComponent<BuildModeManager>();
    }

    public override void SetActive(bool newState)
    {
        m_buildModeUI.SetActive(newState);
        m_buildModeManager.SetActive(newState);

        //if (newState)
        //    Time.timeScale = 0;
        //else
        //    Time.timeScale = 1;
    }

    public override void HandleInput()
    {
        UpdateCellPositions();

        if (Input.GetMouseButton(0) && m_tilePosChangedThisFrame)
        {
            if (HasClickedUI()) return;
            m_buildModeManager.BuildObject(m_currentCellPos);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (HasClickedUI()) return;
            m_buildModeManager.BuildObject(m_currentCellPos);
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (HasClickedUI()) return;
            m_buildModeManager.DestroyBOObject(m_currentCellPos);
        }

        if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Escape))
        {
            m_inputManager.SetInputMode(InputModeType.Settlement);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            m_buildModeManager.RotateObject();
        }
    }

    private void HighlightMouseOvertile()
    {
        m_buildModeManager.HoverCurrentTile(m_currentCellPos);
    }

    private bool HasClickedUI()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return true;
        else
            return false;
    }

    private void UpdateCellPositions()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 hitPos = Vector3.zero;

        if (Physics.Raycast(ray, out hit))
        {
            hitPos = hit.point;
        }
        
        m_currentCellPos = m_groundTileMap.WorldToCell(hitPos);
        m_currentCellCenter = m_groundTileMap.GetCellCenterWorld(m_currentCellPos);

        m_tilePosChangedThisFrame = false;

        if (!m_currentCellPos.Equals(m_previousCellPos))
        {
            HighlightMouseOvertile();
            m_previousCellPos = m_currentCellPos;
            m_previousCellCenter = m_groundTileMap.GetCellCenterWorld(m_previousCellPos);

            m_tilePosChangedThisFrame = true;
        }
    }
}
