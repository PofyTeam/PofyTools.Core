using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
//using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Extensions
{
    public static class ArrayExtensions
    {
        public static T GetRandom<T>(this T[] array)
        {
            return array[Random.Range(0, array.Length)];
        }

        public static T TryGetRandom<T>(this T[] array)
        {
            T result = default(T);

            if (array.Length != 0)
                result = array[Random.Range(0, array.Length)];
            return result;
        }

        public static T GetNextRandom<T>(this T[] array, ref int lastRandomIndex)
        {
            int newIndex = lastRandomIndex;
            int length = array.Length;
            //Debug.Log("newIndex is " + newIndex);

            if (length > 1)
            {
                do
                {
                    newIndex = Random.Range(0, length);
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

        public static T LastMember<T>(this T[] array)
        {
            return array[array.Length - 1];
        }

    }
    public static class ListExtensions
    {
        public static T GetRandom<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        /// <summary>
        /// Gets the random element from the list or element's type default value.
        /// </summary>
        /// <returns>The random element or element's type default value.</returns>
        public static T TryGetRandom<T>(this List<T> list)
        {
            int count = list.Count;
            T result = default(T);

            if (count != 0)
                result = list[Random.Range(0, count)];
            return result;
        }

        public static T GetNextRandom<T>(this List<T> list, ref int lastRandomIndex)
        {
            int newIndex = lastRandomIndex;
            int length = list.Count;

            if (length > 1)
            {
                do
                {
                    newIndex = Random.Range(0, length);
                }
                while (lastRandomIndex == newIndex);
            }

            lastRandomIndex = newIndex;

            return list[newIndex];
        }

        public static T LastMember<T>(this List<T> list)
        {
            return list[list.Count - 1];
        }

        public static bool AddOnce<T>(this List<T> list, T element)
        {
            if (list.Contains(element))
                return false;
            list.Add(element);
            return true;
        }
    }
    public static class AnimatorExtenstion
    {
        public static bool InState(this Animator animator, int layer, int hashedNameState)
        {
            return animator.GetCurrentAnimatorStateInfo(layer).shortNameHash == hashedNameState;
        }

        public static float NormalizedStateTime(this Animator animator, int layer)
        {
            return animator.GetCurrentAnimatorStateInfo(layer).normalizedTime;
        }

        public static bool IsCurrentOrNextStateInfoTag(this Animator animator, int tagHash, int layerIndex = 0)
        {
            return animator.GetCurrentAnimatorStateInfo(layerIndex).tagHash == tagHash || animator.GetNextAnimatorStateInfo(layerIndex).tagHash == tagHash;
        }
    }
    public static class ColliderExtensions
    {

        public static float Radius(this Collider coll)
        {
            if (coll is CapsuleCollider)
            {
                return (coll as CapsuleCollider).radius;
            }
            else if (coll is BoxCollider)
            {
                return (coll as BoxCollider).bounds.size.magnitude;
            }
            else if (coll is SphereCollider)
            {
                return (coll as SphereCollider).radius;
            }
            else
            {
                Debug.Log("Unable to determine collider!");
                return 0;
            }
        }
    }
    public static class ConfigurableJointExtensions
    {

        public static void LockAngularMotion(this ConfigurableJoint joint)
        {
            joint.angularXMotion = ConfigurableJointMotion.Locked;
            joint.angularYMotion = ConfigurableJointMotion.Locked;
            joint.angularZMotion = ConfigurableJointMotion.Locked;
        }

        public static void LockMotion(this ConfigurableJoint joint)
        {
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
        }

        public static void LockAllMotion(this ConfigurableJoint joint)
        {
            joint.LockAngularMotion();
            joint.LockMotion();
        }
    }
    public static class LayerExtension
    {
        public static LayerMask AddLayerToMask(this LayerMask mask, params int[] layer)
        {
            for (int i = 0; i < layer.Length; i++)
            {
                mask = 1 << layer[i] | mask;
            }

            return mask;
        }

        public static LayerMask RemoveLayers(this LayerMask mask, params int[] layers)
        {
            foreach (var layer in layers)
            {
                mask = 1 << layer ^ mask;
            }

            return mask;
        }

        public static bool IsAnyLayerInMask(this LayerMask mask, params int[] layers)
        {
            foreach (var layer in layers)
            {
                if (mask == (mask | 1 << layer))
                {
                    return true;
                }
            }
            return false;
        }
    }
    public static class ComponentExtension
    {
        public static void DeleteComponentIfExists<T>(this Component comp)
        {
            var instance = comp.GetComponent<T>() as Object;
            if (instance != null)
            {
                GameObject.DestroyImmediate(instance);
            }
        }

        //Looks ugly - Destroy in iteration.
        public static void DeleteComponentInChildren<T>(this Component comp)
        {
            foreach (var item in comp.GetComponentsInChildren<T>())
            {
                GameObject.DestroyImmediate(item as Object);
            }
        }

        public static bool TryGetComponent<T>(this Component component, out T result) where T : Component
        {
            result = component.GetComponent<T>();
            return result != null;
        }
    }
    public static class CanvasExtensions
    {
        public static Vector2 WorldToCanvas(this Canvas canvas,
                                            Vector3 world_position,
                                            Camera camera = null)
        {
            if (camera == null)
            {
                camera = Camera.main;
            }

            var viewport_position = camera.WorldToViewportPoint(world_position);
            var canvas_rect = canvas.GetComponent<RectTransform>();

            return new Vector2((viewport_position.x * canvas_rect.sizeDelta.x) - (canvas_rect.sizeDelta.x * 0.5f),
                               (viewport_position.y * canvas_rect.sizeDelta.y) - (canvas_rect.sizeDelta.y * 0.5f));
        }

    }
    public static class EnumerationExtensions
    {

        public static bool Has<T>(this System.Enum type, T value)
        {
            return (((int)(object)type & (int)(object)value) == (int)(object)value);
        }

        public static bool Is<T>(this System.Enum type, T value)
        {
            return (int)(object)type == (int)(object)value;
        }


        public static T Add<T>(this System.Enum type, T value)
        {
            return (T)(object)(((int)(object)type | (int)(object)value));
        }


        public static T Remove<T>(this System.Enum type, T value)
        {
            return (T)(object)(((int)(object)type & ~(int)(object)value));
        }

        public static bool Is<T>(this System.Enum type, params T[] value)
        {
            var thisType = (int)(object)type;
            for (int i = 0; i < value.Length; i++)
            {
                if (thisType == (int)(object)value[i])
                    return true;
            }
            return false;
        }

        public static T Random<T>(this System.Enum type)
        {
            if (Mathf.IsPowerOfTwo((int)(object)type))
            {
                return (T)(object)type;
            }

            int value = 0;
            do
            {
                value = 1 << UnityEngine.Random.Range(0, 32);
            }
            while ((((int)(object)type & value) != value));

            return (T)(object)(value);
        }
    }
    public static class EventTriggerExtensions
    {
        public static void AddEventTriggerListener(this UnityEngine.EventSystems.EventTrigger trigger,
                                                    EventTriggerType eventType,
                                                    System.Action<BaseEventData> callback)
        {
            UnityEngine.EventSystems.EventTrigger.Entry entry = new UnityEngine.EventSystems.EventTrigger.Entry();
            entry.eventID = eventType;
            entry.callback = new UnityEngine.EventSystems.EventTrigger.TriggerEvent();
            entry.callback.AddListener(new UnityAction<BaseEventData>(callback));
            trigger.triggers.Add(entry);
        }
    }
    public static class StringExtensions
    {
        public static string ToTitle(this string value)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
        }

        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }

        //public static string ToTitle(this string str)
        //{
        //    int minLength = 2;
        //    string regexPattern = string.Format(@"^\w|\b\w(?=\w{{{0}}})", minLength);
        //    return Regex.Replace(str, regexPattern, m => m.Value.ToUpperInvariant());

        //}
    }
    public static class TransformExtensions
    {
        /// <summary>
        ///Iterates and destroyes all children
        /// </summary>
        /// <param name="transform"></param>
        public static void ClearChildren(this Transform transform)
        {
            Transform child = null;
            for (int i = transform.childCount - 1; i >= 0; --i)
            {
                child = transform.GetChild(i);
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    GameObject.DestroyImmediate(child.gameObject);
                else
#endif
                    GameObject.Destroy(child.gameObject);
            }
        }

        public static float ForwardToPointAngle(this Transform transform, Vector3 point, Space space = Space.World)
        {
            Vector3 axis = (space == Space.World) ? Vector3.up : transform.up;
            return Vector3.SignedAngle(point - transform.position, transform.forward, axis);
        }

        public static bool IsPointRight(this Transform transform, Vector3 point)
        {
            Plane directionPlane = new Plane(transform.right, transform.position);
            return directionPlane.GetSide(point);
        }

        public static bool IsPointAbove(this Transform transform, Vector3 point)
        {
            Plane directionPlane = new Plane(transform.up, transform.position);
            return directionPlane.GetSide(point);
        }

        public static bool IsPointInFront(this Transform transform, Vector3 point)
        {
            Plane directionPlane = new Plane(transform.forward, transform.position);
            return directionPlane.GetSide(point);
        }

        public static void ResetTransformation(this Transform trans)
        {
            trans.position = Vector3.zero;
            trans.localRotation = Quaternion.identity;
            trans.localScale = new Vector3(1, 1, 1);
        }

        #region Plane Utilities
        public static Plane GetXPlane(this Transform transform, Space space = Space.World)
        {
            return new Plane((space == Space.Self) ? transform.right : Vector3.right, transform.position);
        }

        public static Plane GetYPlane(this Transform transform, Space space = Space.World)
        {
            return new Plane((space == Space.Self) ? transform.up : Vector3.up, transform.position);
        }

        public static Plane GetZPlane(this Transform transform, Space space = Space.World)
        {
            return new Plane((space == Space.Self) ? transform.forward : Vector3.forward, transform.position);
        }
        #endregion
    }
    public static class AnimationCurveExtensions
    {
        public static float AverageValue(this AnimationCurve curve, float step = .1f)
        {
            var numberOfSteps = 0;
            var stepSum = 0f;
            for (float i = 0; i <= 1; i += step)
            {
                stepSum += curve.Evaluate(i);
                numberOfSteps++;
            }

            return stepSum / numberOfSteps;
        }
    }
    public static class RigidbodyExtensions
    {
        public static void EnableGravity(this Rigidbody rgb)
        {
            rgb.useGravity = true;
            rgb.isKinematic = false;
            rgb.interpolation = RigidbodyInterpolation.Interpolate;
        }

        public static void DisableGravity(this Rigidbody rgb)
        {
            rgb.useGravity = false;
            rgb.isKinematic = true;
            rgb.interpolation = RigidbodyInterpolation.None;
        }

        public static void ToggleGravity(this Rigidbody rgb, bool enabled)
        {
            if (enabled)
            {
                rgb.EnableGravity();
            }
            else
            {
                rgb.DisableGravity();
            }
        }
    }
    public static class RectTransformExtensions
    {
        public static void MoveLocalX(this RectTransform transform, float value)
        {
            var tr = transform.localPosition;
            tr.x = value;
            transform.localPosition = tr;
        }

        public static void MoveLocalY(this RectTransform transform, float value)
        {
            var tr = transform.localPosition;
            tr.y = value;
            transform.localPosition = tr;
        }
    }

    public static class Vector3Extenstion
    {

        #region Public Methods
        /// <summary>
        /// Get distance between two vectors
        /// </summary>
        /// <param name="pointA"></param>
        /// <param name="poitnB"></param>
        /// <returns></returns>
        public static float Distance(this Vector3 pointA, Vector3 poitnB)
        {
            return Vector3.Distance(pointA, poitnB);
        }

        /// <summary>
        /// Returns cross product of two vectors
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        //public static Vector3 Cross(this Vector3 lhs, Vector3 rhs)
        //{
        //    return Vector3.Cross(lhs, rhs);
        //}

        /// <summary>
        /// Returns normalized direction from one Vector to second
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static Vector3 NormalizedDirection(this Vector3 from, Vector3 to)
        {
            //var heading = to - from;
            //if (heading.sqrMagnitude == 0)
            //{
            //    return Vector3.zero;
            //}
            //return heading / heading.magnitude;

            //If the vector is too small to be normalized a zero vector will be returned. (https://docs.unity3d.com/ScriptReference/Vector3-normalized.html)
            return (to - from).normalized;
        }


        /// <summary>
        /// Hack, do not use as standard Vector2 function. Is number between two numbers
        /// </summary>
        /// <param name="v"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        //public static bool HBetweenValues(this Vector2 v, float number)
        //{
        //    return (v.x < number && v.y > number) || (v.x == v.y && v.x == number);

        //}
        #endregion

    }
}