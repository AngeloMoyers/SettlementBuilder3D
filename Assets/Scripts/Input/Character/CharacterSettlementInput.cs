using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class CharacterSettlementInput : CharacterInputMode
{

    public override void HandleInput()
    {
        if (!m_isActive) return;

        if (Input.GetKey(KeyCode.W))
            m_owner.Move(new Vector2Int(0, 1));
        if (Input.GetKey(KeyCode.S))
            m_owner.Move(new Vector2Int(0, -1));
        if (Input.GetKey(KeyCode.A))
            m_owner.Move(new Vector2Int(-1, 0));
        if (Input.GetKey(KeyCode.D))
            m_owner.Move(new Vector2Int(1, 0));

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !DidClickSomething()) //If clicked and not on UI or object
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Vector3 hitPos = Vector3.zero;

            if (Physics.Raycast(ray, out hit))
            {
                hitPos = hit.point;
            }
            m_owner.SetTargetWorldTile(hitPos);
            m_owner.SetCurrentPath(AStar.GetPath(m_owner.GetMapTiles(), m_owner.m_currentWorldTile, m_owner.m_targetWorldTile));
        }
    }

    public override void SetActive(bool newState)
    {
        m_isActive = newState;
    }

    public override void SetOwner(CharacterBase character)
    {
        m_owner = character;
    }

    private bool DidClickSomething()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject obj = hit.collider.gameObject;
            if (obj != null && obj.tag != "Ground")
                return true;
        }

        return false;
    }
}
