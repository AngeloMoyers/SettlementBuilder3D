using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettlementUIScript : MonoBehaviour
{
    TextMeshProUGUI m_woodCountText;
    TextMeshProUGUI m_stoneCountText;
    TextMeshProUGUI m_ironCountText;

    TextMeshProUGUI m_populationCountText;
    TextMeshProUGUI m_foodCountText;
    TextMeshProUGUI m_happinessCountText;

    private void Awake()
    {
        m_woodCountText = GameObject.Find("WoodCountText").GetComponent<TextMeshProUGUI>();
        m_stoneCountText = GameObject.Find("StoneCountText").GetComponent<TextMeshProUGUI>();
        m_ironCountText = GameObject.Find("IronCountText").GetComponent<TextMeshProUGUI>();

        m_populationCountText = GameObject.Find("PopulationCountText").GetComponent<TextMeshProUGUI>();
        m_foodCountText = GameObject.Find("FoodCountText").GetComponent<TextMeshProUGUI>();
        m_happinessCountText = GameObject.Find("HappinessCountText").GetComponent<TextMeshProUGUI>();
    }

    public void UpdateValues(int wood, int stone, int iron)
    {
        m_woodCountText.text = wood.ToString();
        m_stoneCountText.text = stone.ToString();
        m_ironCountText.text = iron.ToString();
    }

    public void UpdateStats(int pop, int popMax, int food, int foodMax, int happiness)
    {
        m_populationCountText.text = pop.ToString() + "/" + popMax.ToString();
        m_foodCountText.text = food.ToString() + "/" + foodMax.ToString();
        m_happinessCountText.text = happiness.ToString();
    }
}
