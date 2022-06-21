using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StationBase : BuildableObject
{
    //protected CharacterBase m_owner;

    //public void SetOwner(CharacterBase ch) { m_owner = ch; }
    //public void GetOwner() { return m_owner; }

    public override void Build(TileManager tileMan, Tilemap groundTM)
    {
        base.Build(tileMan, groundTM);
    }
}
