using Melanchall.DryWetMidi.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MappedTrackRepair
{
    private static void ReplaceProgramNumber(MidiSound midiSound, SevenBitNumber newNumber, bool isPercussion)
    {
        if (isPercussion)
        {
            midiSound.noteNumber = newNumber;
        }
        else
        {
            midiSound.programNumber = newNumber;
        }
    }

    /// <summary>
    /// Circumvents a common error where notes reference MIDI Programs which are not defined
    /// in the program mappings.
    /// </summary>
    /// <remarks>
    /// The way in which this issue is circumvented depends on the offending Program Number.
    /// If the offending Program Number is 0, it is changed to the last non-zero Program Number.
    /// If no non-zero Program Numbers also present in the program mappings were found yet, the first
    /// Program Number present in the program mappings is used.
    /// Otherwise, the attempt fails.
    /// </remarks>
    /// <param name="mappedTrack"></param>
    public static void RepairTrack(MappedTrack mappedTrack)
    {
        MidiTrack midiTrack = mappedTrack.midiTrack;
        bool isPercussion = midiTrack.isPercussion;
        MidiSound[] midiSounds = midiTrack.sounds;
        var programMappings = mappedTrack.programMappings;

        int length = midiSounds.Length;

        bool hasValidProgramNumber = false;
        SevenBitNumber mostRecentValidProgramNumber = (SevenBitNumber)0;
        SevenBitNumber firstValidProgramNumber = programMappings.Keys.First();

        int soundsRepaired = 0;

        for (int i = 0; i < length; i++)
        {
            MidiSound midiSound = midiSounds[i];
            SevenBitNumber programNumber = IRFirstPass.GetProgramNumber(midiSound, midiTrack);

            if (mappedTrack.programMappings.ContainsKey(programNumber))
            {
                mostRecentValidProgramNumber = programNumber;
                hasValidProgramNumber = true;
                continue;
            }
            // A sound in the MIDI Track uses a program which is not specified in the program mappings.
            if (programNumber != 0)
            {
                Debug.LogWarning("MIDI data could not confidently be repaired.");
                //throw new Exception("MIDI data could not confidently be repaired.");
            }
            SevenBitNumber newNumber = hasValidProgramNumber ? mostRecentValidProgramNumber : firstValidProgramNumber;
            ReplaceProgramNumber(midiSound, newNumber, isPercussion);
            soundsRepaired++;
        }

        if (soundsRepaired > 0)
        {
            Debug.LogWarning($"Repaired {soundsRepaired} sounds.");
        }
    }
}
