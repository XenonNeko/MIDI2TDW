using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Interaction;
using System;

/// <summary>
/// Stripped-down version of a <see cref="Note"/> which carries only the information required by the converter
/// </summary>
[Serializable]
public class MidiSound
{
    /// <summary>
    /// The MIDI program number (instrument)
    /// </summary>
    public SevenBitNumber programNumber;
    /// <summary>
    /// The MIDI note number (pitch)
    /// </summary>
    public SevenBitNumber noteNumber;
    /// <summary>
    /// The MIDI velocity
    /// </summary>
    public SevenBitNumber velocity;
    /// <summary>
    /// The Channel Volume at the time of this note
    /// </summary>
    public SevenBitNumber channelVolume;
    /// <summary>
    /// Absolute Time of the Note in microseconds
    /// </summary>
    public long timeMicroseconds;
    /// <summary>
    /// Duration of the Note in microseconds
    /// </summary>
    public long durationMicroseconds;

    public int GetVolume()
    {
        return channelVolume * velocity;
    }
}
