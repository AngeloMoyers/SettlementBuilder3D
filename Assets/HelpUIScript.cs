using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpUIScript : MonoBehaviour
{
    public void ToggleActive()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active); 
    }
}
