using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecruitmentBoardScript : MonoBehaviour
{
    [SerializeField] GameObject m_characterPanelPrefab;
    [SerializeField] GameObject m_contentContainer;

    //Managers
    CharacterFactory m_characterFactory;

    private void Awake()
    {
        m_characterFactory = GameObject.Find("CharacterManager").GetComponent<CharacterFactory>();
    }

    private void Start()
    {
        GenerateRecruitmentOptions();
    }

    public void GenerateRecruitmentOptions()
    {
        var charData = m_characterFactory.GetRecruitmentData();
        var panel = Instantiate(m_characterPanelPrefab, m_contentContainer.transform);
        panel.GetComponent<RecruitmentCharacterPanelScript>().SetData(charData);

        charData = m_characterFactory.GetRecruitmentData();
        panel = Instantiate(m_characterPanelPrefab, m_contentContainer.transform);
        panel.GetComponent<RecruitmentCharacterPanelScript>().SetData(charData);

        charData = m_characterFactory.GetRecruitmentData();
        panel = Instantiate(m_characterPanelPrefab, m_contentContainer.transform);
        panel.GetComponent<RecruitmentCharacterPanelScript>().SetData(charData);
    }

    public void Close()
    {
        Destroy(this.gameObject);
    }
}
