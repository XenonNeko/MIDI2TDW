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
    public class SettingsJson
    {
        public int doIntro;
        public int debugMidiImport;
        public int dumpConversionIntermediates;
        public int doVolumeActions;

        public int doPaletteRandomization;
        public int doHueShift;
        public float hueShiftAmount;
        public int doJitter;
        public float jitterFactor;
        public int doPhysics;
        public int doSettingsEveryFrame;
    }

    public SettingsJson AppSettings { get; private set; }

    private void RandomizePalette()
    {
        Camera camera = Camera.main;
        camera.backgroundColor = Random.ColorHSV();
        Canvas canvas = FindObjectOfType<Canvas>();
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
        Canvas canvas = FindObjectOfType<Canvas>();
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
        Canvas canvas = FindObjectOfType<Canvas>();
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
        Canvas canvas = FindObjectOfType<Canvas>();
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

    private void ApplySettings()
    {
        TdwStringify.doIntro = Eval(AppSettings.doIntro);
        MidiFileImporter.debugMidiImport = Eval(AppSettings.debugMidiImport);
        TracksScreen.dumpConversionIntermediates = Eval(AppSettings.dumpConversionIntermediates);
        TdwThirdPass.doVolumeActions = Eval(AppSettings.doVolumeActions);

        DoIf(RandomizePalette, AppSettings.doPaletteRandomization);
        DoIf(HueShift, AppSettings.doHueShift);
        DoIf(JitterUiElements, AppSettings.doJitter);
        DoIf(DoPhysics, AppSettings.doPhysics);
    }

    public void LoadAppSettings()
    {
        Debug.Log("Loading settings...");
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
        string defaultSoundsFile = Path.Combine(configPath, "settings.json");
        if (!File.Exists(defaultSoundsFile))
        {
            Debug.Log("Settings file does not exist. Aborting.");
            return;
        }

        string json = File.ReadAllText(defaultSoundsFile);
        AppSettings = JsonConvert.DeserializeObject<SettingsJson>(json);

        Debug.Log("Settings parsed successfully, now applying...");

        ApplySettings();

        Debug.Log("Settings applied successfully.");
    }

    public Melanchall.DryWetMidi.Core.ReadingSettings ReadingSettings { get; private set; }

    public void LoadReadingSettings()
    {
        Debug.Log("Loading MIDI importer ReadingSettings...");
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
        string defaultSoundsFile = Path.Combine(configPath, "readingsettings.json");
        if (!File.Exists(defaultSoundsFile))
        {
            Debug.Log("ReadingSettings file does not exist. Aborting.");
            return;
        }

        string json = File.ReadAllText(defaultSoundsFile);
        ReadingSettings = JsonConvert.DeserializeObject<Melanchall.DryWetMidi.Core.ReadingSettings>(json);

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
