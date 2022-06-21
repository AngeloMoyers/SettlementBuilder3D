using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneScript : WorldObjectBase
{
    [SerializeField] ResourceCost m_resourceReward;
    [SerializeField] int m_gatherLimit;

    int m_timesGathered = 0;

    //Managers
    SettlementModeManager m_settlementManager;
    TileManager m_tileManager;

    private void Awake()
    {
        m_settlementManager = GameObject.Find("SettlementManager").GetComponent<SettlementModeManager>();
        m_tileManager = GameObject.Find("TileManager").GetComponent<TileManager>();
    }

    public override bool Use(CharacterBase user)
    {
        if (user == null)
        {
            MessagePrinter.DisplayMessage("You need to select a character to use this!", Color.red);
            return false;
        }

        if (!IsANeighbor(user.gameObject))
        {
            MessagePrinter.DisplayMessage("Out of range!", Color.red);
            return false;
        }
        else
            return Gather();
    }

    private bool Gather()
    {
        m_settlementManager.AddResource(m_resourceReward.type, m_resourceReward.cost);
        m_timesGathered++;

        if (m_timesGathered >= m_gatherLimit)
        {
            Despawn();
            return true;
        }
        return false;
    }
}
