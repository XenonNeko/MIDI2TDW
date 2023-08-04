using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorMessage : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_InputField message;

    public void SetMessage(string text)
    {
        message.text = text;
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }
}
