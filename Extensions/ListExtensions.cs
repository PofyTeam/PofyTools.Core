namespace PofyTools
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class ListExtensions
    {
        /// <summary>
        /// Gets the random element from the list or element's type default value.
        /// </summary>
        /// <returns>The random element or element's type default value.</returns>
        public static T GetRandom<T> (this List<T> list)
        {
            int count = list.Count;
            T result = default (T);

            if (count != 0)
                result = list[Random.Range (0, count)];
            return result;
        }

        public static T GetNextRandom<T> (this List<T> list, ref int lastRandomIndex)
        {
            int newIndex = lastRandomIndex;
            int length = list.Count;

            if (length > 1)
            {
                do
                {
                    newIndex = Random.Range (0, length);
                }
                while (lastRandomIndex == newIndex);
            }

            lastRandomIndex = newIndex;

            return list[newIndex];
        }
    }
}