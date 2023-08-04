using System.Collections;
using System.Collections.Generic;

public static class TdwSecondPass 
{
    public static TdwEvent[] SecondPass(TdwEvent[] input)
    {
        // Remove redundant tempo actions.
        string tempo = "";
        List<TdwEvent> tdwEvents = new();
        for (int i = 0; i < input.Length; i++)
        {
            TdwEvent tdwEvent = input[i];
            switch (tdwEvent)
            {
                case TdwTempoAction tempoAction:
                    if (tempoAction.tempo == tempo)
                    {
                        break;
                    }
                    tempo = tempoAction.tempo;
                    tdwEvents.Add(tempoAction);
                    break;
                default:
                    tdwEvents.Add(tdwEvent);
                    break;
            }
        }
        return tdwEvents.ToArray();
    }
}
