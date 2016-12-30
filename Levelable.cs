namespace PofyTools
{
	using UnityEngine;
	using System.Collections;

	public interface ILevelable
	{
		int level {
			get;
			set;
		}

		int maxLevel {
			get;
		}

		void LevelUp ();
	}

	public class Levelable : ILevelable
	{
		private int _level;
		private float[] _values;
		private Range _valueRange;

		#region ILevelable implementation

		public Levelable () : this (2, new Range (0, 1))
		{
		
		}

		public Levelable (Range valueRange) : this (2, valueRange)
		{
		}

		public Levelable (int levelCount, Range valueRange)
		{
			this._valueRange = valueRange;
			this._values = new float[levelCount];
			float valueIncrement = valueRange.MappedPoint (1 / levelCount);
			for (int i = 0; i < levelCount; ++i) {
				this._values [i] = i * valueIncrement + this._valueRange.min;
			}
		}

		public void LevelUp ()
		{
			throw new System.NotImplementedException ();
		}

		public int level {
			get {
				return this._level;
			}
			set {
				this._level = value;
			}
		}

		public int maxLevel {
			get {
				throw new System.NotImplementedException ();
			}
		}

		public float levelValue {
			get {
				return this._values [this._level - 1];
			}
		}

		public float this [int index] {
			get {
				return this._values [index];
			}
//			set {
//				this._values [index] = value;
//			}
		}

		public static long GetLevelCost (long level)
		{
			return (long)(Mathf.Log10 (level) * level * 21f);


		}


		#endregion


	}

}