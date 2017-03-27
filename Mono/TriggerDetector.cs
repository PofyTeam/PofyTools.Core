namespace PofyTools
{
    using UnityEngine;
    using System.Collections;

    public class TriggerDetector : BaseDetector
    {

        void OnTriggerEnter(Collider other)
        {
            ((ITriggerListener)target).TriggerDetected(this, other);
        }

        void OnTriggerStay(Collider other)
        {
            if (this.detectStay)
                ((ITriggerListener)target).TriggerStay(this, other);
        }

        void OnTriggerExit(Collider other)
        {
            if (this.detectExit)
                ((ITriggerListener)target).TriggerEnded(this, other);
        }
    }
}
