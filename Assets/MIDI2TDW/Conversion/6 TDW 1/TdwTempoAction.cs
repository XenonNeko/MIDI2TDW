using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TdwTempoAction : TdwEvent
{
    public long intervalMicroseconds;
    public string tempo;

    private const double BEAT_DURATION_120_BPM = 0.5;
    private static string format = "0";
    private static int tempoDecimalPlaces;
    public static int TempoDecimalPlaces
    {
        set
        {
            if (value < 0)
            {
                Debug.LogError($"Invalid number of decimal places '{value}'");
                tempoDecimalPlaces = 0;
                return;
            }
            tempoDecimalPlaces = value;
            StringBuilder stringBuilder = new("0.");
            for (int i = 0; i < value; i++)
            {
                stringBuilder.Append('#');
            }
            format = stringBuilder.ToString();
        }
    }

    public static TdwTempoAction CreateFromMicrosecondDeltaTime(long microseconds)
    {
        double interval = microseconds / 1_000_000.0;
        double tempoScale = BEAT_DURATION_120_BPM / interval;
        double tempo = 120.0 * tempoScale;
        if (tempoDecimalPlaces == 0)
        {
            tempo = Math.Round(tempo);
        }
        return new()
        {
            intervalMicroseconds = microseconds,
            tempo = tempo.ToString(format, System.Globalization.CultureInfo.InvariantCulture)
        };
    }

    public override string ToString()
    {
        return $"!speed@{tempo}";
    }
}
