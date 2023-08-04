using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class FileSelection : MonoBehaviour
{
    [SerializeField]
    private FileButton fileTemplate;
    [SerializeField]
    private GameObject noneFoundMessage;

    [SerializeField]
    private Button previousButton;
    [SerializeField]
    private TextMeshProUGUI pageNum;
    [SerializeField]
    private Button nextButton;

    private int currentPage;

    public void PreviousPage()
    {
        currentPage--;
        DrawPage(currentPage);
    }
    public void NextPage()
    {
        currentPage++;
        DrawPage(currentPage);
    }

    public void ButtonClicked(int index)
    {
        int pathNdx = (currentPage * filesPerPage) + index;
        TryOpenFile(paths[pathNdx]);
    }

    [SerializeField]
    private ErrorMessage errorMsg;

    public void SetButtonsActive(bool value)
    {
        previousButton.SetActive(value);
        nextButton.SetActive(value);
        for (int i = 0; i < fileButtonCount; i++)
        {
            fileButtons[i].SetButtonActive(value);
        }
    }

    [SerializeField]
    private TracksScreen tracksScreen;

    [SerializeField]
    private GameObject loadingScreen;

    private Task<MidiTrack[]> importTask;
    private bool isAwaitingImportTask;
    private string importedFilename;

    [SerializeField]
    private Settings settings;

    private void ImportFile(string path)
    {
        importedFilename = Path.GetFileNameWithoutExtension(path);
        isAwaitingImportTask = true;

        Debug.Log("Importing file...");
        MidiFileImporter.debugPath = Path.Combine(Application.streamingAssetsPath, "debug");
        importTask = Task.Run(() => MidiFileImporter.TryImportMidiFile(path, settings.ReadingSettings));

        importTask.ConfigureAwait(false);
    }

    private void ImportFinished()
    {
        Debug.Log("Import thread finished. Results:");
        Debug.Log($"diag_elapsed_midiread: {MidiFileImporter.diag_elapsed_midiread}," +
            $" diag_elapsed_midisplit: {MidiFileImporter.diag_elapsed_midisplit}," +
            $" diag_elapsed_midiprocess: {MidiFileImporter.diag_elapsed_midiprocess}");
        MidiTrack[] tracks = importTask.Result;
        loadingScreen.SetActive(false);
        if (tracks is null)
        {
            if(MidiFileImporter.HasException)
            {
                Debug.Log("Importer caught an exception.");
                errorMsg.SetMessage(MidiFileImporter.LastException.ToString());
            }
            else
            {
                Debug.Log("Importer returned with no result.");
                errorMsg.SetMessage("Importer returned with no result.");
            }
            errorMsg.Open();
            return;
        }

        Debug.Log("File imported successfully.");

        tracksScreen.SetFilename(importedFilename);
        tracksScreen.SupplyTracks(tracks);
        tracksScreen.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    [SerializeField]
    private Diagnostics diagnostics;

    public void TryOpenFile(string path)
    {
        Debug.Log($"User opened '{Path.GetFileName(path)}'.");

        SetButtonsActive(false);
        if (!File.Exists(path))
        {
            Debug.Log("File does not exist.");
            errorMsg.SetMessage("File does not exist.");
            errorMsg.Open();
            return;
        }

        diagnostics.SnapshotFile(path, "user-selected-midi.mid");

        loadingScreen.SetActive(true);
        ImportFile(path);
    }

    [SerializeField]
    private float spacing;
    [SerializeField]
    private int filesPerPage;

    private string[] filenames;

    private int fileButtonCount;
    private FileButton[] fileButtons;

    private void ClearPage()
    {
        for (int i = 0; i < fileButtonCount; i++)
        {
            Destroy(fileButtons[i].gameObject);
        }
    }

    private void DrawPage(int page)
    {
        ClearPage();
        previousButton.gameObject.SetActive(page > 0);
        int offset = page * filesPerPage;
        fileButtonCount = filenames.Length - offset;
        nextButton.gameObject.SetActive(fileButtonCount > filesPerPage);
        fileButtonCount = Mathf.Min(fileButtonCount, filesPerPage);
        for (int i = 0; i < fileButtonCount; i++)
        {
            FileButton fileButton = Instantiate(fileTemplate, fileTemplate.transform.parent);
            fileButton.SetOffset(i * spacing);
            fileButton.SetIndexAndText(i, filenames[offset + i]);
            fileButton.gameObject.SetActive(true);
            fileButtons[i] = fileButton;
        }
        noneFoundMessage.SetActive(fileButtonCount == 0);

        int pages = Mathf.CeilToInt((float)filenames.Length / filesPerPage);
        pageNum.text = $"Page {page + 1} of {pages}";
    }

    private string[] paths;
    public void GetFiles()
    {
        Debug.Log("Getting files in 'StreamingAssets/in'.");
        string path = Path.Combine(Application.streamingAssetsPath, "in");
        if (!Directory.Exists(path))
        {
            Debug.Log("'StreamingAssets/in' does not exist, creating it.");
            Directory.CreateDirectory(path);
        }
        paths = Directory.GetFiles(path);
        Debug.Log($"Found {paths.Length} files in 'StreamingAssets/in'. Searching for MIDI files...");
        List<string> list = new();
        foreach (string filePath in paths)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            if (extension is not ".mid" and not ".midi")
            {
                continue;
            }
            list.Add(filePath);
        }
        paths = list.ToArray();
        filenames = new string[paths.Length];
        Debug.Log($"Found {filenames.Length} MIDI files in 'StreamingAssets/in'.");
        for (int i = 0; i < paths.Length; i++)
        {
            filenames[i] = Path.GetFileName(paths[i]);
        }
        currentPage = 0;
        DrawPage(0);
    }

    public void OpenInFolder()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "in");
        path = path.Replace(@"/", @"\");
        System.Diagnostics.Process.Start("explorer.exe", "/root," + path);
    }

    private void Awake()
    {
        fileButtons = new FileButton[filesPerPage];
    }

    // Start is called before the first frame update
    void Start()
    {
        GetFiles();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAwaitingImportTask)
        {
            return;
        }
        if (importTask.IsCompleted)
        {
            isAwaitingImportTask = false;
            ImportFinished();
        }
    }
}
