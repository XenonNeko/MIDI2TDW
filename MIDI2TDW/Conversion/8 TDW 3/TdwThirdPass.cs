using System.Collections;
using System.Collections.Generic;

public static class TdwThirdPass
{
    public static bool doVolumeActions = false;

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
                        if (doVolumeActions)
                        {
                            tdwEvents.Add(new TdwVolumeAction() { volume = sound.volume });
                        }
                        tdwEvents.Add(new TdwSoundEvent() { symbol = sound.tdwSound.symbol, pitch = sound.pitchParameter } );
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
