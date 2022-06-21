using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct GameModeToIcon
{
    public InputModeType mode;
    public Sprite sprite;
}
public class CurrentModeIconScript : MonoBehaviour
{
    [SerializeField] GameModeToIcon[] m_modeIcons;
    Dictionary<InputModeType, GameModeToIcon> m_modeIconsMap = new Dictionary<InputModeType, GameModeToIcon>();

    [SerializeField] Image m_iconSprite;

    private void Awake()
    {
        foreach (var m in m_modeIcons)
        {
            m_modeIconsMap.Add(m.mode, m);
        }
    }


    public void SetModeIcon(InputModeType mode)
    {
        m_iconSprite.sprite = m_modeIconsMap[mode].sprite;
    }
}