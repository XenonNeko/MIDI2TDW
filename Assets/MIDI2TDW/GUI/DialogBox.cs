using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogBox : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI message;

    private Action onClick;
    public void Prompt(string message, Action onClick = null)
    {
        this.message.text = message;
        this.onClick = onClick;
        gameObject.SetActive(true);
    }

    public void Click()
    {
        onClick?.Invoke();
        gameObject.SetActive(false);
    }
}
