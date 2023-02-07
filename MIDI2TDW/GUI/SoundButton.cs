using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    [SerializeField]
    private SoundSelect soundSelect;
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private Graphic highlight;
    [SerializeField]
    private float iconSize;
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private RectTransform iconRect;

    public void SetInsetAndSizeFromParentEdge(RectTransform.Edge edge, float inset, float size)
    {
        rectTransform.SetInsetAndSizeFromParentEdge(edge, inset, size);
    }

    private void SetIcon(Sprite icon)
    {
        iconImage.sprite = icon;
        Rect rect = icon.rect;
        if (rect.width >= rect.height)
        {
            float scale = iconSize / rect.width;
            iconRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, iconSize);
            iconRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rect.height * scale);
        }
        else
        {
            float scale = iconSize / rect.height;
            iconRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.width * scale);
            iconRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, iconSize);
        }
    }

    private TdwSound sound;
    public void SetSound(TdwSound tdwSound)
    {
        sound = tdwSound;
        SetIcon(tdwSound.icon);
    }

    [SerializeField]
    private HoverRect hoverRect;
    public void OnClick()
    {
        hoverRect.Deselect();
        soundSelect.SoundSelected(sound);
    }

    public void SetHighlightEnabled(bool value)
    {
        highlight.enabled = value;
        if (value)
        {
            soundSelect.HoverSound(sound);
            return;
        }
        soundSelect.StopHoverSound(sound);
    }
}
