using System;

/// <summary>
/// A TDW Sound analogue where the sound, pitch, and volume are all combined into one data structure
/// </summary>
[Serializable]
public class IntermediateSound
{
    /// <summary>
    /// The Thirty Dollar Website sound
    /// </summary>
    public TdwSound tdwSound;
    /// <summary>
    /// The pitch parameter to use for this sound
    /// </summary>
    public float pitchParameter;
    /// <summary>
    /// The desired volume of this sound
    /// </summary>
    public int volume;
}
