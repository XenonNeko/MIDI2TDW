using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TdwVolumeAction : TdwEvent
{
    public double volume;

    public override string ToString()
    {
        return $"!volume@{volume.ToString(System.Globalization.CultureInfo.InvariantCulture)}";
    }
}
