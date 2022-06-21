using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public struct ResourceTypeToIcon
{
    public ResourceType type;
    public Sprite icon;
}
public class TooltipPanelScript : MonoBehaviour
{
    
    [SerializeField] ResourceTypeToIcon[] m_resourceIcons;
    Dictionary<ResourceType, ResourceTypeToIcon> m_resourceIconMap = new Dictionary<ResourceType, ResourceTypeToIcon>();

    [SerializeField] TextMeshProUGUI m_nameText;
    [SerializeField] TextMeshProUGUI m_infoText;
    [SerializeField] Image[] m_costImages;
    [SerializeField] TextMeshProUGUI[] m_costTexts;

    private void Awake()
    {
        foreach (var r in m_resourceIcons)
        {
            m_resourceIconMap.Add(r.type, r);
        }
    }


    public void UpdateInfo(string name, string info, ResourceCost[] costs)
    {
        m_nameText.text = name;
        m_infoText.text = info;

        int index = 0;
        foreach (var c in costs)
        {
            m_costImages[index].gameObject.SetActive(true);
            m_costTexts[index].gameObject.SetActive(true);

            m_costImages[index].sprite = m_resourceIconMap[c.type].icon;
            m_costTexts[index].text = c.cost.ToString();
            index++;
        }
    }
}
