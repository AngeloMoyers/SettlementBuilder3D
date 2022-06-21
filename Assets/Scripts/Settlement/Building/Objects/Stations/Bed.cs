using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bed : StationBase
{
    SettlementModeManager m_settlementManager;

    public override void Build(TileManager tileMan, Tilemap groundTM)
    {
        base.Build(tileMan, groundTM);

        m_settlementManager = GameObject.Find("SettlementManager").GetComponent<SettlementModeManager>();
        m_settlementManager.AddPopulationMax(1);
    }

    public override void BeforeDestroy()
    {
        base.BeforeDestroy();

        m_settlementManager.AddPopulationMax(-1);
    }
}
