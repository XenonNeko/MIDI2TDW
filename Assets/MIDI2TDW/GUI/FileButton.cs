using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FileButton : MonoBehaviour
{
    [SerializeField]
    private FileSelection fileSelection;
    [SerializeField]
    private Button button;
    [SerializeField]
    private TextMeshProUGUI filename;
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private float baseOffset;
    [SerializeField]
    private float size;

    public void SetOffset(float offset)
    {
        float inset = offset + baseOffset;
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, inset, size);
    }

    private int index;
    public void SetIndexAndText(int index, string text)
    {
        this.index = index;
        filename.text = text;
    }

    public void Click()
    {
        fileSelection.ButtonClicked(index);
    }

    public void SetButtonActive(bool value)
    {
        button.SetActive(value);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
