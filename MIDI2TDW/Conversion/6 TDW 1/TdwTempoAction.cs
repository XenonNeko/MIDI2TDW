using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TdwTempoAction : TdwEvent
{
    public long intervalMicroseconds;
    public double tempo;

    private const double BEAT_DURATION_120_BPM = 0.5;

    public static TdwTempoAction CreateFromMicrosecondDeltaTime(long microseconds)
    {
        double interval = microseconds / 1_000_000.0;
        double tempoScale = BEAT_DURATION_120_BPM / interval;
        double tempo = 120.0 * tempoScale;
        return new() { intervalMicroseconds = microseconds, tempo = tempo };
    }

    public override string ToString()
    {
        return $"!speed@{tempo.ToString(System.Globalization.CultureInfo.InvariantCulture)}";
    }
}
