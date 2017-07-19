namespace PofyTools
{
    using UnityEngine;
    using System.Collections;

    public abstract class BaseDetector2D : MonoBehaviour, ICollidable2D, ITransformable, IInitializable
    {
        public MonoBehaviour target;
        public bool detectStay = false;
        public bool detectExit = false;

        #region ITransformable implementation

        protected Transform _selfTransform;

        public Transform selfTransform
        {
            get
            {
                return this._selfTransform;
            }
        }

        #endregion

        #region ICollidable implementation

        protected Collider2D _selfCollider2D;

        public Collider2D selfCollider2D
        {
            get
            {
                return this._selfCollider2D;
            }
        }

        protected Rigidbody2D _selfRigidbody2D;

        public Rigidbody2D selfRigidbody2D
        {
            get
            {
                return this._selfRigidbody2D;
            }
        }

        #endregion

        #region Mono

        protected virtual void Awake()
        {
            Initialize();
        }

        #endregion

        #region IInitializable implementation

        public bool Initialize()
        {
            if (!this.isInitialized)
            {
                this._selfTransform = this.transform;
                this._selfCollider2D = GetComponent<Collider2D>();
                this._selfRigidbody2D = GetComponent<Rigidbody2D>();
                this.isInitialized = true;
                return true;
            }
            return false;
        }

        public bool isInitialized
        {
            get;
            protected set;
        }

        #endregion


    }
}
