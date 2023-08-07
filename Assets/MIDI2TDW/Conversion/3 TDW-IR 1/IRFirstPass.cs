using Melanchall.DryWetMidi.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class IRFirstPass
{
    public static SevenBitNumber GetProgramNumber(MidiSound midiSound, MidiTrack sourceTrack)
    {
        return sourceTrack.isPercussion ? midiSound.noteNumber : midiSound.programNumber;
    }

    public static IntermediateChord[] FirstPass(MappedTrack[] input)
    {
        // Initialize the output to a buffer which can hold all sounds from all tracks.
        IntermediateChord[] output = new IntermediateChord[input.Sum(track => track.midiTrack.sounds.Length)];

        // Iterate over all tracks, converting each one to a collection of single-note chords.
        int destinationIndex = 0;
        for (int h = 0; h < input.Length; h++)
        {
            MappedTrack mappedTrack = input[h];

            MidiTrack midiTrack = mappedTrack.midiTrack;

            Debug.Log($"input[{h}] \"{mappedTrack.midiTrack.name}\", isPercussion: {midiTrack.isPercussion}, # of mappings: {mappedTrack.programMappings.Count}");

            MidiSound[] midiSounds = midiTrack.sounds;
            // Allocate a buffer for the chords we will be creating.
            IntermediateChord[] interChords = new IntermediateChord[midiSounds.Length];

            // Iterate over all notes in this track, converting each one to a single-note chord.
            int length = interChords.Length;
            for (int i = 0; i < length; i++)
            {
                MidiSound midiSound = midiSounds[i];
                IntermediateSound interSound = new();
                SevenBitNumber programNumber = GetProgramNumber(midiSound, midiTrack);

                TdwProgramMap mapping = mappedTrack.programMappings[programNumber];

                interSound.tdwSound = mapping.sound;
                interSound.pitchParameter = mapping.GetPitch(midiSound.noteNumber);
                interSound.volume = midiSound.GetVolume();

                IntermediateChord interChord = new();
                interChord.AddSound(interSound);
                interChord.absoluteTime = midiSound.timeMicroseconds;

                interChords[i] = interChord;
            }

            // Copy the buffer into the output buffer.
            Array.Copy(interChords, 0, output, destinationIndex, length);
            destinationIndex += length;
        }

        // Sort the output buffer chronologically.
        Array.Sort(output, (a, b) => (int)(a.absoluteTime - b.absoluteTime));

        return output;
    }
}
