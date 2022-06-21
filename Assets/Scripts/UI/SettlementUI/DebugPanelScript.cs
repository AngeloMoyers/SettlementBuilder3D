using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugPanelScript : MonoBehaviour
{
    CharacterFactory m_characterFactory;
    SettlementModeManager m_settlementModeManager;

    private void Awake()
    {
        m_characterFactory = GameObject.Find("CharacterManager").GetComponent<CharacterFactory>();
        m_settlementModeManager = GameObject.Find("SettlementManager").GetComponent<SettlementModeManager>();
    }
    private void Start()
    {
        
    }
    public void ToggleActive()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void SpawnCharacter()
    {
        m_characterFactory.SpawnCharacter(new Vector3( 0,0,0), m_characterFactory.GetRecruitmentData());
    }

    public void AddResources()
    {
        m_settlementModeManager.AddResource(ResourceType.Wood, 20);
        m_settlementModeManager.AddResource(ResourceType.Stone, 20);
        m_settlementModeManager.AddResource(ResourceType.Iron, 20);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
