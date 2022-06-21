using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuldableObjectFactory : MonoBehaviour
{
    [SerializeField] GameObject[] m_allBuildableObjects;

    BuildModeUI m_buildModeUI;

    void Start()
    {
        m_buildModeUI = GameObject.Find("BuildModeUI").GetComponent<BuildModeUI>();

        AssignIDs();

        PopulateBuildModeUI();
    }

    private void AssignIDs()
    {
        int currentID = 0;
        foreach (var obj in m_allBuildableObjects)
        {
            var script = obj.GetComponentInChildren<BuildableObject>();
            if (script != null)
            {
                script.ID = currentID;
            }

            currentID++;
        }
    }

    private void PopulateBuildModeUI()
    {
        foreach (var obj in m_allBuildableObjects)
        { 
            m_buildModeUI.AddObjectToScrollPanel(obj);
        }
    }
}
