using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class TracksScreen : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI filenameLabel;

    [SerializeField]
    private TrackGui trackTemplate;
    [SerializeField]
    private StackedLayout stackedLayout;

    private TrackGui[] trackGuis;

    private string filename;
    public void SetFilename(string filename)
    {
        this.filename = filename;
        filenameLabel.text = filename;
    }

    public void SupplyTracks(MidiTrack[] tracks)
    {
        if (trackGuis is not null)
        {
            for (int i = 0; i < trackGuis.Length; i++)
            {
                Destroy(trackGuis[i].gameObject);
            }
            stackedLayout.Clear();
        }

        numTracks = tracks.Length;
        trackGuis = new TrackGui[numTracks];
        mixer = new int[numTracks];

        for (int i = 0; i < numTracks; i++)
        {
            TrackGui trackGui = Instantiate(trackTemplate, trackTemplate.transform.parent);
            trackGui.SetTrack(tracks[i], i);
            trackGui.gameObject.name = $"track_{i}";
            trackGui.gameObject.SetActive(true);
            trackGuis[i] = trackGui;
            stackedLayout.AddRect(trackGui);
        }

        stackedLayout.Finish();
    }

    private const int MIX_ON = 0;
    private const int MIX_MUTE = 1;
    private const int MIX_SOLO = 2;

    private int numTracks;
    private int[] mixer;

    public void OnTrack(int index)
    {
        mixer[index] = MIX_ON;
        Remix();
    }

    public void MuteTrack(int index)
    {
        mixer[index] = MIX_MUTE;
        Remix();
    }

    public void SoloTrack(int index)
    {
        mixer[index] = MIX_SOLO;
        Remix();
    }

    private void Remix()
    {
        if (mixer.Contains(MIX_SOLO))
        {
            for (int i = 0; i < numTracks; i++)
            {
                trackGuis[i].SetAudible(mixer[i] == MIX_SOLO);
            }
            return;
        }
        for (int i = 0; i < numTracks; i++)
        {
            trackGuis[i].SetAudible(mixer[i] == MIX_ON);
        }
    }

    public MappedTrack[] GetMappedTracks()
    {
        List<MappedTrack> mappedTracks = new();
        bool hasSolos = mixer.Contains(MIX_SOLO);
        for (int i = 0; i < numTracks; i++)
        {
            if (hasSolos ? mixer[i] == MIX_SOLO : mixer[i] == MIX_ON)
            {
                mappedTracks.Add(trackGuis[i].GetMappedTrack());
            }
        }
        return mappedTracks.ToArray();
    }

    [SerializeField]
    private Diagnostics diagnostics;

    [SerializeField]
    private ErrorMessage errorMessage;

    [SerializeField]
    private Sounds sounds;
    [SerializeField]
    private DefaultSounds defaultSounds;
    public void ApplyDefaults()
    {
        TdwSound[] tdwSounds = sounds.GetTDWSounds();

        foreach (TrackGui track in trackGuis)
        {
            var programMaps = track.GetProgramMaps();
            foreach (ProgramMapGui programMap in programMaps)
            {
                bool isPercussion = programMap.IsPercussion();
                var program = programMap.GetProgram();
                if (isPercussion)
                {
                    var symbol = defaultSounds.defaultSoundsByPercussionNote[program];

                    if (string.IsNullOrEmpty(symbol))
                    {
                        continue;
                    }

                    var pitch = defaultSounds.defaultPitchByPercussionNote[program];

                    programMap.SetSound(Array.Find(tdwSounds, sound => sound.symbol == symbol));

                    programMap.SetPitch(pitch);
                }
                else
                {
                    var symbol = defaultSounds.defaultSoundsByProgramNumber[program];

                    if (string.IsNullOrEmpty(symbol))
                    {
                        continue;
                    }

                    programMap.SetSound(Array.Find(tdwSounds, sound => sound.symbol == symbol));
                }
            }
        }    
    }

    public void Next()
    {
        Debug.Log("Exporting Thirty Dollar Website file...");

        string path = Path.Combine(Application.streamingAssetsPath, "out", $"{filename}.mapinfo.json");
        //IR1Json mapInfo = new() { mappedTracks = GetMappedTracks() };
        //string json = JsonConvert.SerializeObject(mapInfo, Formatting.Indented);
        //System.IO.File.WriteAllText(path, json);

        try
        {
            Debug.Log("Performing IR First Pass...");
            var ir1 = IRFirstPass.FirstPass(GetMappedTracks());
            //json = JsonConvert.SerializeObject(ir1, Formatting.Indented);
            //path = System.IO.Path.Combine(Application.streamingAssetsPath, "out", $"{filename}.ir1.json");
            //System.IO.File.WriteAllText(path, json);

            Debug.Log("Performing IR Second Pass...");
            var ir2 = IRSecondPass.SecondPass(ir1);
            //json = JsonConvert.SerializeObject(ir2, Formatting.Indented);
            //path = System.IO.Path.Combine(Application.streamingAssetsPath, "out", $"{filename}.ir2.json");
            //System.IO.File.WriteAllText(path, json);

            Debug.Log("Performing IR Third Pass...");
            var ir3 = IRThirdPass.ThirdPass(ir2);
            //json = JsonConvert.SerializeObject(ir3, Formatting.Indented);
            //path = System.IO.Path.Combine(Application.streamingAssetsPath, "out", $"{filename}.ir3.json");
            //System.IO.File.WriteAllText(path, json);

            Debug.Log("Performing TDW First Pass...");
            var tdw1 = TdwFirstPass.FirstPass(ir3);
            Debug.Log("Performing TDW Second Pass...");
            var tdw2 = TdwSecondPass.SecondPass(tdw1);
            Debug.Log("Performing TDW Third Pass...");
            var tdw3 = TdwThirdPass.ThirdPass(tdw2);
            Debug.Log("Performing TDW Fourth Pass...");
            var tdw4 = TdwFourthPass.FourthPass(tdw3);
            Debug.Log("Converting TDW Fourth Pass to text...");
            string tdw = TdwStringify.Stringify(tdw4);

            path = Path.Combine(Application.streamingAssetsPath, "out", $"{filename}.🗿");

            Debug.Log($"Writing text to file '{Path.GetFileName(path)}'...");
            File.WriteAllText(path, tdw);
        }
        catch (Exception e)
        {
            errorMessage.SetMessage(e.ToString());
            errorMessage.Open();

            Debug.Log("Export failed!");
            return;
        }
        Debug.Log("Export finished.");

        diagnostics.SnapshotFile(path, "exported-tdw.txt");
    }
}

[Serializable]
public class IR1Json
{
    public MappedTrack[] mappedTracks;
}