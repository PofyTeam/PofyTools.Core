using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PofyTools
{

	public abstract class StateableActor : MonoBehaviour, IStateable//, ITransformable, ISubscribable
	{
		#region Variables

		public bool selfSubscribe = true;
		public bool removeAllStatesOnStart = true;

		protected List<IState> _stateStack;

		#endregion

		#region ISubscribable implementation

		protected bool _isSubscribed = false;

		public virtual void Subscribe ()
		{
			Unsubscribe ();
			this._isSubscribed = true;
		}

		public virtual void Unsubscribe ()
		{
			this._isSubscribed = false;
		}

		public bool isSubscribed {
			get {
				return this._isSubscribed;
			}

		}

		protected virtual void OnDestroy ()
		{
			//base.OnDestroy ();
			RemoveAllStates ();
			if (this._isSubscribed)
				Unsubscribe ();
		}

		#endregion

		#region IStateable implementation

		public void AddState (IState state)
		{
			state.EnterState ();
			if (!this._stateStack.Contains (state)) {
				
				if (state.hasUpdate) {
					this._stateStack.Add (state);
					this.enabled = true;
				} else
					state.ExitState ();
			}
		}

		public void RemoveState (IState state)
		{
			if (this._stateStack.Remove (state)) {
				state.ExitState ();
			}

			if (this._stateStack.Count == 0)
				this.enabled = false;
		}

		public void RemoveAllStates (bool endStates = true)
		{
			
			if (this._stateStack != null && endStates) {
				int count = this._stateStack.Count;
				IState state = null;
				for (int i = count - 1; i >= 0; --i) {
					state = this._stateStack [i];
					this._stateStack.RemoveAt (i);
					state.ExitState ();
				}
			}
			PurgeStateStack ();
		}

		public void PurgeStateStack ()
		{
			if (this._stateStack != null)
				this._stateStack.Clear ();

			this.enabled = false;
		}

		public void StackState (IState state)
		{
			state.EnterState ();
			if (state.hasUpdate) {
				
				this._stateStack.Add (state);
			} else {
				AddState (state);
			}
		}

		public void SetToState (IState state)
		{
			RemoveAllStates ();
			AddState (state);
		}

		#endregion

		#region ITransformable implementation

		protected Transform _selfTransform;

		public Transform selfTransform {
			get {
				return this._selfTransform;
			}
		}

		#endregion

		#region Mono

		protected virtual void Awake ()
		{
			this._selfTransform = this.transform;
			ConstructAvailableStates ();
			InitializeStateStack ();
		}
			
		// Use this for initialization
		protected virtual void Start ()
		{
			if (this.selfSubscribe)
				Subscribe ();

			if (this.removeAllStatesOnStart)
				PurgeStateStack ();
		}
	
		// Update is called once per frame
		protected void Update ()
		{
			IState state = null;

			for (int i = this._stateStack.Count - 1; i >= 0; --i) {
				state = this._stateStack [i];

				if (state.UpdateState ()) {
					this._stateStack.RemoveAt (i);
					state.ExitState ();
				}
			}
		}

		protected void FixedUpdate ()
		{
			IState state = null;

			for (int i = this._stateStack.Count - 1; i >= 0; --i) {
				state = this._stateStack [i];

				if (state.FixedUpdateState ()) {
					this._stateStack.RemoveAt (i);
					state.ExitState ();

				}
			}
		}

		protected void LateUpdate ()
		{
			IState state = null;

			for (int i = this._stateStack.Count - 1; i >= 0; --i) {
				state = this._stateStack [i];

				if (state.LateUpdateState ()) {
					this._stateStack.RemoveAt (i);
					state.ExitState ();
				}
			}
		}

		#endregion

		#region States

		public abstract void ConstructAvailableStates ();

		public abstract void InitializeStateStack ();

		#endregion
	}

	public class StateObject<T>:IState where T:MonoBehaviour
	{
		protected T _controlledObject;

		protected bool _hasUpdate = true;

		public bool hasUpdate {
			get{ return this._hasUpdate; }
		}

		#region constructor

		public StateObject ()
		{
		}

		public StateObject (T controlledObject)
		{
			this._controlledObject = controlledObject;
			InitializeState ();
		}

		#endregion

		#region IState implementation

		public virtual void InitializeState ()
		{
			if (this._controlledObject == null) {
				Debug.LogError (this.ToString () + " has no controlled object");
			}
		}

		public virtual void EnterState ()
		{
			//do staff on enter
		}

		public virtual bool UpdateState ()
		{
			//return true on exit condition
			return false;
		}

		public virtual bool FixedUpdateState ()
		{
			//do fixed stuff
			return false;
		}

		public virtual bool LateUpdateState ()
		{
			//do late state
			return false;
		}

		public virtual void ExitState ()
		{
			//do stuff on exit state
		}

		#endregion
	}

	public interface IState
	{
		void InitializeState ();

		void EnterState ();

		bool UpdateState ();

		bool FixedUpdateState ();

		bool LateUpdateState ();

		void ExitState ();

		bool hasUpdate{ get; }
	}

	public interface IStateable
	{
		void InitializeStateStack ();

		void ConstructAvailableStates ();

		void AddState (IState state);

		void RemoveState (IState state);

		void RemoveAllStates (bool endStates = true);

		void PurgeStateStack ();

		void StackState (IState state);

	}

}