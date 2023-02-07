using System;
using System.Collections;
using System.Collections.Generic;

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

    public static string Stringify(TdwEvent[] input)
    {
        return string.Join('|', input.ForEach(e => e.ToString()));
    }
}
