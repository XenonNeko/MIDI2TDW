using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    private Dictionary<string, SoundJson> sounds;

    [SerializeField]
    private Icons iconLibrary;

    private TdwSound[] cachedSounds;

    public TdwSound GetSilence()
    {
        return cachedSounds[0];
    }

    public TdwSound[] GetTDWSounds()
    {
        Sprite[] allIcons = iconLibrary.IconSprites;
        TdwSound[] tdwSounds = new TdwSound[sounds.Count];
        foreach (KeyValuePair<string, SoundJson> keyValuePair in sounds)
        {
            string key = keyValuePair.Key;
            SoundJson soundJson = keyValuePair.Value;
            TdwSound tdwSound = new()
            {
                symbol = key,
                name = soundJson.name,
                origin = soundJson.origin,
                hasPitch = soundJson.midi.HasValue,
                midiPitch = soundJson.midi.GetValueOrDefault()
            };
            // emoji
            if (key.Length == 2 && char.IsSurrogatePair(key[0], key[1]))
            {
                key = char.ConvertToUtf32(key[0], key[1]).ToString("x5");
            }
            tdwSound.icon = Array.Find(allIcons, i => i.name == key);
            if (!tdwSound.icon)
            {
                tdwSound.icon = iconLibrary.NoIcon;
            }
            tdwSounds[soundJson.id] = tdwSound;
        }
        cachedSounds = tdwSounds;
        return tdwSounds;
    }

    [SerializeField]
    private StatusBar statusBar;

    [SerializeField]
    private TextAsset soundsJson;
    [SerializeField]
    private TextAsset soundsCsv;

    public bool IsLoaded { get; private set; }
    public bool HasError { get; private set; }

    public void Load()
    {
        Debug.Log("Loading Thirty Dollar Website sound data...");
        string json = soundsJson.text;
        sounds = LoadSounds(json);
    }

    private Dictionary<string, SoundJson> LoadSounds(string json)
    {
        Dictionary<string, SoundJson> sounds = JsonConvert.DeserializeObject<Dictionary<string, SoundJson>>(json);

        Regex regex = new(@"""([^""]+)"":\s*{");
        MatchCollection matches = regex.Matches(json);

        if (matches.Count != sounds.Count)
        {
            throw new Exception($"Number of RegEx Matches ({matches.Count}) differs from number of Keys ({sounds.Count})");
        }

        for (int i = 0; i < matches.Count; i++)
        {
            Match match = matches[i];
            Group group = match.Groups[1];
            string key = group.Value;
            SoundJson sound = sounds[key];
            sound.id = i;
        }

        return sounds;
    }
}
