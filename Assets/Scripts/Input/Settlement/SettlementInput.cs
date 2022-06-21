using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettlementInput : InputMode
{

    //Managers
    InputManager m_inputManager;
    CharacterManager m_characterManager;

    //UI
    ClickedObjectUI m_clickedObjectUI;
    PortaitUIScript m_portaitUIScript;

    private void Awake()
    {
        m_clickedObjectUI = GameObject.Find("ClickedObjectUI").GetComponent<ClickedObjectUI>();
        m_characterManager = GameObject.Find("CharacterManager").GetComponent<CharacterManager>();
        m_portaitUIScript = GameObject.Find("PortraitUI").GetComponent<PortaitUIScript>();
    }

    void Start()
    {
        m_inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
    }
    public override void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (HasClickedUI()) return;

            IfClickedSomething();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            m_inputManager.SetInputMode(InputModeType.Build);
        }
    }

    public override void SetActive(bool newState)
    {
        //
        if(newState)
        {
            m_portaitUIScript.ShowPortrait();
        }
        else
        {
            m_portaitUIScript.HidePortrait();
        }
    }

    private bool HasClickedUI()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return true;
        else
            return false;
    }

    private GameObject GetClickedObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject obj = hit.collider.gameObject;
            if (obj != null)
                return obj;
        }

        return null;
    }

    private bool IfClickedSomething()
    {
        GameObject clickedObj = GetClickedObject();
        if (clickedObj != null)
        {
            BuildableObject bObj = null;
            bObj = clickedObj.GetComponent<BuildableObject>();
            if (bObj != null)
            {
                m_clickedObjectUI.Open(bObj);
                return true;
            }

            CharacterBase charBase = null;
            charBase = clickedObj.GetComponent<CharacterBase>();
            if (charBase != null)
            {
                m_characterManager.SetActiveCharacter(charBase);
                return true;
            }

            WorldObjectBase wObj = null;
            wObj = clickedObj.GetComponent<WorldObjectBase>();
            if (wObj != null)
            {
                m_clickedObjectUI.Open(wObj);
                return true;
            }
        }

        return false;
    }
}
