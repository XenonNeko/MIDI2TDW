using Melanchall.DryWetMidi.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class BuzzToneConvert
{
    private const string timbreSound = "hammer";

    private static double MidiNoteNumberToFrequency(SevenBitNumber noteNumber)
    {
        return 440.0 * Math.Pow(2.0, (noteNumber - 69.0) / 12.0);
    }

    public static void ConvertTrack(MidiTrack midiTrack)
    {
        string test = "!speed@7937|!speed@10000@+|!looptarget|hammer|!loopmany@60|!stop@4";

        StringBuilder stringBuilder = new StringBuilder();

        long previousEndMicroseconds = 0;

        foreach (var note in midiTrack.sounds)
        {
            long pause = note.timeMicroseconds - previousEndMicroseconds;

            double bpm = 10000.0;
            double frequency = bpm / 60.0;
            double wavelength = 1.0 / frequency;
            double duration = pause / 1_000_000.0;
            double cyclesForDuration = duration / wavelength;

            int integerLoops = (int)Math.Round(cyclesForDuration);

            if (integerLoops > 0)
            {
                stringBuilder.Append($"!speed@10000|");
                stringBuilder.Append($"!stop@{integerLoops}|");
            }

            frequency = MidiNoteNumberToFrequency(note.noteNumber);
            bpm = frequency * 60.0;
            wavelength = 1.0 / frequency;
            duration = note.durationMicroseconds / 1_000_000.0;
            cyclesForDuration = duration / wavelength;

            //Debug.Log($"frequency: {frequency}, bpm: {bpm}, wavelength: {wavelength}, duration: {duration}, cyclesForDuration: {cyclesForDuration}");

            int integerBpm = (int)Math.Round(bpm);
            int addBpm = 0;
            if (integerBpm > 10000)
            {
                addBpm = integerBpm - 10000;
                integerBpm = 10000;
            }
            integerLoops = (int)Math.Round(cyclesForDuration);

            Debug.Log($"bpm: {bpm}, integerBpm: {integerBpm}, addBpm: {addBpm}");

            stringBuilder.Append($"!speed@{integerBpm}|");
            if (addBpm > 0)
            {
                stringBuilder.Append($"!speed@{addBpm}@+|");
            }

            stringBuilder.Append("!looptarget|");
            stringBuilder.Append($"{timbreSound}|");
            stringBuilder.Append($"!loopmany@{integerLoops}|");

            previousEndMicroseconds = note.timeMicroseconds + note.durationMicroseconds;
        }

        string path = Path.Combine(Application.streamingAssetsPath, "out", "test.🗿");

        File.WriteAllText(path, stringBuilder.ToString());
    }
}
