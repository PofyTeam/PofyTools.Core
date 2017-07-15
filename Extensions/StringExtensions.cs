namespace PofyTools
{
using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System.Globalization;

    public static class StringExtensions
    {
        public static string ToTitle(this string value)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
        }
    }
}