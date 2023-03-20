using Melanchall.DryWetMidi.Common;
using System.Collections;
using System.Collections.Generic;

public static class TdwThirdPass
{
    public static bool doVolumeParameters = false;
    private static readonly float SEVEN_BIT_MAX_SQR = SevenBitNumber.MaxValue * SevenBitNumber.MaxValue;

    public static TdwEvent[] ThirdPass(TdwEvent[] input)
    {
        // Split TdwIntermediateSoundCollections into pairs of Volume Actions and Sounds joined with Combine Actions.
        List<TdwEvent> tdwEvents = new();
        for (int i = 0; i < input.Length; i++)
        {
            TdwEvent tdwEvent = input[i];
            switch (tdwEvent)
            {
                case TdwIntermediateSoundCollection soundCollection:
                    IntermediateSound[] sounds = soundCollection.sounds;
                    for (int s = 0; s < sounds.Length; s++)
                    {
                        IntermediateSound sound = sounds[s];
                        TdwSoundEvent soundEvent = new()
                        {
                            symbol = sound.tdwSound.symbol,
                            pitch = sound.pitchParameter,
                            volume = doVolumeParameters ? (sound.volume / SEVEN_BIT_MAX_SQR) : 1f
                        };
                        tdwEvents.Add(soundEvent);
                        if (s == sounds.Length - 1)
                        {
                            continue;
                        }
                        tdwEvents.Add(new TdwCombineAction());
                    }
                    break;
                default:
                    tdwEvents.Add(tdwEvent);
                    break;
            }
        }
        return tdwEvents.ToArray();
    }
}
