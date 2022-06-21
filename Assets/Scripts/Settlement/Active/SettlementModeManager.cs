using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettlementModeManager : MonoBehaviour
{
    SettlementData m_stats;
    SettlementUIScript m_settlementUI;

    private void Awake()
    {
        m_settlementUI = GameObject.Find("SettlementUI").GetComponent<SettlementUIScript>();

        m_stats = new SettlementData();

        m_stats.m_population = 0;
        m_stats.m_populationMax = 0;
        m_stats.m_food = 5;
        m_stats.m_foodMax = 10;
        m_stats.m_happiness = 100;

        m_stats.m_resourceCounts.Add(ResourceType.Wood, 15);
        m_stats.m_resourceCounts.Add(ResourceType.Stone, 0);
        m_stats.m_resourceCounts.Add(ResourceType.Iron, 0);
    }

    void Start()
    {
        UpdateValues();
        UpdateStats();
    }

    void Update()
    {
        
    }

    public bool HasEnoughResources(ResourceType type, int cost)
    {
        if (m_stats.m_resourceCounts.ContainsKey(type))
        {
            if (m_stats.m_resourceCounts[type] >= cost)
            {
                return true;
            }
        }
        return false;
    }

    public void ChargeResource(ResourceType type, int cost)
    {
        if (m_stats.m_resourceCounts.ContainsKey(type))
        {
            if (m_stats.m_resourceCounts[type] >= cost)
            {
                m_stats.m_resourceCounts[type] -= cost;
            }
        }

        UpdateValues();
    }

    public void AddResource(ResourceType type, int amt)
    {
        if (m_stats.m_resourceCounts.ContainsKey(type))
        {
            m_stats.m_resourceCounts[type] += amt;
        }

        UpdateValues();
    }

    //Stats
    private void UpdateValues()
    {
        m_settlementUI.UpdateValues(m_stats.m_resourceCounts[ResourceType.Wood], m_stats.m_resourceCounts[ResourceType.Stone], m_stats.m_resourceCounts[ResourceType.Iron]);
    }
    private void UpdateStats()
    {
        m_settlementUI.UpdateStats(m_stats.m_population, m_stats.m_populationMax, m_stats.m_food, m_stats.m_foodMax, m_stats.m_happiness);
    }

    public bool AddFood(int amount)
    {
        if (m_stats.AddFood(amount))
        {
            UpdateStats();
            return true;
        }
        return false;
    }
    public void AddFoodMax(int amount)
    {
        m_stats.AddFoodMax(amount);

        UpdateStats();
    }
    public void AddPopulation(int amount)
    {
        m_stats.AddPopulation(amount);

        UpdateStats();
    }
    public void AddPopulationMax(int amount)
    {
        m_stats.AddPopulationMax(amount);

        UpdateStats();
    }

    public int GetPopulation()
    { return m_stats.m_population; }

    public int GetPopulationMax()
    { return m_stats.m_populationMax;}
}
