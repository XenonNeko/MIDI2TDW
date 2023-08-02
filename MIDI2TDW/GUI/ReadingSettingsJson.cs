using Melanchall.DryWetMidi.Core;

public class ReadingSettingsJson
{
    public string UnexpectedTrackChunksCountPolicy;
    public string ExtraTrackChunkPolicy;
    public string UnknownChunkIdPolicy;
    public string MissedEndOfTrackPolicy;
    public string SilentNoteOnPolicy;
    public string InvalidChunkSizePolicy;
    public string UnknownFileFormatPolicy;
    public string UnknownChannelEventPolicy;
    public string InvalidChannelEventParameterValuePolicy;
    public string InvalidMetaEventParameterValuePolicy;
    public string InvalidSystemCommonEventParameterValuePolicy;
    public string NotEnoughBytesPolicy;
    public string NoHeaderChunkPolicy;
    public string ZeroLengthDataPolicy;
    public string EndOfTrackStoringPolicy;

    public static explicit operator ReadingSettingsJson(ReadingSettings readingSettings)
    {
        return new ReadingSettingsJson
        {
            UnexpectedTrackChunksCountPolicy = readingSettings.UnexpectedTrackChunksCountPolicy.ToString(),
            ExtraTrackChunkPolicy = readingSettings.ExtraTrackChunkPolicy.ToString(),
            UnknownChunkIdPolicy = readingSettings.UnknownChunkIdPolicy.ToString(),
            MissedEndOfTrackPolicy = readingSettings.MissedEndOfTrackPolicy.ToString(),
            SilentNoteOnPolicy = readingSettings.SilentNoteOnPolicy.ToString(),
            InvalidChunkSizePolicy = readingSettings.InvalidChunkSizePolicy.ToString(),
            UnknownFileFormatPolicy = readingSettings.UnknownFileFormatPolicy.ToString(),
            UnknownChannelEventPolicy = readingSettings.UnknownChannelEventPolicy.ToString(),
            InvalidChannelEventParameterValuePolicy = readingSettings.InvalidChannelEventParameterValuePolicy.ToString(),
            InvalidMetaEventParameterValuePolicy = readingSettings.InvalidMetaEventParameterValuePolicy.ToString(),
            InvalidSystemCommonEventParameterValuePolicy = readingSettings.InvalidSystemCommonEventParameterValuePolicy.ToString(),
            NotEnoughBytesPolicy = readingSettings.NotEnoughBytesPolicy.ToString(),
            NoHeaderChunkPolicy = readingSettings.NoHeaderChunkPolicy.ToString(),
            ZeroLengthDataPolicy = readingSettings.ZeroLengthDataPolicy.ToString(),
            EndOfTrackStoringPolicy = readingSettings.EndOfTrackStoringPolicy.ToString(),
        };
    }
}
