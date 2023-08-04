using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class App : MonoBehaviour
{
    [SerializeField]
    private Settings settings;
    [SerializeField]
    private Sounds sounds;
    [SerializeField]
    private DefaultSounds defaultSounds;
    [SerializeField]
    private SoundSelect soundSelect;
    [SerializeField]
    private FileSelection fileSelection;

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

    [DllImport("user32.dll", EntryPoint = "SetWindowText")]
    private static extern bool SetWindowText(IntPtr hwnd, string lpString);

    public static void SetWindowTitle(string title)
    {
        SetWindowText(GetActiveWindow(), title);
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    private const int SW_SHOWMAXIMIZED = 3;

    private void Awake()
    {
//#if !UNITY_EDITOR
        ShowWindow(GetActiveWindow(), SW_SHOWMAXIMIZED);
//#endif
    }

    public void Main(string[] args)
    {
        Debug.Log("Application was passed the following command line arguments:");
        Debug.Log("[BEGIN COMMAND LINE ARGUMENTS]");
        foreach (string arg in args)
        {
            Debug.Log(arg);
        }
        Debug.Log("[END COMMAND LINE ARGUMENTS]");

#if !UNITY_EDITOR
        if (args.Length < 2)
        {
            return;
        }
        Debug.Log($"Attempting to open path passed as command line argument...");
        string path = args[1];
        fileSelection.TryOpenFile(path);
#endif
    }

    private void Start()
    {
        Main(Environment.GetCommandLineArgs());

        Debug.Log("Performing startup tasks...");
        settings.LoadAppSettings();
        settings.LoadReadingSettings();
        sounds.Load();
        defaultSounds.Load();
        soundSelect.DrawButtons();
    }

    private Action action;
    private float delay;
    private IEnumerator DoAfterCoroutine()
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }

    public void DoAfter(Action action, float delay)
    {
        this.action = action;
        this.delay = delay;
        StartCoroutine(DoAfterCoroutine());
    }
}
