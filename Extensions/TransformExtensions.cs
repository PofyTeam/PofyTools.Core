namespace PofyTools
{
    using UnityEngine;

    public static class TransformExtensions
    {
        public static void ClearChildren (this Transform transform)
        {
            Transform child = null;
            for (int i = transform.childCount - 1; i >= 0; --i)
            {
                child = transform.GetChild (i);
                if (Application.isPlaying) GameObject.Destroy (child.gameObject); else GameObject.DestroyImmediate (child.gameObject);
            }
        }

        public static float ForwardToPointAngle (this Transform transform, Vector3 point)
        {
            return Vector3.Angle (transform.forward, (point - transform.position).normalized);
        }

        public static bool IsPointLeft (this Transform transform, Vector3 point)
        {
            Plane directionPlane = new Plane (transform.right, transform.position);
            return directionPlane.GetSide (point);
        }

        public static bool IsPointBelow (this Transform transform, Vector3 point)
        {
            Plane directionPlane = new Plane (transform.up, transform.position);
            return directionPlane.GetSide (point);
        }

        public static bool IsPointBehind (this Transform transform, Vector3 point)
        {
            Plane directionPlane = new Plane (transform.forward, transform.position);
            return directionPlane.GetSide (point);
        }
    }
}