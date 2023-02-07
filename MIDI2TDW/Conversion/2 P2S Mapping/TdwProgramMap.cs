using Melanchall.DryWetMidi.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mapping information for a single MIDI program/percussion note
/// </summary>
public class TdwProgramMap
{
    /// <summary>
    /// A TDW Sound selected by the user
    /// </summary>
    public TdwSound sound;
    /// <summary>
    /// Whether or not the user chose to tune this Sound based on MIDI pitch
    /// </summary>
    public bool useMidiTuning;
    /// <summary>
    /// The value to use as every TDW Sound's pitch parameter (in the case of no tuning) or to add to the MIDI pitch to derive the pitch parameter
    /// </summary>
    public float pitchOffset;

    public float GetPitch(SevenBitNumber midiPitchIn)
    {
        if (useMidiTuning)
        {
            return midiPitchIn + pitchOffset;
        }
        return pitchOffset;
    }
}
