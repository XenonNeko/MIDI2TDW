using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TdwSoundEvent : TdwEvent
{
    public string symbol;
    public double pitch;
    public float volume;

    public override string ToString()
    {
        string pitchText = pitch.ToString(System.Globalization.CultureInfo.InvariantCulture);
        if (volume == 1f)
        {
            return $"{symbol}@{pitchText}";
        }
        else
        {
            int volumePercent = Mathf.RoundToInt(volume * 100f);
            return $"{symbol}@{pitchText}%{volumePercent}";
        }
    }
}
