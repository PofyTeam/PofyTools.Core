namespace PofyTools
{
    using UnityEngine;
    using System.Collections;

    public class CollisionDetector2D : BaseDetector2D
    {

        protected virtual void OnCollisionEnter(Collision2D collision)
        {
            ((ICollisionListener2D)target).CollisionDetected(this, collision);
        }

        protected virtual void OnCollisionStay(Collision2D collision)
        {
            if (this.detectStay)
                ((ICollisionListener2D)target).CollisionStay(this, collision);
        }

        protected virtual void OnCollisionExit(Collision2D collision)
        {
            if (this.detectExit)
                ((ICollisionListener2D)target).CollisionEnded(this, collision);
        }
    }
}
