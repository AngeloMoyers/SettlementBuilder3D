using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PortaitUIScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_nameText;
    [SerializeField] Image m_portraitImage;

    PortraitCamera m_portraitCam;

    private void Awake()
    {
        m_portraitCam = GameObject.Find("PortraitCam").GetComponent<PortraitCamera>();
    }


    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowPortrait(CharacterBase character)
    {
        gameObject.SetActive(true);
        m_nameText.text = character.GetData().name;

        m_portraitCam.TakePortrait(character);
    }

    public void ShowPortrait()
    {
        if (m_nameText.text != "" && m_nameText.text != "Name")
        {
            gameObject.SetActive(true);
        }
    }

    public void HidePortrait()
    {
        gameObject.SetActive(false);
    }
}
