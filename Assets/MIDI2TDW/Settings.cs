using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Settings : MonoBehaviour
{
    [SerializeField] private SettingsGUI settingsGUI;
    [SerializeField] private ReadingSettingsGUI readingSettingsGUI;

    public FullSettingsJson AppSettings { get; private set; }

    private void RandomizePalette()
    {
        Camera camera = Camera.main;
        camera.backgroundColor = Random.ColorHSV();
        Canvas canvas = FindFirstObjectByType<Canvas>();
        Graphic[] graphics = canvas.GetComponentsInChildren<Graphic>(true);
        HashSet<Color> palette = new();
        Dictionary<Color, Color> colorRemap = new();
        foreach (Graphic graphic in graphics)
        {
            if (graphic.color == Color.white)
            {
                continue;
            }
            palette.Add(graphic.color);
        }
        colorRemap[Color.white] = Color.white;
        foreach (Color color in palette)
        {
            Color newColor = Random.ColorHSV();
            newColor.a = color.a;
            colorRemap[color] = newColor;
        }
        foreach (Graphic graphic in graphics)
        {
            graphic.color = colorRemap[graphic.color];
        }
    }

    private void HueShift()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        Graphic[] graphics = canvas.GetComponentsInChildren<Graphic>(true);
        foreach (Graphic graphic in graphics)
        {
            Color.RGBToHSV(graphic.color, out float H, out float S, out float V);
            Color outColor = Color.HSVToRGB((H + (AppSettings.hueShiftAmount / 360f)) % 1f, S, V);
            outColor.a = graphic.color.a;
            graphic.color = outColor;
        }
    }

    private void JitterUiElements()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        Transform[] transforms = canvas.GetComponentsInChildren<Transform>(true);
        foreach (Transform transform in transforms)
        {
            Vector2 offset = Random.insideUnitCircle.normalized;
            offset *= AppSettings.jitterFactor;
            transform.Translate(offset);
        }
    }

    private bool Eval(int value)
    {
        return value > 0;
    }

    private void DoPhysics()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        Graphic[] graphics = canvas.GetComponentsInChildren<Graphic>(true);
        foreach (Graphic graphic in graphics)
        {
            RectTransform rect = graphic.GetComponent<RectTransform>();
            Mask mask = graphic.GetComponent<Mask>();
            if (mask)
            {
                continue;
            }
            TMPro.TextMeshProUGUI text = graphic.GetComponent<TMPro.TextMeshProUGUI>();
            if (text)
            {
                continue;
            }
            if (rect.gameObject == canvas.gameObject)
            {
                continue;
            }
            BoxCollider2D box = rect.gameObject.AddComponent<BoxCollider2D>();
            box.size = rect.rect.size;
            box.offset = rect.rect.center;
            Rigidbody2D rigidbody = rect.gameObject.AddComponent<Rigidbody2D>();
            rigidbody.mass = 1000;
        }
    }

    private void DoIf(Action action, int condition)
    {
        if (condition <= 0)
        {
            return;
        }
        action.Invoke();
    }

    public void ApplySettings()
    {
        TdwStringify.doIntro = Eval(AppSettings.doIntro);
        MidiFileImporter.debugMidiImport = Eval(AppSettings.debugMidiImport);
        TracksScreen.dumpConversionIntermediates = Eval(AppSettings.dumpConversionIntermediates);
        TdwThirdPass.doVolumeParameters = Eval(AppSettings.doVolumeParameters);
        TdwTempoAction.TempoDecimalPlaces = AppSettings.tempoDecimalPlaces;

        DoIf(RandomizePalette, AppSettings.doPaletteRandomization);
        DoIf(HueShift, AppSettings.doHueShift);
        DoIf(JitterUiElements, AppSettings.doJitter);
        DoIf(DoPhysics, AppSettings.doPhysics);
    }

    public string EnsureConfigDirectory()
    {
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
        return configPath;
    }

    public void LoadAppSettings()
    {
        Debug.Log("Loading settings...");
        string configPath = EnsureConfigDirectory();
        string settingsFile = Path.Combine(configPath, "settings.json");
        if (!File.Exists(settingsFile))
        {
            Debug.Log("Settings file does not exist. Aborting.");
            return;
        }

        string json = File.ReadAllText(settingsFile);
        AppSettings = JsonConvert.DeserializeObject<FullSettingsJson>(json);

        Debug.Log("Settings parsed successfully, now applying...");

        ApplySettings();

        settingsGUI.LoadSettings();

        Debug.Log("Settings applied successfully.");
    }

    public Melanchall.DryWetMidi.Core.ReadingSettings ReadingSettings { get; private set; }

    public void LoadReadingSettings()
    {
        Debug.Log("Loading MIDI importer ReadingSettings...");
        string configPath = EnsureConfigDirectory();
        string readingSettingsFile = Path.Combine(configPath, "readingsettings.json");
        if (!File.Exists(readingSettingsFile))
        {
            Debug.Log("ReadingSettings file does not exist. Aborting.");
            return;
        }

        string json = File.ReadAllText(readingSettingsFile);
        ReadingSettings = JsonConvert.DeserializeObject<Melanchall.DryWetMidi.Core.ReadingSettings>(json);

        readingSettingsGUI.LoadSettings();

        Debug.Log("ReadingSettings imported successfully.");
    }

    private void Update()
    {
        if (AppSettings.doSettingsEveryFrame <= 0)
        {
            return;
        }
        if (AppSettings.doPaletteRandomization > 0)
        {
            RandomizePalette();
        }
        if (AppSettings.doHueShift > 0)
        {
            HueShift();
        }
        if (AppSettings.doJitter > 0)
        {
            JitterUiElements();
        }
    }
}
