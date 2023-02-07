using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    private Dictionary<string, SoundJson> sounds;
    private Dictionary<string, Texture2D> icons;

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
            if (soundJson.icon is not null)
            {
                tdwSound.icon = Array.Find(allIcons, i => i.name == soundJson.icon);
            }
            else
            {
                tdwSound.icon = Array.Find(allIcons, i => i.name == key);
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

    private Exception exception;
    private readonly List<string> warnings = new();

    public bool IsLoaded { get; private set; }
    public bool HasError { get; private set; }

    //private IEnumerator LoadCoroutine()
    //{
    //    IsLoaded = false;
    //    HasError = false;

    //    statusBar.SetMessage("Searching for assets...");
    //    yield return null;
    //    string directory = Application.streamingAssetsPath;
    //    if (!Directory.Exists(directory))
    //    {
    //        statusBar.SetMessage("Could not find the 'StreamingAssets' directory in the app's files.");
    //        IsLoaded = true;
    //        HasError = true;
    //        yield break;
    //    }
    //    directory = Path.Combine(directory, "assets");
    //    if (!Directory.Exists(directory))
    //    {
    //        statusBar.SetMessage("Could not find an 'assets' subdirectory in 'StreamingAssets/'.");
    //        IsLoaded = true;
    //        HasError = true;
    //        yield break;
    //    }
    //    string filepath = Path.Combine(directory, "sounds.json");
    //    if (!File.Exists(filepath))
    //    {
    //        statusBar.SetMessage("Could not find a 'sounds.json' file in 'StreamingAssets/assets/'.");
    //        IsLoaded = true;
    //        HasError = true;
    //        yield break;
    //    }

    //    statusBar.SetMessage("Loading 'settings.json'...");
    //    yield return null;
    //    string json;
    //    try
    //    {
    //        json = File.ReadAllText(filepath);
    //    }
    //    catch (Exception e)
    //    {
    //        statusBar.SetMessage("An unhandled exception ocurred while loading 'StreamingAssets/assets/sounds.json'.");
    //        exception = e;
    //        IsLoaded = true;
    //        HasError = true;
    //        yield break;
    //    }

    //    statusBar.SetMessage("Parsing 'settings.json'...");
    //    yield return null;
    //    try
    //    {
    //        sounds = JsonConvert.DeserializeObject<Dictionary<string, SoundJson>>(json);
    //    }
    //    catch (Exception e)
    //    {
    //        statusBar.SetMessage("An unhandled exception ocurred while parsing 'StreamingAssets/assets/sounds.json'.");
    //        exception = e;
    //        IsLoaded = true;
    //        HasError = true;
    //        yield break;
    //    }

    //    statusBar.SetMessage("Searching for assets...");
    //    yield return null;
    //    directory = Path.Combine(directory, "thirtydollar.website");
    //    if (!Directory.Exists(directory))
    //    {
    //        statusBar.SetMessage("Could not find a 'thirtydollar.website' subdirectory in 'StreamingAssets/assets/'.");
    //        IsLoaded = true;
    //        HasError = true;
    //        yield break;
    //    }
    //    string subdirectory = Path.Combine(directory, "sound_icons");
    //    if (!Directory.Exists(subdirectory))
    //    {
    //        statusBar.SetMessage("Could not find a 'sound_icons' subdirectory in 'StreamingAssets/assets/thirtydollar.website/'.");
    //        IsLoaded = true;
    //        HasError = true;
    //        yield break;
    //    }

    //    icons = new();
    //    string[] iconPaths = Directory.GetFiles(subdirectory, "*.png");
    //    int total = iconPaths.Length;
    //    int current = 1;

    //    statusBar.SetMessage($"Searching for assets (found {total} files)...");
    //    yield return null;
    //    foreach (string iconPath in iconPaths)
    //    {
    //        string relativePath = Path.GetRelativePath(Application.streamingAssetsPath, iconPath);
    //        statusBar.SetMessage($"Loading asset '{relativePath}' ({current}/{total})...");
    //        yield return null;
    //        byte[] bytes;
    //        try
    //        {
    //            bytes = File.ReadAllBytes(iconPath);
    //        }
    //        catch (Exception e)
    //        {
    //            statusBar.SetMessage($"An unhandled exception ocurred while loading '{relativePath}'.");
    //            exception = e;
    //            IsLoaded = true;
    //            HasError = true;
    //            yield break;
    //        }

    //        statusBar.SetMessage($"Importing asset '{relativePath}' ({current}/{total})...");
    //        yield return null;
    //        Texture2D texture = new(2, 2);
    //        if (!texture.LoadImage(bytes))
    //        {
    //            warnings.Add($"Failed to load '{relativePath}'.");
    //        }

    //        try
    //        {
    //            string filename = Path.GetFileNameWithoutExtension(iconPath);
    //            icons.Add(filename, texture);
    //        }
    //        catch (Exception e)
    //        {
    //            statusBar.SetMessage($"An unhandled exception ocurred while importing '{relativePath}'.");
    //            exception = e;
    //            IsLoaded = true;
    //            HasError = true;
    //            yield break;
    //        }

    //        current++;
    //    }

    //    statusBar.SetMessage("Loading complete.");
    //    IsLoaded = true;
    //}

    public void Load()
    {
        Debug.Log("Loading Thirty Dollar Website sound data...");
        string json = soundsJson.text;
        sounds = JsonConvert.DeserializeObject<Dictionary<string, SoundJson>>(json);
    }
}
