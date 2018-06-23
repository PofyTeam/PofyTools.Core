namespace PofyTools
{
	using UnityEngine;
	using System.Collections;

	public abstract class BaseDetector : MonoBehaviour, ICollidable, ITransformable
	{
		#region ITransformable implementation

		protected Transform _selfTransform;

		public Transform SelfTransform {
			get {
				return this._selfTransform;
			}
		}

		#endregion

		#region ICollidable implementation

		protected Collider _selfCollider;

		public Collider SelfCollider {
			get {
				return this._selfCollider;
			}
		}

		protected Rigidbody _selfRigidbody;

		public Rigidbody SelfRigidbody {
			get {
				return this._selfRigidbody;
			}
		}

		#endregion

		#region Mono

		protected virtual void Awake ()
		{
			this._selfTransform = this.transform;
			this._selfCollider = GetComponent<Collider> ();
			this._selfRigidbody = GetComponent<Rigidbody> ();
		}

		#endregion

		public MonoBehaviour target;
		public bool detectStay = false;
		public bool detectExit = false;
	}
}
