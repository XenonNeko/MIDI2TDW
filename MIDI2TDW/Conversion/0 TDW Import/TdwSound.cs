using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Thirty Dollar Website sound
/// </summary>
[Serializable]
public class TdwSound
{
    /// <summary>
    /// The name of this sound (as it appears on thirtydollar.website)
    /// </summary>
    public string name;
    /// <summary>
    /// The origin of this sound (as it appears on thirtydollar.website)
    /// </summary>
    public string origin;
    /// <summary>
    /// The string representation of this sound (as used internally by the TDW file format)
    /// </summary>
    public string symbol;
    [JsonIgnore]
    public Sprite icon;
    /// <summary>
    /// Whether or not I could be bothered to check the pitch of this sound
    /// </summary>
    public bool hasPitch;
    /// <summary>
    /// My best guess of the MIDI pitch of this sound
    /// </summary>
    public float midiPitch;
    public override string ToString()
    {
        return $"({name} ({origin}) {symbol} {icon})";
    }
}
