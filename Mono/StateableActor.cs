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

        #region IInitializable

        protected bool _isInitialized = false;

        public virtual bool Initialize()
        {
            if (!this.IsInitialized)
            {
                this._selfTransform = this.transform;
                ConstructAvailableStates();
                InitializeStateStack();

                this._isInitialized = true;
                return true;
            }
            return false;
        }

        public virtual bool IsInitialized
        {
            get
            {
                return this._isInitialized;
            }
        }

        #endregion

        #region ISubscribable

        protected bool _isSubscribed = false;

        public virtual bool Subscribe()
        {
            if (!this.IsSubscribed)
            {
                this._isSubscribed = true;
                return true;
            }
            return false;
        }

        public virtual bool Unsubscribe()
        {
            if (this.IsSubscribed)
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
            PurgeStateStack();
            Unsubscribe();
        }

        #endregion

        #region IStateable

        public void AddState(IState state)
        {
            if (state == null)
            {
                if (this._stateStack.Count == 0)
                    this.enabled = false;
                return;
            }

            if (!this._stateStack.Contains(state))
            {
                state.EnterState();
                if (state.HasUpdate)
                {
                    this._stateStack.Add(state);
                    this._stateStack.Sort((x, y) => x.Priority.CompareTo(y.Priority));
                    this.enabled = true;
                }
                else
                    state.ExitState();
            }
            else
            {
                if (!state.IgnoreStacking)
                {
                    state.ExitState();
                    state.EnterState();
                }
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

        public void RemoveAllStates(bool endPermanent = false, int priority = 0)
        {

            if (this._stateStack != null)
            {
                int count = this._stateStack.Count;
                IState state = null;
                for (int i = count - 1; i >= 0; --i)
                {
                    state = this._stateStack[i];
                    if (!state.IsPermanent || endPermanent)
                    {
                        if (state.Priority > priority)
                        {
                            this._stateStack.RemoveAt(i);
                            state.ExitState();
                        }
                    }
                }
            }
        }

        public void PurgeStateStack()
        {
            if (this._stateStack != null)
            {
                foreach (var state in this._stateStack)
                {
                    state.Deactivate();
                }
                this._stateStack.Clear();
            }
            this.enabled = false;
        }

        public void StackState(IState state)
        {
            if (state.IgnoreStacking)
            {
                if (this._stateStack.Contains(state))
                    return;
            }

            state.EnterState();
            if (state.HasUpdate)
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
            if (state != null)
                AddState(state);
        }

        #endregion

        #region ITransformable

        protected Transform _selfTransform;

        public Transform SelfTransform
        {
            get
            {
                if (this._selfTransform == null)
                {
                    Debug.LogError(this.name + " : " + this.GetType().ToString());
                    return this.transform;
                }
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
        protected virtual void Update()
        {
            IState state = null;

            for (int i = this._stateStack.Count - 1; i >= 0 && i < this._stateStack.Count; --i)
            {

                state = this._stateStack[i];

                if (state.UpdateState())
                {
                    this._stateStack.RemoveAt(i);
                    state.ExitState();
                }
            }
        }

        protected virtual void FixedUpdate()
        {
            IState state = null;

            for (int i = this._stateStack.Count - 1; i >= 0 && i < this._stateStack.Count; --i)
            {
                state = this._stateStack[i];

                if (state.FixedUpdateState())
                {
                    this._stateStack.RemoveAt(i);
                    state.ExitState();

                }
            }
        }

        protected virtual void LateUpdate()
        {
            IState state = null;

            for (int i = this._stateStack.Count - 1; i >= 0 && i < this._stateStack.Count; --i)
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

    public class StateObject<T> : IState where T : MonoBehaviour
    {

        public T ControlledObject
        {
            get;
            protected set;
        }

        public bool HasUpdate
        {
            get;
            protected set;
        }

        public bool IsInitialized
        {
            get;
            protected set;
        }

        public bool IsActive
        {
            get;
            protected set;
        }

        public void Deactivate()
        {
            this.IsActive = false;
        }

        public bool IgnoreStacking
        {
            get;
            protected set;
        }

        public bool IsPermanent
        {
            get;
            protected set;
        }

        public int Priority
        {
            get; set;
        }

        #region Constructor

        public StateObject()
        {
        }

        public StateObject(T controlledObject)
        {
            this.ControlledObject = controlledObject;
            InitializeState();
        }

        #endregion

        #region IState implementation

        public virtual void InitializeState()
        {
            this.IsInitialized = true;
            if (this.ControlledObject == null)
            {
                Debug.LogError(this.ToString() + " has no controlled object");
            }
        }

        public virtual void EnterState()
        {
            this.IsActive = true;
            //this.onEnter (this);
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

            this.IsActive = false;
            //this.onExit (this);
        }

        #endregion

        public T this[int arg]
        {
            get
            {
                return this.ControlledObject;
            }
        }

    }

    public class TimedStateObject<T> : StateObject<T> where T : MonoBehaviour
    {
        protected Range _timeRange;
        protected AnimationCurve _curve;

        public TimedStateObject(T controlledObject, Range timeRange, AnimationCurve curve)
        {
            this.ControlledObject = controlledObject;
            this._timeRange = timeRange;
            this._curve = curve;

            InitializeState();
        }

        public TimedStateObject(T controlledObject, float duration, AnimationCurve curve)
            : this(controlledObject, new Range(duration), curve)
        {
        }

        public TimedStateObject(T controlledObject, float duration)
            : this(controlledObject, new Range(duration), null)
        {
        }

        public TimedStateObject(T controlledObject)
            : this(controlledObject, new Range(1), null)
        {
        }

        public virtual void InitializeState(float duration, AnimationCurve curve)
        {
            this.HasUpdate = true;

            base.InitializeState();
        }

        public override void InitializeState()
        {
            this.HasUpdate = true;
            base.InitializeState();
        }
    }

    public class TimerStateObject<T> : StateObject<T> where T : MonoBehaviour
    {
        public Timer timer = null;

        public TimerStateObject(T controlledObject, float timerDuration) : this(controlledObject, new Timer("timer", timerDuration))
        {
        }

        public TimerStateObject(T controlledObject, Timer timer) : base(controlledObject)
        {
            this.timer = timer;
        }

        public override void InitializeState()
        {
            this.HasUpdate = true;
            base.InitializeState();
        }
    }

    #region Utility States
    public class BackButtonListenerState : StateObject<MonoBehaviour>
    {
        public UpdateDelegate onBackButton;

        public BackButtonListenerState(MonoBehaviour controlledObject)
            : base(controlledObject)
        {
        }

        public void VoidIdle()
        {
        }

        public override void InitializeState()
        {
            this.onBackButton = VoidIdle;
            this.HasUpdate = true;
            base.InitializeState();
        }

        public override bool LateUpdateState()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                this.onBackButton.Invoke();
            }
            return false;
        }

    }

    public class DelegateStack : StateObject<MonoBehaviour>
    {
        public UpdateDelegate updater;

        void VoidIdle()
        {

        }

        public DelegateStack(MonoBehaviour controlledObject)
            : base(controlledObject)
        {
        }

        public override void InitializeState()
        {
            this.HasUpdate = true;
            this.updater = VoidIdle;
        }

        public override bool UpdateState()
        {
            this.updater();
            return false;
        }
    }

    #endregion

    public interface IState
    {
        void InitializeState();

        void EnterState();

        bool UpdateState();

        bool FixedUpdateState();

        bool LateUpdateState();

        void ExitState();

        bool HasUpdate { get; }
        bool IsPermanent { get; }
        int Priority { get; }
        bool IgnoreStacking { get; }

        bool IsActive
        {
            get;
        }

        void Deactivate();
    }

    public interface IStateable
    {
        void InitializeStateStack();

        void ConstructAvailableStates();

        void AddState(IState state);

        void RemoveState(IState state);

        void RemoveAllStates(bool endPermanent = false, int priority = 0);

        void PurgeStateStack();

        void StackState(IState state);

    }

    public delegate void IStateDelegate(IState state);
}