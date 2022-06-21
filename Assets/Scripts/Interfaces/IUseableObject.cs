using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUseableObject
{
    //Should return true if the object is consumed, false if not
    public bool Use(CharacterBase user);
}
