using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildableObjectButtonScript : MonoBehaviour
{
    [SerializeField] Image m_image;

    HoverTooltip m_tooltip;

    private void Awake()
    {
        m_tooltip = GetComponent<HoverTooltip>();
    }

    public void SetImage(Texture2D sprite)
    {
        if (sprite == null) return;
        m_image.overrideSprite = Sprite.Create(sprite, new Rect(0,0,sprite.width, sprite.height), new Vector2(0.5f, 0.5f));
    }

    public void SetTooltip(string name, string info, ResourceCost[] costs)
    {
        m_tooltip.UpdateInfo(name, info, costs);
    }
}
