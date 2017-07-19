using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayExtensions
{

    public static void GetRandom<T>(this T[] array)
    {
        T result = default(T);

        if (array.Length != 0)
            result = array[Random.Range(0, array.Length)];
        return result;
    }
}
