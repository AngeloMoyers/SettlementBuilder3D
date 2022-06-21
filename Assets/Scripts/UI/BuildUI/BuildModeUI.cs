using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildModeUI : MonoBehaviour
{
    ScrollPanelScript m_scrollPanel;

    private void Awake()
    {
        m_scrollPanel = GetComponent<ScrollPanelScript>();
    }

    private void Start()
    {
        //Deactivate at Start, after other gameobjects have linked to it in Awake
        gameObject.SetActive(false);
    }

    public void AddObjectToScrollPanel(GameObject obj)
    {
        m_scrollPanel.AddBuildableObjectToContent(obj);
    }

    public void SetActive(bool newState)
    {
        gameObject.SetActive(newState);
    }
}
