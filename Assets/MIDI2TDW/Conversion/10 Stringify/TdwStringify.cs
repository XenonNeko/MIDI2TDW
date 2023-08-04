using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class TdwStringify
{
    public static TResult[] ForEach<TArrayType, TResult>(this TArrayType[] array, Func<TArrayType, TResult> func)
    {
        TResult[] result = new TResult[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            result[i] = func.Invoke(array[i]);
        }
        return result;
    }

    public static bool doIntro;
    private const string introTdw = "!speed@960|bong|!pulse@1,1|!loopmany@30|!speed@65|mrbeast|!volume@400|!flash|!combine|boom|!volume@100|!cut|!bg@#0078d7,0|🚫|_pause|!bg@#36393c,0";

    public static string Stringify(TdwEvent[] input)
    {
        string intro = doIntro ? introTdw : string.Empty;
        return intro + string.Join('|', input.ForEach(e => e.ToString()));
    }
}
