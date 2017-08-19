using UnityEngine;
using System.Collections;

namespace PofyTools
{
    [System.Serializable]
    public class ElapsedTimeHandler:IInitializable
    {
        public Range initialCooldownRange;
        public Range cooldownRange;

        protected float _nextTimestamp;
        protected UpdateDelegate _onEvent = null;

        #region Constructors

        public ElapsedTimeHandler()
            : this(0)
        {
        }

        public ElapsedTimeHandler(float fixedDuration)
            : this(fixedDuration, fixedDuration)
        {
        }

        public ElapsedTimeHandler(float min, float max)
            : this(min, max, min, max)
        {
        }

        public ElapsedTimeHandler(float initMin, float initMax, float min, float max)
        {
            this.initialCooldownRange.min = initMin;
            this.initialCooldownRange.max = initMax;

            this.cooldownRange.min = min;
            this.cooldownRange.max = max;
        }

        public ElapsedTimeHandler(Range cooldown)
            : this(cooldown, cooldown)
        {
        }

        public ElapsedTimeHandler(float initCooldown, Range cooldown)
            : this(new Range(initCooldown, initCooldown), cooldown)
        {
        }

        public ElapsedTimeHandler(Range initCooldown, Range cooldown)
        {
            this.initialCooldownRange = initCooldown;
            this.cooldownRange = cooldown;
            Initialize();
        }

        public ElapsedTimeHandler(ElapsedTimeHandler source)
        {
            this.initialCooldownRange = source.initialCooldownRange;
            this.cooldownRange = source.cooldownRange;
            Initialize();
        }

        #endregion

        #region Private Methods

        protected void FireEvent()
        {                
            this._onEvent();   
            ResetCooldown();
        }

        protected void SetTimer(float timeOffset)
        {
            this._nextTimestamp = Time.time + timeOffset;
        }

        protected void IdleEventListener()
        {
            
        }

        #endregion

        #region API

        /// <summary>
        /// Gets the next timestamp in seconds.
        /// </summary>
        /// <value>The next timestamp in seconds.</value>
        public float nextTimestamp
        {
            get
            {
                return this._nextTimestamp;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="PofyTools.ElapsedTimeHandler"/> has reached next timestamp.
        /// </summary>
        /// <value><c>true</c> if is ready; otherwise, <c>false</c>.</value>
        public bool isReady
        {
            get
            {
                return (Time.time > this._nextTimestamp);
            }
        }

        /// <summary>
        /// Gets the time left for cooldown in seconds. Time left is difference between next timestamp and Time.time maxed at 0.
        /// </summary>
        /// <value>The time left.</value>
        public float timeLeft
        {
            get
            {
                return Mathf.Max(0, this._nextTimestamp - Time.time);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="PofyTools.ElapsedTimeHandler"/> is initialized.
        /// </summary>
        /// <value><c>true</c> if is initialized; otherwise, <c>false</c>.</value>
        public bool isInitialized
        {
            get;
            protected set;
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public bool Initialize()
        {
            if (!this.isInitialized)
            {
                ResetCooldown(toInitial: true);
                this._onEvent = this.IdleEventListener;
                this.isInitialized = true;
                return true;
            }

            return false;
        }

        //        public void Initialize(Range initialCooldownRange, Range cooldownRange)
        //        {
        //            this.initialCooldownRange = initialCooldownRange;
        //            this.cooldownRange = cooldownRange;
        //
        //            Initialize();
        //        }
        //
        //        public void Initialize(float initialCooldown, Range cooldownRange)
        //        {
        //            Range initialCooldownRange = new Range(initialCooldown, initialCooldown);
        //            Initialize(initialCooldownRange, cooldownRange);
        //        }
        //
        //        public void Initialize(float min, float max)
        //        {
        //            Range range = new Range(min, max);
        //            Initialize(range, range);
        //        }
        //

        /// <summary>
        /// Tries the execute, firing event and reseting the cooldown.
        /// </summary>
        /// <returns><c>true</c>, if execution was successful, <c>false</c> otherwise.</returns>
        /// <param name="force">If set to <c>true</c> force execution.</param>
        public bool TryExecute(bool force = false)
        {
            if (this.isReady || force)
            {
                FireEvent();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Resets the cooldown.
        /// </summary>
        /// <param name="toInitial">If set to <c>true</c> resets to initial cooldown.</param>
        public void ResetCooldown(bool toInitial = false)
        {
            if (toInitial)
            {
                SetTimer(this.initialCooldownRange.Random);
                return;
            }

            SetTimer(this.cooldownRange.Random);
        }

        /// <summary>
        /// Adds the event listener.
        /// </summary>
        /// <param name="listener">Listener.</param>
        public void AddEventListener(UpdateDelegate listener)
        {
            this._onEvent += listener;
        }

        /// <summary>
        /// Removes the event listener.
        /// </summary>
        /// <param name="listener">Listener.</param>
        public void RemoveEventListener(UpdateDelegate listener)
        {
            this._onEvent -= listener;
        }

        /// <summary>
        /// Removes all event listeners.
        /// </summary>
        public void RemoveAllEventListeners()
        {
            this._onEvent = this.IdleEventListener;
        }

        #endregion
    }
}