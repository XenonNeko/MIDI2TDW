using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DefaultSounds : MonoBehaviour
{
    private class DefaultSoundValue
    {
        public string name;
        public string tdw;
        public float pitch;
    }

    private class DefaultSoundsJson
    {
        public Dictionary<string, DefaultSoundValue> programs;
        public Dictionary<string, DefaultSoundValue> percussion;
    }

    private DefaultSoundsJson defaultSounds;

    public readonly string[] defaultSoundsByProgramNumber = new string[128];
    public readonly string[] defaultSoundsByPercussionNote = new string[128];
    public readonly float[] defaultPitchByPercussionNote = new float[128];

    public void Load()
    {
        Debug.Log("Loading default sounds...");
        string streamingAssetsPath = Application.streamingAssetsPath;
        if (!Directory.Exists(streamingAssetsPath))
        {
            Debug.Log("'StreamingAssets' directory does not exist, creating it.");
            Directory.CreateDirectory(streamingAssetsPath);
        }
        string configPath = Path.Combine(streamingAssetsPath, "config");
        if (!Directory.Exists(configPath))
        {
            Debug.Log("'config' directory does not exist, creating it.");
            Directory.CreateDirectory(configPath);
        }
        string defaultSoundsFile = Path.Combine(configPath, "default-sounds.json");
        if (!File.Exists(defaultSoundsFile))
        {
            Debug.Log("Default sounds file does not exist. Aborting.");
            return;
        }

        string json = File.ReadAllText(defaultSoundsFile);
        defaultSounds = JsonConvert.DeserializeObject<DefaultSoundsJson>(json);

        Debug.Log("Default sounds read in successfully, now importing...");

        foreach (KeyValuePair<string, DefaultSoundValue> kvp in defaultSounds.programs)
        {
            int programNumber = Convert.ToInt32(kvp.Key, 16);
            defaultSoundsByProgramNumber[programNumber] = kvp.Value.tdw;
        }

        foreach (KeyValuePair<string, DefaultSoundValue> kvp in defaultSounds.percussion)
        {
            int programNumber = int.Parse(kvp.Key);
            defaultSoundsByPercussionNote[programNumber] = kvp.Value.tdw;
            defaultPitchByPercussionNote[programNumber] = kvp.Value.pitch;
        }

        Debug.Log("Default sounds imported successfully.");
    }
}
