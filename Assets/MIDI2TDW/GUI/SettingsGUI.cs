using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Settings;

public class SettingsGUI : MonoBehaviour
{
    [SerializeField] private Settings settings;
    [Header("GUI")]
    [SerializeField] private Toggle doVolumeParameters;
    [SerializeField] private Toggle dumpConversionIntermediates;
    [SerializeField] private TMP_InputField tempoDecimalPlaces;

    public void LoadSettings()
    {
        doVolumeParameters.SetIsOnWithoutNotify(settings.AppSettings.doVolumeParameters > 0);
        dumpConversionIntermediates.SetIsOnWithoutNotify(settings.AppSettings.dumpConversionIntermediates > 0);
        tempoDecimalPlaces.SetTextWithoutNotify(settings.AppSettings.tempoDecimalPlaces.ToString());
    }

    public void DoVolumeParameters(bool value)
    {
        settings.AppSettings.doVolumeParameters = value ? 1 : 0;
        ApplyAndSaveSettings();
    }
    public void DumpConversionIntermediates(bool value)
    {
        settings.AppSettings.dumpConversionIntermediates = value ? 1 : 0;
        ApplyAndSaveSettings();
    }
    public void TempoDecimalPlaces(string valueAsString)
    {
        int value = string.IsNullOrEmpty(valueAsString) ? 0 : int.Parse(valueAsString);
        settings.AppSettings.tempoDecimalPlaces = value;
        ApplyAndSaveSettings();
    }

    public void ApplyAndSaveSettings()
    {
        settings.ApplySettings();

        string configPath = settings.EnsureConfigDirectory();
        string settingsFile = Path.Combine(configPath, "settings.json");
        string json;
        if (settings.AppSettings.doSecretSettings)
        {
            json = JsonConvert.SerializeObject(settings.AppSettings, Formatting.Indented);
        }
        else
        {
            SettingsJson settings = (SettingsJson)this.settings.AppSettings;
            json = JsonConvert.SerializeObject(settings, Formatting.Indented);
        }
        File.WriteAllText(settingsFile, json);
    }
}
