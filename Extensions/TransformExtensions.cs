namespace PofyTools
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class TransformExtensions
    {
        public static void ClearChildren(this Transform transform)
        {
            Transform child = null;
            for (int i = transform.childCount - 1; i >= 0; --i)
            {
                child = transform.GetChild(i);
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}