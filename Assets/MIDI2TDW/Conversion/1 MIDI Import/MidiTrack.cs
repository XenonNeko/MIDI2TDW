using Melanchall.DryWetMidi.Common;
using System;
using System.Text;

/// <summary>
/// A stripped-down representation of a MIDI track
/// </summary>
[Serializable]
public class MidiTrack
{
    /// <summary>
    /// The name of this track
    /// </summary>
    public string name;
    /// <summary>
    /// Whether or not this track is a percussion (channel 10) track
    /// </summary>
    public bool isPercussion;
    /// <summary>
    /// The list of Notes in this track
    /// </summary>
    public MidiSound[] sounds;
    /// <summary>
    /// All MIDI programs (or notes in the case of channel 10) which appear in this track
    /// </summary>
    public SevenBitNumber[] programs;

    public override string ToString()
    {
        return $"{name} ({(isPercussion ? "percussion" : "melody")}, " +
            $"{programs.Length} {(programs.Length == 1 ? "program" : "programs")}, " +
            $"{sounds.Length} {(sounds.Length == 1 ? "sound" : "sounds")})";
    }

    public string PrintFullTrack()
    {
        StringBuilder builder = new();
        builder.AppendLine($"\"{name}\" ({(isPercussion ? "percussion" : "melody")} track)");
        foreach (MidiSound midiSound in sounds)
        {

        }
        return builder.ToString();
    }
}
