using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PofyTools
{
    //[System.Serializable]
    public struct Cone
    {
        //X is cone angle Y is cone height 
        public Vector2 data
        {
            get; private set;
        }

        //from tip to base center
        public Vector3 direction
        {
            get; private set;
        }

        //tip world position
        public Vector3 tipPosition
        {
            get; private set;
        }

        #region Constructors

        public Cone (Vector3 tipPosition, Vector3 direction, Vector2 data)
        {
            this.tipPosition = tipPosition;
            this.direction = direction;
            this.data = data;
        }

        public Cone (float tipAngle = 1f, float coneHeight = 1f, Vector3 tipPosition = default (Vector3), Vector3 direction = default (Vector3))
        {
            this.tipPosition = tipPosition;
            this.direction = direction;

            this.data = new Vector2 (tipAngle, coneHeight);
        }

        #endregion

        #region API
        /// <summary>
        /// Check if the cone contains world space point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool Contains (Vector3 point)
        {
            if ((this.tipPosition - point).magnitude < data.y)
            {
                return InfinityContains (point);
            }

            return false;
        }

        /// <summary>
        /// Checks if infinity cone contains world space point. (Does not check against distance)
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool InfinityContains (Vector3 point)
        {
            Vector3 directionToPoint = point - this.tipPosition;
            if (Vector3.Angle (directionToPoint, this.direction) < this.data.x)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Set direction to the point
        /// </summary>
        /// <param name="point"></param>
        public void LookAt (Vector3 point)
        {
            this.direction = (point - this.tipPosition).normalized;
        }

        /// <summary>
        /// Sets new direction.
        /// </summary>
        /// <param name="direction"></param>
        public void SetDirection (Vector3 direction)
        {
            this.direction = direction.normalized;
        }


        public void SetTipPosition (Vector3 worldPosition)
        {
            this.tipPosition = worldPosition;
        }

        public void SetHeight (float height)
        {
            Vector2 newData = this.data;
            newData.y = height;
            this.data = newData;
        }

        public void SetAngle (float angleInDegrees)
        {
            Vector2 newData = this.data;
            newData.x = angleInDegrees;
            this.data = newData;
        }

        public void SetData (float angleInDegrees, float height)
        {
            Vector2 newData = new Vector2 (angleInDegrees, height);
            this.data = newData;
        }

        public void Draw (Color color)
        {
            Color tempColor = Gizmos.color;
            Gizmos.color = color;

            Gizmos.DrawWireSphere (this.tipPosition, 0.05f);
            float rad = this.data.y * Mathf.Tan (this.data.x * Mathf.Deg2Rad);

            Vector3 endPoint = this.tipPosition + (this.direction * this.data.y);
            Gizmos.DrawLine (this.tipPosition, endPoint);
            Gizmos.DrawWireSphere (endPoint, rad);

            //Gizmos.DrawFrustum (this.tipPosition, this.data.x, this.data.y, 0.1f, 1f);
            Gizmos.color = tempColor;
        }

        //TODO
        public Vector3 GetRandomPointInisde ()
        {
            Vector3 result = default (Vector3);
            float height = (Random.Range (0, this.data.y));
            Vector3 distance = this.tipPosition + direction * height;
            float radius = GetRadiusAtDistanceFromTip (height);
            radius = Random.Range (0, radius);

            return result;
        }

        public float GetRadiusAtDistanceFromTip (float distance)
        {
            return distance * Mathf.Tan (this.data.x * Mathf.Deg2Rad);
        }

        #endregion
    }
}
