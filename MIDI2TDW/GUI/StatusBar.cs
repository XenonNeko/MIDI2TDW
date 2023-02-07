using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusBar : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI statusMessage;

    public void SetMessage(string message)
    {
        statusMessage.text = message;
    }
}
