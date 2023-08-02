using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Melanchall.DryWetMidi.Core;
using TMPro;

public class ReadingSettingsGUI : MonoBehaviour
{
    [SerializeField] private Settings settings;
    [SerializeField] private SettingsGUI settingsGui;
    [Header("GUI")]
    [SerializeField] private Toggle debugMidiImport;
    [SerializeField] private TMP_Dropdown unexpectedTrackChunksCountPolicy;
    [SerializeField] private TMP_Dropdown extraTrackChunkPolicy;
    [SerializeField] private TMP_Dropdown unknownChunkIdPolicy;
    [SerializeField] private TMP_Dropdown missedEndOfTrackPolicy;
    [SerializeField] private TMP_Dropdown silentNoteOnPolicy;
    [SerializeField] private TMP_Dropdown invalidChunkSizePolicy;
    [SerializeField] private TMP_Dropdown unknownFileFormatPolicy;
    [SerializeField] private TMP_Dropdown unknownChannelEventPolicy;
    [SerializeField] private TMP_Dropdown invalidChannelEventParameterValuePolicy;
    [SerializeField] private TMP_Dropdown invalidMetaEventParameterValuePolicy;
    [SerializeField] private TMP_Dropdown invalidSystemCommonEventParameterValuePolicy;
    [SerializeField] private TMP_Dropdown notEnoughBytesPolicy;
    [SerializeField] private TMP_Dropdown noHeaderChunkPolicy;
    [SerializeField] private TMP_Dropdown zeroLengthDataPolicy;
    [SerializeField] private TMP_Dropdown endOfTrackStoringPolicy;

    public void LoadSettings()
    {
        debugMidiImport.SetIsOnWithoutNotify(settings.AppSettings.debugMidiImport > 0);

        unexpectedTrackChunksCountPolicy.SetValueWithoutNotify((int)settings.ReadingSettings.UnexpectedTrackChunksCountPolicy);
        extraTrackChunkPolicy.SetValueWithoutNotify((int)settings.ReadingSettings.ExtraTrackChunkPolicy);
        unknownChunkIdPolicy.SetValueWithoutNotify((int)settings.ReadingSettings.UnknownChunkIdPolicy);
        missedEndOfTrackPolicy.SetValueWithoutNotify((int)settings.ReadingSettings.MissedEndOfTrackPolicy);
        silentNoteOnPolicy.SetValueWithoutNotify((int)settings.ReadingSettings.SilentNoteOnPolicy);
        invalidChunkSizePolicy.SetValueWithoutNotify((int)settings.ReadingSettings.InvalidChunkSizePolicy);
        unknownFileFormatPolicy.SetValueWithoutNotify((int)settings.ReadingSettings.UnknownFileFormatPolicy);
        unknownChannelEventPolicy.SetValueWithoutNotify((int)settings.ReadingSettings.UnknownChannelEventPolicy);
        invalidChannelEventParameterValuePolicy.SetValueWithoutNotify((int)settings.ReadingSettings.InvalidChannelEventParameterValuePolicy);
        invalidMetaEventParameterValuePolicy.SetValueWithoutNotify((int)settings.ReadingSettings.InvalidMetaEventParameterValuePolicy);
        invalidSystemCommonEventParameterValuePolicy.SetValueWithoutNotify((int)settings.ReadingSettings.InvalidSystemCommonEventParameterValuePolicy);
        notEnoughBytesPolicy.SetValueWithoutNotify((int)settings.ReadingSettings.NotEnoughBytesPolicy);
        noHeaderChunkPolicy.SetValueWithoutNotify((int)settings.ReadingSettings.NoHeaderChunkPolicy);
        zeroLengthDataPolicy.SetValueWithoutNotify((int)settings.ReadingSettings.ZeroLengthDataPolicy);
        endOfTrackStoringPolicy.SetValueWithoutNotify((int)settings.ReadingSettings.EndOfTrackStoringPolicy);
    }

