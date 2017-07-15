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
        public static T GetRandom<T>(this List<T>list)
        {
            int count = list.Count;
            T result = default(T);

            if (count != 0)
                result = list[Random.Range(0, count - 1)];
            return result;
        }
    }
}