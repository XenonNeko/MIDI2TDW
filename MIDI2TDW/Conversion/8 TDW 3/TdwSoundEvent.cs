using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TdwSoundEvent : TdwEvent
{
    public string symbol;
    public double pitch;

    public override string ToString()
    {
        return $"{symbol}@{pitch.ToString(System.Globalization.CultureInfo.InvariantCulture)}";
    }
}
