using Melanchall.DryWetMidi.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A <see cref="MidiTrack"/> augmented with user-defined Mapping information
/// </summary>
[Serializable]
public class MappedTrack
{
    /// <summary>
    /// The MidiTrack being mapped
    /// </summary>
    public MidiTrack midiTrack;
    /// <summary>
    /// A mapping of MIDI programs/percussion notes to TDW Sounds
    /// </summary>
    public Dictionary<SevenBitNumber, TdwProgramMap> programMappings = new();
}
