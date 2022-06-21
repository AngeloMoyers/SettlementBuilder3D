using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SettlementRoleToAIScript
{
    public SettlementRole role;
    public AIModeBase script;
}
[System.Serializable]
public struct CombatRoleToAIScript
{
    public CombatRole role;
    public AIModeBase script;
}

public class CharacterFactory : MonoBehaviour
{
    [SerializeField] GameObject m_characterBasePrefab;

    [SerializeField] SettlementRoleToAIScript[] m_settlementRolesToScripts;
    [SerializeField] CombatRoleToAIScript[] m_combatRolesToScripts;
    
    Dictionary<SettlementRole, AIModeBase> m_settlementRoles = new Dictionary<SettlementRole, AIModeBase>();
    Dictionary<CombatRole, AIModeBase> m_combatRoles = new Dictionary<CombatRole, AIModeBase>();

    //Managers
    CharacterManager m_characterManager;
    TileManager m_tileManager;
    SettlementModeManager m_settlementManager;

    private void Awake()
    {
        m_characterManager = GetComponent<CharacterManager>();
        m_tileManager = GameObject.Find("TileManager").GetComponent<TileManager>();
        m_settlementManager = GameObject.Find("SettlementManager").GetComponent<SettlementModeManager>();
    }

    private void Start()
    {
        foreach (var v in m_settlementRolesToScripts)
        {
            m_settlementRoles.Add(v.role, v.script);
        }
        m_settlementRoles.Add(SettlementRole.None, null);


        foreach (var v in m_combatRolesToScripts)
        {
            m_combatRoles.Add(v.role, v.script);
        }
        m_combatRoles.Add(CombatRole.None, null);
    }

    public bool SpawnCharacter(Vector3 pos, CharacterData data, SettlementRole settlementRole = SettlementRole.None, CombatRole combatRole = CombatRole.None)
    {
        if (m_settlementManager.GetPopulation() >= m_settlementManager.GetPopulationMax())
        {
            MessagePrinter.DisplayMessage("There aren't enough beds in this settlemnt!", Color.red);
            return false; ;
        }

        WorldTile tileAtPosition = (m_tileManager.GetWorldTile(pos));
        if (tileAtPosition.isWalkable)
        {
            GameObject ch = Instantiate(m_characterBasePrefab.gameObject, pos, Quaternion.identity);
            CharacterBase charBase = ch.GetComponent<CharacterBase>();
            AICharacterBase aiBase =  ch.GetComponent<AICharacterBase>();

            charBase.SetData(data);
            aiBase.AddAIMode(AIMode.Settlement, m_settlementRoles[settlementRole]);
            aiBase.AddAIMode(AIMode.Combat, m_combatRoles[combatRole]);

            return true;
        }
        else
        {
            MessagePrinter.DisplayMessage("Cannot spawn a character here!", Color.red);
            return false;
        }
    }

    public CharacterData GetRecruitmentData() //TODO
    {
        CharacterData data = new CharacterData();

        data.name = "Arthurius Or Sum'n";
        data.level = Random.Range(1, 10);
        data.health = data.healthMax;
        data.healthMax = Random.Range(1, 20);
        data.energy = data.energyMax;
        data.energyMax = Random.Range(1, 20);

        data.stats = new CharacterStats(Random.Range(1,10), Random.Range(1, 10), Random.Range(1, 10), Random.Range(1, 10), Random.Range(1, 10));

        return data;
    }
}
