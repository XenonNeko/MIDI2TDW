using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A collection of one or more <see cref="IntermediateSound"/>s which occur simultaneously
/// </summary>
[Serializable]
public class IntermediateChord
{
    /// <summary>
    /// The absolute time of this chord 
    /// </summary>
    public long absoluteTime;
    /// <summary>
    /// The sounds in this chord
    /// </summary>
    public List<IntermediateSound> sounds;
    public IntermediateSound[] GetSounds()
    {
        return sounds.ToArray();
    }
    public void AddSound(IntermediateSound sound)
    {
        sounds = new() { sound };
    }
    public void MergeChord(IntermediateChord next)
    {
        sounds.Add(next.sounds[0]);
    }
    /// <summary>
    /// The number of microseconds until the next chord
    /// </summary>
    public long deltaTime;
}
