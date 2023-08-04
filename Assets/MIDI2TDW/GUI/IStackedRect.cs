using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStackedRect
{
    public delegate void LayoutChangedCallback();

    void LayoutAwake(LayoutChangedCallback layoutChangedCallback);

    RectTransform GetRect();
}
