namespace PofyTools
{
    using UnityEngine;
    using System.Collections;

    public class TriggerDetector2D : BaseDetector2D
    {

        void OnTriggerEnter2D(Collider2D other)
        {
            ((ITriggerListener2D)target).TriggerDetected(this, other);
        }

        void OnTriggerStay2D(Collider2D other)
        {
            if (this.detectStay)
                ((ITriggerListener2D)target).TriggerStay(this, other);
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (this.detectExit)
                ((ITriggerListener2D)target).TriggerEnded(this, other);
        }
    }
}
