using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Logger : MonoBehaviour
{
    [SerializeField]
    private VersionCheck versionCheck;

    public static string TimeToFilename(DateTime time)
    {
        return $"{time.Year}-{time.Month}-{time.Day}_{time.Hour}-{time.Minute}-{time.Second}";
    }

    private StreamWriter logStream;
    private string logFilePath;

    public string GetLogText()
    {
        string text;
        logStream.Close();
        using (var reader = File.OpenText(logFilePath))
        {
            text = reader.ReadToEnd();
        }
        logStream = new StreamWriter(File.OpenWrite(logFilePath));
        return text;
    }

    private void CreateLog()
    {
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }
        string logsDirectory = Path.Combine(Application.streamingAssetsPath, "logs");
        if (!Directory.Exists(logsDirectory))
        {
            Directory.CreateDirectory(logsDirectory);
        }
        DateTime now = DateTime.Now;
        string filename = TimeToFilename(now);
        logFilePath = Path.Combine(logsDirectory, $"{filename}.log");
        logStream = File.CreateText(logFilePath);
    }

    private void Awake()
    {
        Application.logMessageReceived += Application_logMessageReceived;
        CreateLog();
        var versionInfo = versionCheck.GetVersion();
        Debug.Log($"MIDI2TDW {versionInfo.ver} ({versionInfo.date})\r\nLog created at {DateTime.Now}.");
    }

    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        logStream.WriteLine($"[{type}]");
        logStream.WriteLine(condition);
        if (type != LogType.Log)
        {
            logStream.WriteLine(stackTrace);
        }
        logStream.WriteLine();
    }

    private void OnDestroy()
    {
        logStream.WriteLine($"Logger is closing.");
        logStream.Close();
    }
}
