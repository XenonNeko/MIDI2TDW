using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TdwIntermediateSoundCollection : TdwEvent
{
    public IntermediateSound[] sounds;

    public static TdwIntermediateSoundCollection CreateFromIntermediateChord(IntermediateChord chord)
    {
        return new() { sounds = chord.GetSounds() };
    }

    public override string ToString()
    {
        throw new NotSupportedException();
    }
}
