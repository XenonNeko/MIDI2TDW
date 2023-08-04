using System.Collections;
using System.Collections.Generic;

public static class IRThirdPass
{
    public static IntermediateChord[] ThirdPass(IntermediateChord[] input)
    {
        // Calculate delta-times for each chord.
        for (int i = 0; i < input.Length; i++)
        {
            IntermediateChord chord = input[i];
            if (i == input.Length - 1)
            {
                chord.deltaTime = 500_000;
                continue;
            }
            IntermediateChord nextChord = input[i + 1];
            chord.deltaTime = nextChord.absoluteTime - chord.absoluteTime;
        }
        return input;
    }
}
