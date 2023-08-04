using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class HoverRect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Events")]
    [SerializeField]
    private UnityEvent onPointerEnter;
    [SerializeField]
    private UnityEvent onPointerExit;
    [SerializeField]
    private UnityEvent onPointerDown;
    [SerializeField]
    private UnityEvent onPointerUp;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter.Invoke();
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        onPointerExit.Invoke();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        onPointerDown.Invoke();
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        onPointerUp.Invoke();
    }

    public void Deselect()
    {
        onPointerExit.Invoke();
    }
}
