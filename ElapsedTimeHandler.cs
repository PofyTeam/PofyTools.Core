using UnityEngine;
using System.Collections;

namespace PofyTools
{
	[System.Serializable]
	public class ElapsedTimeHandler
	{
		public Range initialCooldownRange;
		public Range cooldownRange;
		float _nextTimestamp;

		public ElapsedTimeHandler () : this (0)
		{
		}

		public ElapsedTimeHandler (float fixedDuration) : this (fixedDuration, fixedDuration)
		{
		}

		public ElapsedTimeHandler (float min, float max) : this (min, max, min, max)
		{
		}

		public ElapsedTimeHandler (float initMin, float initMax, float min, float max)
		{
			this.initialCooldownRange.min = initMin;
			this.initialCooldownRange.max = initMax;

			this.cooldownRange.min = min;
			this.cooldownRange.max = max;
		}

		public ElapsedTimeHandler (Range cooldown) : this (cooldown, cooldown)
		{
		}

		public ElapsedTimeHandler (float initCooldown, Range cooldown) : this (new Range (initCooldown, initCooldown), cooldown)
		{
		}

		public ElapsedTimeHandler (Range initCooldown, Range cooldown)
		{
			this.initialCooldownRange = initCooldown;
			this.cooldownRange = cooldown;
		}

		public float nextTimestamp {
			get {
				return this._nextTimestamp;
			}
		}

		public bool isReady {
			get {
				return (Time.time > this._nextTimestamp);
			}
		}

		public float timeLeft {
			get {
				return Mathf.Max (0, this._nextTimestamp - Time.time);
			}
		}

		public void Initialize ()
		{
			float timeOffset = initialCooldownRange.Random;
			SetTimer (timeOffset);
		}

		public void Initialize (Range initialCooldownRange, Range cooldownRange)
		{
			this.initialCooldownRange = initialCooldownRange;
			this.cooldownRange = cooldownRange;

			Initialize ();
		}

		public void Initialize (float initialCooldown, Range cooldownRange)
		{
			Range initialCooldownRange = new Range (initialCooldown, initialCooldown);
			Initialize (initialCooldownRange, cooldownRange);
		}

		public void ResetCooldown ()
		{
			float timeOffset = this.cooldownRange.Random;
			SetTimer (timeOffset);
		}

		void OnEvent ()
		{
			float timeOffset = this.cooldownRange.Random;
			SetTimer (timeOffset);
		}

		void SetTimer (float timeOffset)
		{
			this._nextTimestamp = Time.time + timeOffset;
		}

		public bool TrySet ()
		{
			if (isReady) {
				OnEvent ();
				return true;
			}
			return false;
		}
	}

}