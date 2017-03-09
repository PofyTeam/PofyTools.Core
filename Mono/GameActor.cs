namespace PofyTools
{
    using UnityEngine;
    using System.Collections;

    public class GameActor : MonoBehaviour, ISubscribable, IStateMachine
    {
        public bool selfSubscribe = true;

        #region ISubscribable implementation

        protected bool _isSubscribed = false;

        public virtual void Subscribe()
        {
            Unsubscribe();
            this._isSubscribed = true;
        }

        public virtual void Unsubscribe()
        {
            this._isSubscribed = false;
        }

        public bool isSubscribed
        {
            get
            {
                return this._isSubscribed;
            }

        }

        protected virtual void OnDestroy()
        {
            if (this._isSubscribed)
                Unsubscribe();
        }

        #endregion

        // Use this for initialization
        protected virtual void Start()
        {
            if (this.selfSubscribe)
                Subscribe();
			
            RemoveAllStates();
        }

        protected virtual void Awake()
        {
            //
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

        public UpdateDelegate currentState
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