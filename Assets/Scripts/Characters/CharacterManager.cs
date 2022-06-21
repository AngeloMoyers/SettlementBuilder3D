using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    //TargetedCharacterUI

    List<CharacterBase> m_allCharactersInScene = new List<CharacterBase>();
    CharacterBase m_activeCharacter;
    CharacterInputMode m_activeInput;

    //Mode
    InputModeType m_currentInputMode;

    //Managers
    InputManager m_inputManager;
    SettlementModeManager m_settlementManager;
    CharacterFactory m_characterFactory;
    PortaitUIScript m_portaitUI;

    private void Awake()
    {
        m_inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        m_settlementManager = GameObject.Find("SettlementManager").GetComponent<SettlementModeManager>();   
        m_characterFactory = GetComponent<CharacterFactory>();
        m_portaitUI = GameObject.Find("PortraitUI").GetComponent<PortaitUIScript>();
    }

    private void Start()
    {
    }

    void Update()
    {
        HandleCharacterInput();
    }

    private void HandleCharacterInput()
    {
        if (m_activeInput != null)
            m_activeInput.HandleInput();
    }

    public void AddCharacter(CharacterBase character)
    {
        m_allCharactersInScene.Add(character);

        m_settlementManager.AddPopulation(1);
    }

    public void RemoveCharacter(CharacterBase character)
    {
        m_allCharactersInScene.Remove(character);

        m_settlementManager.AddPopulation(-1);
    }

    public void SetActiveCharacter(CharacterBase charBase)
    {
        m_activeCharacter = charBase;
        m_activeCharacter.SetAsActiveCharacter();
        m_activeCharacter.SetInputMode(m_currentInputMode);

        m_activeInput = m_activeCharacter.GetCurrentInputMode();
        m_portaitUI.ShowPortrait(charBase);
    }

    public void SetInputMode(InputModeType mode)
    {
        m_currentInputMode = mode;
        if (m_activeCharacter != null)
            m_activeCharacter.SetInputMode(mode);
    }

    public GameObject GetActiveCharacterGO()
    {
        if (m_activeCharacter != null)
        {
            return m_activeCharacter.gameObject;
        }
        else return null;
    }
    public CharacterBase GetActiveCharacter()
    {
        if (m_activeCharacter != null)
        {
            return m_activeCharacter;
        }
        else return null;
    }
}
