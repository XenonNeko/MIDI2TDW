using System.Collections;
using System.Collections.Generic;

public static class TdwFirstPass
{
    public static TdwEvent[] FirstPass(IntermediateChord[] input)
    {
        // Create an array of TDW events by pairing each input chord with a tempo action based on its delta time.
        TdwEvent[] output = new TdwEvent[input.Length * 2];
        for (int i = 0; i < input.Length; i++)
        {
            IntermediateChord chord = input[i];
            int j0 = i * 2;
            int j1 = j0 + 1;
            output[j0] = TdwTempoAction.CreateFromMicrosecondDeltaTime(chord.deltaTime);
            output[j1] = TdwIntermediateSoundCollection.CreateFromIntermediateChord(chord);
        }
        return output;
    }
}
