using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PofyTools
{

    public abstract class StateableActor : MonoBehaviour, IStateable, ISubscribable, IInitializable, ITransformable
    {
        #region Variables

        public bool removeAllStatesOnStart = true;

        protected List<IState> _stateStack;

        #endregion

        #region IInitializable implementation

        protected bool _isInitialized = false;

        public virtual bool Initialize()
        {
            if (!this.isInitialized)
            {
                this._selfTransform = this.transform;
                ConstructAvailableStates();
                InitializeStateStack();

                this._isInitialized = true;
                return true;
            }
            return false;
        }

        public virtual bool isInitialized
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
            if (!this.isSubscribed)
            {
                this._isSubscribed = true;
                return true;
            }
            return false;
        }

        public virtual bool Unsubscribe()
        {
            if (this.isSubscribed)
            {
                this._isSubscribed = false;
                return true;
            }
            return false;
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
            PurgeStateStack();
            Unsubscribe();
        }

        #endregion

        #region IStateable implementation

        public void AddState(IState state)
        {
            state.EnterState();
            if (!this._stateStack.Contains(state))
            {
				
                if (state.hasUpdate)
                {
                    this._stateStack.Add(state);
                    this.enabled = true;
                }
                else
                    state.ExitState();
            }
            else
            {
                state.ExitState();
                state.EnterState();
            }
        }

        public void RemoveState(IState state)
        {
            if (this._stateStack.Remove(state))
            {
                state.ExitState();
            }

            if (this._stateStack.Count == 0)
                this.enabled = false;
        }

        public void RemoveAllStates(bool endStates = true)
        {
			
            if (this._stateStack != null && endStates)
            {
                int count = this._stateStack.Count;
                IState state = null;
                for (int i = count - 1; i >= 0; --i)
                {
                    state = this._stateStack[i];
                    this._stateStack.RemoveAt(i);
                    state.ExitState();
                }
            }
            PurgeStateStack();
        }

        public void PurgeStateStack()
        {
            if (this._stateStack != null)
                this._stateStack.Clear();

            this.enabled = false;
        }

        public void StackState(IState state)
        {
            state.EnterState();
            if (state.hasUpdate)
            {
				
                this._stateStack.Add(state);
            }
            else
            {
                AddState(state);
            }
        }

        public void SetToState(IState state)
        {
            RemoveAllStates();
            AddState(state);
        }

        #endregion

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

        #region Mono

        protected virtual void Awake()
        {
            Initialize();
        }
			
        // Use this for initialization
        protected virtual void Start()
        {
            Subscribe();

            if (this.removeAllStatesOnStart)
                PurgeStateStack();
        }
	
        // Update is called once per frame
        protected void Update()
        {
            IState state = null;

            for (int i = this._stateStack.Count - 1; i >= 0; --i)
            {
                state = this._stateStack[i];

                if (state.UpdateState())
                {
                    this._stateStack.RemoveAt(i);
                    state.ExitState();
                }
            }
        }

        protected void FixedUpdate()
        {
            IState state = null;

            for (int i = this._stateStack.Count - 1; i >= 0; --i)
            {
                state = this._stateStack[i];

                if (state.FixedUpdateState())
                {
                    this._stateStack.RemoveAt(i);
                    state.ExitState();

                }
            }
        }

        protected void LateUpdate()
        {
            IState state = null;

            for (int i = this._stateStack.Count - 1; i >= 0; --i)
            {
                state = this._stateStack[i];

                if (state.LateUpdateState())
                {
                    this._stateStack.RemoveAt(i);
                    state.ExitState();
                }
            }
        }

        #endregion

        #region States

        public virtual void ConstructAvailableStates()
        {
        }

        public virtual void InitializeStateStack()
        {
            this._stateStack = new List<IState>();
        }

        #endregion
    }

    public class StateObject<T>:IState where T:MonoBehaviour
    {
        public IStateDelegate onEnter = IStateIdle;
        public IStateDelegate onExit = IStateIdle;

        public T controlledObject
        {
            get;
            protected set;
        }

        public bool hasUpdate
        {
            get;
            protected set;
        }

        public bool isInitialized
        {
            get;
            protected set;
        }

        public bool isActive
        {
            get;
            protected set;
        }


        public static void IStateIdle()
        {
        }

        #region constructor

        public StateObject()
        {
        }

        public StateObject(T controlledObject)
        {
            this.controlledObject = controlledObject;
            InitializeState();
        }

        #endregion

        #region IState implementation

        public virtual void InitializeState()
        {
            this.isInitialized = true;
            if (this.controlledObject = null)
            {
                Debug.LogError(this.ToString() + " has no controlled object");
            }
        }

        public virtual void EnterState()
        {
            this.isActive = true;
            this.onEnter(this);
        }

        public virtual bool UpdateState()
        {
            //return true on exit condition
            return false;
        }

        public virtual bool FixedUpdateState()
        {
            //do fixed stuff
            return false;
        }

        public virtual bool LateUpdateState()
        {
            //do late state
            return false;
        }

        public virtual void ExitState()
        {
            this.isActive = false;
            this.onEnter(this);
        }

        #endregion
    }

    public class TimedStateObject<T>: StateObject<T> where T:MonoBehaviour
    {
        protected Range _timeRange;
        protected AnimationCurve _curve;

        public TimedStateObject(T controlledObject, Range timeRange, AnimationCurve curve)
        {
            this.controlledObject = controlledObject;
            this._timeRange = timeRange;
            this._curve = curve;

            InitializeState();
        }

        public virtual void InitializeState(float duration, AnimationCurve curve)
        {
            this.hasUpdate = true;

            base.InitializeState();
        }
    }

    public interface IState
    {
        void InitializeState();

        void EnterState();

        bool UpdateState();

        bool FixedUpdateState();

        bool LateUpdateState();

        void ExitState();

        bool hasUpdate{ get; }
    }

    public interface IStateable
    {
        void InitializeStateStack();

        void ConstructAvailableStates();

        void AddState(IState state);

        void RemoveState(IState state);

        void RemoveAllStates(bool endStates = true);

        void PurgeStateStack();

        void StackState(IState state);

    }

    public delegate void IStateDelegate(IState state);
}