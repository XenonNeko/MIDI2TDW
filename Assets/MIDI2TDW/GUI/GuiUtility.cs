using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GuiUtility
{

    private static readonly Vector3[] fourCornersArray = new Vector3[4];
    //private static Camera cam;
    public static bool ContainsScreenPoint(this RectTransform rect, Vector2 screenPoint)
    {
        //if (!cam)
        //{
        //    Canvas canvas = rect.root.GetComponent<Canvas>();
         //   cam = canvas.worldCamera;
        //}
        rect.GetWorldCorners(fourCornersArray);
        //Vector2 min = cam.WorldToScreenPoint(fourCornersArray[0]);
        //Vector2 max = cam.WorldToScreenPoint(fourCornersArray[2]);
        Vector2 min = fourCornersArray[0];
        Vector2 max = fourCornersArray[2];
        Rect r = new()
        {
            min = min,
            max = max
        };
        return r.Contains(screenPoint);
    }
}
