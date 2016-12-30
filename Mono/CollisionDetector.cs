namespace PofyTools
{
	using UnityEngine;
	using System.Collections;

	public class CollisionDetector : BaseDetector
	{

		protected virtual void OnCollisionEnter (Collision collision)
		{
			((ICollisionListener)target).CollisionDetected (this, collision);
		}

		protected virtual void OnCollisionStay (Collision collision)
		{
			if (this.detectStay)
				((ICollisionListener)target).CollisionStay (this, collision);
		}

		protected virtual void OnCollisionExit (Collision collision)
		{
			if (this.detectExit)
				((ICollisionListener)target).CollisionEnded (this, collision);
		}
	}
}
