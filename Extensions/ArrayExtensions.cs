using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayExtensions
{

    public static T GetRandom<T> (this T[] array)
    {
        T result = default (T);

        if (array.Length != 0)
            result = array[Random.Range (0, array.Length)];
        return result;
    }

    public static T GetNextRandom<T> (this T[] array, ref int lastRandomIndex)
    {
        int newIndex = lastRandomIndex;
        int length = array.Length;
        Debug.Log ("newIndex is " + newIndex);

        if (length > 1)
        {
            do
            {
                newIndex = Random.Range (0, length);
            }
            while (lastRandomIndex == newIndex);
        }
        else
        {
            newIndex = 0;
        }

        lastRandomIndex = newIndex;

        return array[newIndex];
    }
}
