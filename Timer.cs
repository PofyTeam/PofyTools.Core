using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PofyTools
{
    public delegate void TimerDelegate (Timer timer);

    public class Timer : IInitializable
    {
        protected float _timerDuration;
        protected float _initialTimerDuration;
        protected float _nextTimestamp;

        protected string _id;
        public string Id
        {
            get { return this._id; }
        }

        #region Constructors
        public Timer (string id) : this (id, 0f) { }

        public Timer (string id, float countDownDuration)
        {
            this._id = id;
            this._timerDuration = countDownDuration;
            this._initialTimerDuration = countDownDuration;
        }

        #endregion

        #region Event
        protected TimerDelegate _onEvent = null;

        protected void IdleEventListener (Timer timer)
        {
        }

        /// <summary>
        /// Adds the event listener.
        /// </summary>
        /// <param name="listener">Listener.</param>
        public void AddEventListener (TimerDelegate listener)
        {
            this._onEvent += listener;
        }

        /// <summary>
        /// Removes the event listener.
        /// </summary>
        /// <param name="listener">Listener.</param>
        public void RemoveEventListener (TimerDelegate listener)
        {
            this._onEvent -= listener;
        }

        /// <summary>
        /// Removes all event listeners.
        /// </summary>
        public void RemoveAllEventListeners ()
        {
            this._onEvent = this.IdleEventListener;
        }
        #endregion

        #region Initialize
        /// <summary>
        /// Gets a value indicating whether this <see cref="PofyTools.Timer"/> is initialized.
        /// </summary>
        /// <value><c>true</c> if is initialized; otherwise, <c>false</c>.</value>
        public bool IsInitialized
        {
            get;
            protected set;
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>

        public virtual bool Initialize ()
        {
            if (!this.IsInitialized)
            {
                this._onEvent = this.IdleEventListener;
                this.IsInitialized = true;
                return true;
            }

            return false;
        }

        #endregion

        #region Count Up

        private float _counterTimestamp;

        public void StartCounter ()
        {
            this._counterTimestamp = Time.time;
        }

        /// <summary>
        /// Time in seconds since counter started via StartCounter().
        /// </summary>
        public float Counter
        {
            get
            {
                return Time.time - this._counterTimestamp;
            }
        }

        #endregion

        #region Count Down
        public void SetTimer (float duration)
        {
            this._timerDuration = duration;
            this._nextTimestamp = Time.time + duration;
        }

        /// <summary>
        /// Gets the next timestamp in seconds.
        /// </summary>
        /// <value>The next timestamp in seconds.</value>
        public float NextTimestamp
        {
            get
            {
                return this._nextTimestamp;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="PofyTools.Timer"/> has reached next timestamp.
        /// </summary>
        /// <value><c>true</c> if is ready; otherwise, <c>false</c>.</value>
        public bool IsReady
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
        public float TimeLeft
        {
            get
            {
                return Mathf.Max (0, this._nextTimestamp - Time.time);
            }
        }

        /// <summary>
        /// Tries the execute, firing event and reseting the cooldown.
        /// </summary>
        /// <returns><c>true</c>, if execution was successful, <c>false</c> otherwise.</returns>
        /// <param name="force">If set to <c>true</c> force execution.</param>
        public bool TryExecute (bool force = false)
        {
            if (this.IsReady || force)
            {
                FireEvent ();
                return true;
            }
            return false;
        }

        protected void FireEvent (bool autoResetOnEvent = false)
        {
            this._onEvent (this);
            if(autoResetOnEvent)
                ResetTimer ();
        }

        public virtual void ResetTimer ()
        {
            SetTimer (this._timerDuration);
        }

        #endregion

    }
}
