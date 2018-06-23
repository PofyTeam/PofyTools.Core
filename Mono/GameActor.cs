namespace PofyTools
{
    using UnityEngine;
    using System.Collections;

    public class GameActor : MonoBehaviour,IInitializable, ISubscribable, IStateMachine
    {
        public bool selfSubscribe = true;

        #region IInitializable implementation

        protected bool _isInitialized = false;

        public virtual bool Initialize()
        {
            if (!this._isInitialized)
            {
                this._isInitialized = true;
                return true;
            }
            return false;
        }

        public bool IsInitialized
        {
            get
            {
                return this._isInitialized;
            }
        }

        #endregion

        #region ISubscribable implementation

        protected bool _isSubscribed = false;

        public virtual bool Subscribe()
        {
            if (!this._isSubscribed)
            {
                this._isSubscribed = true;
                return true;
            }

            return false;
        }

        public virtual bool Unsubscribe()
        {
            if (this._isSubscribed)
            {
                this._isSubscribed = false;
                return true;
            }

            return false;
        }

        public bool IsSubscribed
        {
            get
            {
                return this._isSubscribed;
            }

        }

        protected virtual void OnDestroy()
        {
            Unsubscribe();
        }

        #endregion

        // Use this for initialization
        protected virtual void Start()
        {
            Subscribe();
            RemoveAllStates();
        }

        protected virtual void Awake()
        {
            Initialize();
        }

        #region IStateMachine implementation

        protected UpdateDelegate _updater = null;

        protected virtual void Update()
        {
            if (this._updater != null)
                this._updater();
        }

        public void AddState(UpdateDelegate state)
        {
            this._updater -= state;
            this._updater += state;
            this.enabled = true;
            //return this._updater;
        }

        public void RemoveState(UpdateDelegate state)
        {
            this._updater -= state;
            if (this._updater == null)
            {
                this.enabled = false;
            }
            //return this._updater;
        }

        public void RemoveAllStates()
        {
            this._updater = null;
            this.enabled = false;
            //return this._updater;
        }

        public void SetToState(UpdateDelegate state)
        {
            this._updater = state;
            this.enabled = true;
            //return this._updater;
        }


        public void StackState(UpdateDelegate state)
        {
            this._updater += state;
            this.enabled = true;
        }

        public UpdateDelegate CurrentState
        {
            get
            {
                return this._updater;
            }
        }

        #endregion
    }

    public delegate void UpdateDelegate();
}