using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClickedObjectUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_nameText;
    [SerializeField] TextMeshProUGUI m_assignedToText;
    [SerializeField] TextMeshProUGUI m_infoText;

    GameObject m_targetedObject;

    //Managers
    CharacterManager m_characterManager;

    private void Awake()
    {
        m_characterManager = GameObject.Find("CharacterManager").GetComponent<CharacterManager>();
    }

    private void Start()
    {
        //Deactivate at Start, after other gameobjects have linked to it in Awake
        gameObject.SetActive(false);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    public void Open(BuildableObject BOScript)
    {
        this.gameObject.SetActive(true);

        m_targetedObject = BOScript.gameObject;

        m_nameText.text = BOScript.GetName(); ;
        var owner = BOScript.GetOwner();
        if (owner != null)
            m_assignedToText.text = BOScript.GetOwner().name;
        else
            m_assignedToText.text = "None" ;

        m_infoText.text = "Info: " + BOScript.GetInfo(); ;
    }

    public void Open(WorldObjectBase wObjScript)
    {
        this.gameObject.SetActive(true);

        m_targetedObject = wObjScript.gameObject;

        m_nameText.text = wObjScript.GetName();
        m_assignedToText.text = "None";
        m_infoText.text = "Info: " + wObjScript.GetInfo(); ;
    }

    public void Assign()
    {
        Debug.Log("Assign");
    }

    public void Use()
    {
        var useable = m_targetedObject.GetComponent<IUseableObject>();
        if (useable != null)
        {
            var character = m_characterManager.GetActiveCharacter();

            if (character != null)
            {
                if (m_characterManager.GetActiveCharacter().UseObject(useable))
                    Close();
            }
            else
            {
                if (useable.Use(null))
                    Close();
            }
        }
        else
        {
            MessagePrinter.DisplayMessage("This object cannot be used!", Color.red);
        }
    }
}
