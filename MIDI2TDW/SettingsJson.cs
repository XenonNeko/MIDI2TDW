public class SettingsJson
{
    public int debugMidiImport;
    public int dumpConversionIntermediates;
    public int doVolumeParameters;
    public int tempoDecimalPlaces;

    public static explicit operator SettingsJson(FullSettingsJson full)
    {
        return new SettingsJson()
        {
            debugMidiImport = full.debugMidiImport,
            dumpConversionIntermediates = full.dumpConversionIntermediates,
            doVolumeParameters = full.doVolumeParameters,
            tempoDecimalPlaces = full.tempoDecimalPlaces
        };
    }
}
