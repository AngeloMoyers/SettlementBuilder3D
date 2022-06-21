using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject m_tooltipPanelPrefab;

    GameObject m_tooltipPanel;
    TooltipPanelScript m_tooltipScript;

    string m_name;
    string m_info;
    ResourceCost[] m_costs;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Spawn tooltip
        m_tooltipPanel = Instantiate(m_tooltipPanelPrefab, this.transform);
        m_tooltipScript = m_tooltipPanel.GetComponent<TooltipPanelScript>();
        m_tooltipScript.UpdateInfo(m_name, m_info, m_costs);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Destroy tooltip
        Destroy(m_tooltipPanel);
        m_tooltipPanel = null;
        m_tooltipScript = null;
    }

    public void UpdateInfo(string name, string info, ResourceCost[] costs)
    {
        m_name = name;
        m_info = info;
        m_costs = costs;
    }
}
