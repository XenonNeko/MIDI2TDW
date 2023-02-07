using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackedLayout : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform;

    private readonly List<RectTransform> rects = new();

    public void Clear()
    {
        rects.Clear();
    }

    private void OnLayoutChanged()
    {
        float runningInset = 0f;
        foreach (RectTransform rect in rects)
        {
            float height = rect.rect.height;
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, runningInset, height);
            runningInset += height;
        }
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, runningInset);
    }

    public void AddRect(IStackedRect rect)
    {
        rect.LayoutAwake(OnLayoutChanged);
        rects.Add(rect.GetRect());
    }

    public void Finish()
    {
        OnLayoutChanged();
    }
}
