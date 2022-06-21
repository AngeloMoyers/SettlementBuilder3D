using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecruitmentCharacterPanelScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_nameText;
    [SerializeField] TextMeshProUGUI m_levelText;
    [SerializeField] TextMeshProUGUI m_healthText;
    [SerializeField] TextMeshProUGUI m_energyText;
    [SerializeField] TextMeshProUGUI m_constitutionText;
    [SerializeField] TextMeshProUGUI m_witsText;
    [SerializeField] TextMeshProUGUI m_agilityText;
    [SerializeField] TextMeshProUGUI m_strengthText;
    [SerializeField] TextMeshProUGUI m_intelligenceText;

    CharacterData m_characterData;

    public void SetData(CharacterData data)
    {
        m_characterData = data;

        m_nameText.text = data.name;
        m_levelText.text = "LV: " + data.level.ToString();
        m_healthText.text = "HP: " + data.healthMax.ToString();
        m_energyText.text = "EN: " + data.energyMax.ToString();
        m_constitutionText.text = "CON: " + data.stats.constitution.ToString();
        m_witsText.text = "WIT: " + data.stats.wits.ToString();
        m_agilityText.text = "AGI: " + data.stats.agility.ToString();
        m_strengthText.text = "STR: " + data.stats.strength.ToString();
        m_intelligenceText.text = "INT: " + data.stats.intelligence.ToString();
    }

    public void RecruitCharacter()
    {
        var factory = GameObject.Find("CharacterManager").GetComponent<CharacterFactory>();
        if (factory.SpawnCharacter(new Vector3(0,0,0), m_characterData))
            DeleteThis();
    }

    private void DeleteThis()
    {
        Destroy(this.gameObject);
    }
}
