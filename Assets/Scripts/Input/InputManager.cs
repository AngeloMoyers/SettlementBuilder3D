using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputModeType
{
    Settlement,
    Build,
    Combat
}

[System.Serializable]
public class InputTypeToInputMode
{
    public InputModeType type;
    public InputMode mode;
}

public class InputManager : MonoBehaviour
{

    //Mode Input
    [SerializeField] InputTypeToInputMode[] m_inputModes;
    InputMode m_currentInputMode;
    InputModeType m_currentInputModeType;

    //Mangers
    CharacterManager m_characterManager;
    CurrentModeIconScript m_currentModeIcon;

    private void Awake()
    {
        m_characterManager = GameObject.Find("CharacterManager").GetComponent<CharacterManager>();
        m_currentModeIcon = GameObject.Find("CurrentModeIcon").GetComponent<CurrentModeIconScript>();   
    }

    void Start()
    {
        SetInputMode(InputModeType.Settlement);
    }

    void Update()
    {
        m_currentInputMode.HandleInput();
    }

    public void ToggleBuildMode()
    {
        if (m_currentInputModeType == InputModeType.Build)
            SetInputMode(InputModeType.Settlement);
        else
            SetInputMode(InputModeType.Build);
    }

    public void SetInputMode(InputModeType modeType)
    {
        int index = -1;
        do
        {
            index++;

        } while (m_inputModes[index].type != modeType);

        if (m_currentInputMode != null)
            m_currentInputMode.SetActive(false);
        m_currentInputMode = m_inputModes[index].mode;
        m_currentInputMode.SetActive(true);

        m_currentInputModeType = modeType;
        m_characterManager.SetInputMode(modeType);

        m_currentModeIcon.SetModeIcon(m_currentInputModeType);
        //MessagePrinter.DisplayMessage("Entering " + m_currentInputModeType.ToString() + " mode.", Color.cyan);
    }

    public InputModeType GetCurrentInputModeType()
    { return m_currentInputModeType; }
}
