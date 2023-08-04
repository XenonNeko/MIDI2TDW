using System.Collections;
using System.Collections.Generic;

public static class IRSecondPass
{
    public static IntermediateChord[] SecondPass(IntermediateChord[] input)
    {
        // Condense the collection of single-note chords into a collection of multi-note chords
        // by merging simultaneous single-note chords.
        List<IntermediateChord> chords = new();
        IntermediateChord workingChord = null;
        long currentTime = -1;
        foreach (IntermediateChord chord in input)
        {
            long time = chord.absoluteTime;
            // If we have no working chord, make the current chord the working chord.
            if (workingChord is null)
            {
                currentTime = time;
                workingChord = chord;
                chords.Add(workingChord);
                continue;
            }
            // If the time of the current chord does not match that of the working chord,
            // add it to the list as a new working chord.
            if (time != currentTime)
            {
                currentTime = time;
                workingChord = chord;
                chords.Add(workingChord);
                continue;
            }
            // If the time of the current chord matches that of the working chord,
            // merge it into the working chord.
            workingChord.MergeChord(chord);
        }

        return chords.ToArray();
    }
}
