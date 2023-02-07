using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using B83.Win32;


public class FileDragAndDrop : MonoBehaviour
{
    [SerializeField]
    private FileSelection fileSelection;

    void OnEnable()
    {
        // must be installed on the main thread to get the right thread id.
        UnityDragAndDropHook.InstallHook();
        UnityDragAndDropHook.OnDroppedFiles += OnFiles;
    }

    void OnDisable()
    {
        UnityDragAndDropHook.UninstallHook();
    }

    void OnFiles(List<string> aFiles, POINT aPos)
    {
        // do something with the dropped file names. aPos will contain the 
        // mouse position within the window where the files has been dropped.
        string str = "Dropped " + aFiles.Count + " files at: " + aPos + "\n\t" +
            aFiles.Aggregate((a, b) => a + "\n\t" + b);
        Debug.Log(str);

        if (!fileSelection.gameObject.activeSelf)
        {
            Debug.Log("File drop ignored because file selection screen was not open.");
            return;
        }

        string path = aFiles.First();
        fileSelection.TryOpenFile(path);
    }
}