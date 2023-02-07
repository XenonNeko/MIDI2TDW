using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class VersionCheck : MonoBehaviour
{
    [SerializeField]
    private string versionPrefix;
    [SerializeField]
    private VersionInfo currentVersion;

    public VersionInfo GetVersion()
    {
        return currentVersion;
    }

    [SerializeField]
    private VersionBadge versionBadge;

    [Serializable]
    public class VersionInfo
    {
        public string ver;
        public string date;
        public string dl;
    }

    [SerializeField]
    private string url = "https://xenonneko.gamejolt.io/midi2tdw/latest.json";

    private IEnumerator CheckForNewerVersion()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    ProcessResponse(webRequest.downloadHandler.text);
                    break;
                default:
                    versionBadge.SetError($"Update check failed.");
                    break;
            }
        }
    }

    private string download;

    private void ProcessResponse(string text)
    {
        VersionInfo latestVersion = JsonConvert.DeserializeObject<VersionInfo>(text);
        if (latestVersion.ver != currentVersion.ver)
        {
            versionBadge.SetOutOfDate();
            download = latestVersion.dl;
            return;
        }
        versionBadge.SetUpToDate();
    }

    // Start is called before the first frame update
    void Awake()
    {
        currentVersion.ver = versionPrefix + Application.version;
        versionBadge.SetVersion(currentVersion.ver, currentVersion.date);
        StartCoroutine(CheckForNewerVersion());
    }

    public void DownloadLatest()
    {
        Application.OpenURL(download);
    }
}
