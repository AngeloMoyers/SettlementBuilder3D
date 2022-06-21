using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class CharacterInputMode : MonoBehaviour
{
    //Data
    protected CharacterBase m_owner;
    protected bool m_isActive = false;

    //Functions
    public abstract void HandleInput();
    public abstract void SetActive(bool newState);
    public abstract void SetOwner(CharacterBase character);
}
