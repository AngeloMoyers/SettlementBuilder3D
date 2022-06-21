using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RecruitmentBoard : StationBase, IUseableObject
{
    [SerializeField] GameObject m_recruitmentUIPrefab;

    GameObject m_UIObject;
    public bool Use(CharacterBase user)
    {
        ShowRecruitmentUI();
        return true;
    }

    public override void Build(TileManager tileMan, Tilemap groundTM)
    {
        base.Build(tileMan, groundTM);
    }

    private void ShowRecruitmentUI()
    {
        m_UIObject = Instantiate(m_recruitmentUIPrefab, this.transform);
    }

    private void HideRecruitmentUI()
    {
        Destroy(m_UIObject);
        m_UIObject = null;
    }
}
