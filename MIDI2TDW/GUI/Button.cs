using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class Button : MonoBehaviour
{
    [SerializeField]
    private bool m_Interactable = true;
    public bool Interactable
    {
        get
        {
            return m_Interactable;
        }
        set
        {
            m_Interactable = value;
            SetInteractable(m_Interactable);
        }
    }

    [SerializeField]
    private float highlightedScale;
    [SerializeField]
    private float pressedScale;
    [SerializeField]
    private float disabledOpacity;

    private Graphic[] graphics;
    private float[] alphas;

    private void GetGraphics()
    {
        graphics = GetComponentsInChildren<Graphic>();
        alphas = new float[graphics.Length];
        for (int i = 0; i < graphics.Length; i++)
        {
            alphas[i] = graphics[i].color.a;
        }
    }

    private void SetInteractable(bool value)
    {
        for (int i = 0; i < graphics.Length; i++)
        {
            Color color = graphics[i].color;
            color.a = value ? alphas[i] : disabledOpacity;
            graphics[i].color = color;
        }
    }

    private RectTransform rectTransform;

    private void Awake()
    {
        GetGraphics();
        rectTransform = GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentScale = 1f;
        progress = 1f;
        sourceScale = 1f;
        targetScale = 1f;
    }

    [SerializeField]
    private float animateSpeed;

    private bool mouseWasDown;
    private float currentScale;
    private float progress;

    private float sourceScale;
    private float targetScale;

    private void UpdateAnimate()
    {
        currentScale = Mathf.Lerp(sourceScale, targetScale, progress);
        transform.localScale = Vector3.one * currentScale;

        progress += Time.deltaTime * (1f / animateSpeed);
    }

    public void OnPointerEnter()
    {
        if (!m_Interactable)
        {
            return;
        }
        if (!isActive)
        {
            return;
        }
        sourceScale = 1f;
        targetScale = highlightedScale;
        progress = Mathf.InverseLerp(sourceScale, targetScale, currentScale);
    }

    public void OnPointerExit()
    {
        if (!m_Interactable)
        {
            return;
        }
        if (!isActive)
        {
            return;
        }
        sourceScale = mouseWasDown ? pressedScale : highlightedScale;
        targetScale = 1f;
        progress = Mathf.InverseLerp(sourceScale, targetScale, currentScale);
    }

    public void OnPointerDown()
    {
        if (!m_Interactable)
        {
            return;
        }
        if (!isActive)
        {
            return;
        }
        sourceScale = highlightedScale;
        targetScale = pressedScale;
        mouseWasDown = true;
        progress = Mathf.InverseLerp(sourceScale, targetScale, currentScale);
    }

    public void OnPointerUp()
    {
        if (!m_Interactable)
        {
            return;
        }
        if (!isActive)
        {
            return;
        }
        sourceScale = pressedScale;
        targetScale = highlightedScale;
        mouseWasDown = false;
        progress = Mathf.InverseLerp(sourceScale, targetScale, currentScale);
        onClick.Invoke();
    }

    [SerializeField]
    private UnityEvent onClick;

    private bool isActive = true;
    public void SetActive(bool value)
    {
        isActive = value;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimate();
    }
}
