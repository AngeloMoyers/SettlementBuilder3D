using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputMode : MonoBehaviour
{
    protected bool m_isActive = false;
    public abstract void HandleInput();

    public abstract void SetActive(bool newState);
}
