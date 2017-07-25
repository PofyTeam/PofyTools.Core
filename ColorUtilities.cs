namespace PofyTools
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class ColorUtilities
    {
        public static Color32 AverageColorFromTexture(Texture2D texture)
        {
            Color32[] texColors = texture.GetPixels32();

            int total = texColors.Length;

            float r = 0;
            float g = 0;
            float b = 0;

            for (int i = 0; i < total; i++)
            {

                r += texColors[i].r;

                g += texColors[i].g;

                b += texColors[i].b;

            }

            return new Color32((byte)(r / total), (byte)(g / total), (byte)(b / total), 255);
        }
    }
}