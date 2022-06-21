using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettlementData
{
    public int m_population;
    public int m_populationMax;

    public int m_food;
    public int m_foodMax;

    public int m_happiness;

    public Dictionary<ResourceType, int> m_resourceCounts = new Dictionary<ResourceType, int>();


    public bool AddFood(int amount)
    {
        if (m_food + amount > m_foodMax || m_food + amount < 0)
            return false;

        m_food += amount;
        if (m_food > m_foodMax)
            m_food = m_foodMax;

        return true;
    }

    public void AddFoodMax(int amount)
    {
        m_foodMax += amount;
    }

    public void AddPopulation(int amount)
    {
        m_population += amount;
        if (m_population > m_populationMax)
            m_population = m_populationMax;
    }

    public void AddPopulationMax(int amount)
    {
        m_populationMax += amount;
    }
}