    public void DebugMidiImport(bool value)
    {
        settings.AppSettings.debugMidiImport = value ? 1 : 0;
        settingsGui.ApplyAndSaveSettings();
    }

    public void UnexpectedTrackChunksCountPolicy(int value)
    {
        settings.ReadingSettings.UnexpectedTrackChunksCountPolicy = (UnexpectedTrackChunksCountPolicy)value;
        SaveReadingSettings();
    }
    public void ExtraTrackChunkPolicy(int value)
    {
        settings.ReadingSettings.ExtraTrackChunkPolicy = (ExtraTrackChunkPolicy)value;
        SaveReadingSettings();
    }
    public void UnknownChunkIdPolicy(int value)
    {
        settings.ReadingSettings.UnknownChunkIdPolicy = (UnknownChunkIdPolicy)value;
        SaveReadingSettings();
    }
    public void MissedEndOfTrackPolicy(int value)
    {
        settings.ReadingSettings.MissedEndOfTrackPolicy = (MissedEndOfTrackPolicy)value;
        SaveReadingSettings();
    }
    public void SilentNoteOnPolicy(int value)
    {
        settings.ReadingSettings.SilentNoteOnPolicy = (SilentNoteOnPolicy)value;
        SaveReadingSettings();
    }
    public void InvalidChunkSizePolicy(int value)
    {
        settings.ReadingSettings.InvalidChunkSizePolicy = (InvalidChunkSizePolicy)value;
        SaveReadingSettings();
    }
    public void UnknownFileFormatPolicy(int value)
    {
        settings.ReadingSettings.UnknownFileFormatPolicy = (UnknownFileFormatPolicy)value;
        SaveReadingSettings();
    }
    public void UnknownChannelEventPolicy(int value)
    {
        settings.ReadingSettings.UnknownChannelEventPolicy = (UnknownChannelEventPolicy)value;
        SaveReadingSettings();
    }
    public void InvalidChannelEventParameterValuePolicy(int value)
    {
        settings.ReadingSettings.InvalidChannelEventParameterValuePolicy = (InvalidChannelEventParameterValuePolicy)value;
        SaveReadingSettings();
    }
    public void InvalidMetaEventParameterValuePolicy(int value)
    {
        settings.ReadingSettings.InvalidMetaEventParameterValuePolicy = (InvalidMetaEventParameterValuePolicy)value;
        SaveReadingSettings();
    }
    public void InvalidSystemCommonEventParameterValuePolicy(int value)
    {
        settings.ReadingSettings.InvalidSystemCommonEventParameterValuePolicy = (InvalidSystemCommonEventParameterValuePolicy)value;
        SaveReadingSettings();
    }
    public void NotEnoughBytesPolicy(int value)
    {
        settings.ReadingSettings.NotEnoughBytesPolicy = (NotEnoughBytesPolicy)value;
        SaveReadingSettings();
    }
    public void NoHeaderChunkPolicy(int value)
    {
        settings.ReadingSettings.NoHeaderChunkPolicy = (NoHeaderChunkPolicy)value;
        SaveReadingSettings();
    }
    public void ZeroLengthDataPolicy(int value)
    {
        settings.ReadingSettings.ZeroLengthDataPolicy = (ZeroLengthDataPolicy)value;
        SaveReadingSettings();
    }
    public void EndOfTrackStoringPolicy(int value)
    {
        settings.ReadingSettings.EndOfTrackStoringPolicy = (EndOfTrackStoringPolicy)value;
        SaveReadingSettings();
    }

    private void SaveReadingSettings()
    {
        string configPath = settings.EnsureConfigDirectory();
        string settingsFile = Path.Combine(configPath, "readingsettings.json");
        ReadingSettingsJson readingSettings = (ReadingSettingsJson)settings.ReadingSettings;
        string json = JsonConvert.SerializeObject(readingSettings, Formatting.Indented);
        File.WriteAllText(settingsFile, json);
    }
}
