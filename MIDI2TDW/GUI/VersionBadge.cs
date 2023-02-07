using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VersionBadge : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI versionLabel;

    [SerializeField]
    private GameObject iconChecking;
    [SerializeField]
    private GameObject msgChecking;

    [SerializeField]
    private GameObject iconOutOfDate;
    [SerializeField]
    private TextMeshProUGUI msgOutOfDate;

    [SerializeField]
    private GameObject iconUpToDate;
    [SerializeField]
    private GameObject msgUpToDate;

    public void SetVersion(string name, string date)
    {
        versionLabel.text = $"{name}  -  <size=12><color=#BFBFBF>{date}";
    }

    public void SetOutOfDate()
    {
        iconChecking.SetActive(false);
        msgChecking.SetActive(false);

        iconOutOfDate.SetActive(true);
        msgOutOfDate.gameObject.SetActive(true);
    }

    public void SetUpToDate()
    {
        iconChecking.SetActive(false);
        msgChecking.SetActive(false);

        iconUpToDate.SetActive(true);
        msgUpToDate.SetActive(true);
    }

    public void SetError(string message)
    {
        iconChecking.SetActive(false);
        msgChecking.SetActive(false);

        iconOutOfDate.SetActive(true);
        msgOutOfDate.gameObject.SetActive(true);

        msgOutOfDate.text = message;
    }
}
