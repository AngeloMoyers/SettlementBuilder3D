using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollPanelScript : MonoBehaviour
{
    [SerializeField] GameObject m_contentContainer;
    [SerializeField] GameObject m_buttonPrefab;

    BuildModeManager m_buildModeManager;

    List<GameObject> m_children = new List<GameObject>();

    void Start()
    {
        m_buildModeManager = GameObject.Find("BuildManager").GetComponent<BuildModeManager>();

        foreach (Transform t in transform)
        {
            m_children.Add(t.gameObject);
        }
    }

    void Update()
    {
        
    }
    
    public void AddBuildableObjectToContent(GameObject obj)
    {
        BuildableObject buildable = obj.GetComponentInChildren<BuildableObject>();
        GameObject newButton = SpawnButton(obj, buildable);
        if (newButton != null)
            m_children.Add(newButton);
    }

    private GameObject SpawnButton(GameObject prefab, BuildableObject obj)
    {
        if (obj == null) return null;

        GameObject newButton = Instantiate(m_buttonPrefab, m_contentContainer.transform);
        BuildableObjectButtonScript BOBScript = newButton.GetComponent<BuildableObjectButtonScript>();
        newButton.name = obj.name;
        BOBScript.SetImage(obj.sprite);

        BOBScript.SetTooltip(obj.GetName(), obj.GetInfo(), obj.resources);
        
        Button button = newButton.GetComponent<Button>();
        button.onClick.AddListener(delegate { SetActiveObject(prefab); });

        return newButton;
    }

    void SetActiveObject(GameObject prefab)
    {
        m_buildModeManager.SetActiveObject(prefab);
    }
}
