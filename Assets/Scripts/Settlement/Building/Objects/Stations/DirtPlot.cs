using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DirtPlot : StationBase
{
    [SerializeField] int m_foodGenerationAmount;
    [SerializeField] float m_foodGenerationRate;


    SettlementModeManager m_settlementManager;

    bool m_isActive = false;

    //Timing
    float m_startTime;

    private void Update()
    {
        GenerateFood();
    }

    private void GenerateFood()
    {
        if (!m_isActive)
            return;

        if (Time.time > m_startTime + m_foodGenerationRate)
        {
            m_startTime = Time.time;

            m_settlementManager.AddFood(m_foodGenerationAmount);
        }
    }

    public override void Build(TileManager tileMan, Tilemap groundTM)
    {
        base.Build(tileMan, groundTM);

        m_settlementManager = GameObject.Find("SettlementManager").GetComponent<SettlementModeManager>();
        m_startTime = Time.time;

        m_isActive = true;
    }
}
