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
    public class VersionInfo : IComparable<VersionInfo>
    {
        public string ver;
        public string date;
        public string dl;

        public int CompareTo(VersionInfo other)
        {
            var date = DateTime.Parse(this.date);
            var otherDate = DateTime.Parse(other.date);
            return date.CompareTo(otherDate);
        }

        public static bool operator <(VersionInfo a, VersionInfo b)
        {
            return a.CompareTo(b) < 0;
        }
        public static bool operator >(VersionInfo a, VersionInfo b)
        {
            return a.CompareTo(b) > 0;
        }
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
        if (currentVersion < latestVersion)
        {
            versionBadge.SetOutOfDate();
            download = latestVersion.dl;
            return;
        }
        else if (currentVersion > latestVersion)
        {
            versionBadge.SetDev();
            return;
        }
        else
        {
            versionBadge.SetUpToDate();
        }
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
