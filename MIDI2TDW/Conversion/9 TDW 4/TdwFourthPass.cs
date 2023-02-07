using Melanchall.DryWetMidi.Common;
using System.Collections;
using System.Collections.Generic;

public static class TdwFourthPass
{
    private static readonly double SEVEN_BIT_MAX_SQR = (SevenBitNumber.MaxValue * SevenBitNumber.MaxValue) / 100.0;

    public static TdwEvent[] FourthPass(TdwEvent[] input)
    {
        // Remove redundant Volume Actions while normalizing the volumes of the remaining actions.
        double volume = 0;
        List<TdwEvent> tdwEvents = new();
        for (int i = 0; i < input.Length; i++)
        {
            TdwEvent tdwEvent = input[i];
            switch (tdwEvent)
            {
                case TdwVolumeAction volumeAction:
                    if (volumeAction.volume == volume)
                    {
                        break;
                    }
                    volume = volumeAction.volume;
                    volumeAction.volume /= SEVEN_BIT_MAX_SQR;
                    tdwEvents.Add(volumeAction);
                    break;
                default:
                    tdwEvents.Add(tdwEvent);
                    break;
            }
        }
        return tdwEvents.ToArray();
    }
}
