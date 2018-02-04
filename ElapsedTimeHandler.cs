using UnityEngine;
using System.Collections;

namespace PofyTools
{
    [System.Serializable]
    public class ElapsedTimeHandler : Timer
    {
        protected Range _cooldownRange;
        public Range initialCooldownRange;
        //public Range cooldownRange;
        // protected UpdateDelegate _onEvent = null;

        #region Constructors

        public ElapsedTimeHandler (string id)
            : base (id, 0f)
        {
        }

        public ElapsedTimeHandler (string id, float fixedDuration)
            : base (id, fixedDuration)
        {
            this.initialCooldownRange = new Range (fixedDuration);
            this._cooldownRange = new Range (fixedDuration);
        }

        public ElapsedTimeHandler (string id, float min, float max)
            : this (id, min, max, min, max)
        {
        }

        public ElapsedTimeHandler (string id, float initMin, float initMax, float min, float max) : base (id, initMax)
        {
            this.initialCooldownRange.min = initMin;
            this.initialCooldownRange.max = initMax;

            this._cooldownRange.min = min;
            this._cooldownRange.max = max;
        }

        public ElapsedTimeHandler (string id, Range cooldown)
            : this (id, cooldown, cooldown)
        {
        }

        public ElapsedTimeHandler (string id, float initCooldown, Range cooldown)
            : this (id, new Range (initCooldown, initCooldown), cooldown)
        {
        }

        public ElapsedTimeHandler (string id, Range initCooldown, Range cooldown) : base (id)
        {
            this.initialCooldownRange = initCooldown;
            this._cooldownRange = cooldown;
            Initialize ();
        }

        public ElapsedTimeHandler (string id, ElapsedTimeHandler source) : base (id)
        {
            this.initialCooldownRange = source.initialCooldownRange;
            this._cooldownRange = source._cooldownRange;
            Initialize ();
        }

        #endregion

        #region API

        /// <summary>
        /// Resets the Timer.
        /// </summary>
        public override void ResetTimer ()
        {
            SetTimer (this._cooldownRange.Random);
        }

        /// <summary>
        /// Sets Timer to Initial value
        /// </summary>
        public void InitializeTimer ()
        {
            SetTimer (this.initialCooldownRange.Random);
        }

        #endregion

        #region Initialize
        public override bool Initialize ()
        {
            if (base.Initialize ())
            {
                InitializeTimer ();
                return true;
            }
            return false;
        }
        #endregion

    }
}