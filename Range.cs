namespace PofyTools
{
    using UnityEngine;
    using System.Collections;

    /// <summary>
    /// Sortable Range. Min is always smaller or equal to Max
    /// </summary>
    [System.Serializable]
    public struct Range
    {
        public float min;
        public float max;

        [SerializeField]
        private float _current;
        public float Current
        {
            get
            {
                return this._current;
            }
            set
            {
                if (value != this._current)
                {
                    this._current = Clamp(value);
                }
            }
        }


        public Range(Vector2 range)
            : this(Mathf.Min(range[0], range[1]), Mathf.Max(range[0], range[1]))
        {

        }

        public Range(float maxFromZero)
            : this(0, maxFromZero)
        {

        }

        public Range(float min, float max)
            : this(min, max, min)
        {

        }

        public Range(float min, float max, float current)
        {
            this._current = 0;
            this.min = min;
            this.max = max;

            Range.Sort(this);

            this.Current = current;
        }

        public Range Clone()
        {
            return new Range(this.min, this.max, this._current);
        }

        #region INSTANCE METHODS

        public bool IsEmpty
        {
            get
            {
                return min == 0 && max == 0;
            }
        }

        public bool IsZeroLength
        {
            get { return min == max; }
        }

        public bool AtMin
        {
            get { return this._current == this.min; }
        }

        public bool AtMax
        {
            get { return this._current == this.max; }
        }

        public float Avarege
        {
            get
            {
                return (this.min + this.max) / 2;
            }
        }

        public float MinToMaxRatio
        {
            get
            {
                if (max != 0)
                    return min / max;
                return this.min / float.Epsilon;
            }
        }

        public float MaxToMinRatio
        {
            get
            {
                if (min != 0)
                    return max / min;
                return max / float.Epsilon;
            }
        }

        //Returns current value to max value ratio
        public float CurrentToMaxRatio
        {
            get
            {
                if (max != 0)
                    return this.Current / this.max;
                return this.Current / float.Epsilon;
            }
        }

        // Returns current distance from minimum
        public float CurrentOffset
        {
            get
            {
                return this._current - this.min;
            }
        }

        //public float GetRandom()
        //{
        //    return UnityEngine.Random.Range(this.min, this.max);
        //}

        public float Random
        {
            get { return UnityEngine.Random.Range(this.min, this.max); }
        }

        public int IntRandom
        {
            get { return UnityEngine.Random.Range((int)this.min, (int)this.max + 1); }
        }

        public float Length
        {
            get
            {
                return this.max - this.min;
            }
        }

        public float Percentage(float value)
        {
            if (!this.IsZeroLength)
                return Mathf.Clamp01((value - this.min) / (this.max - this.min));
            return 0;
        }

        public float CurrentPercentage
        {

            get
            {
                if (!this.IsZeroLength)
                    return (this._current - this.min) / (this.max - this.min);
                return 0;
            }
        }

        public float MappedPoint(float normalizedInput)
        {
            return (max - min) * normalizedInput + min;
        }

        public bool Contains(float point)
        {
            return (point >= min && point <= max);
        }

        public void Negate()
        {
            float _cache = 0;
            _cache = -this.min;
            this.min = -this.max;
            this.max = _cache;
        }

        /// <summary>
        /// Increments both min and max for given value
        /// </summary>
        /// <param name="offset"></param>
        public void Offset(float offset)
        {
            this.min += offset;
            this.max += offset;
        }

        /// <summary>
        /// Decreases min and increaces max so that length is increased for provided value.
        /// </summary>
        /// <param name="value"></param>
        public void Spread(float value)
        {
            value *= 0.5f;
            this.min -= value;
            this.max += value;
        }
        /// <summary>
        /// Multiplies min and max for provided value
        /// </summary>
        /// <param name="scale"></param>
        public void Scale(float scale)
        {
            this.min *= scale;
            this.max *= scale;
        }

        /// <summary>
        /// Clamps provided value between min and max.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public float Clamp(float value)
        {
            if (value < this.min)
                return min;
            if (value > this.max)
                return max;
            return value;
        }

        #endregion

        #region STATIC METHODS

        public static void Sort(Range range)
        {
            float _cache = range.min;
            if (range.min > range.max)
            {
                range.min = range.max;
                range.max = _cache;
            }
        }

        /// <summary>
        /// Constructs a 0 to 1 range.
        /// </summary>
        public static Range ZeroToOne
        {
            get { return new Range(0f, 1f); }
        }
        /// <summary>
        /// Constructs a 0 to 100 range.
        /// </summary>
        public static Range ZeroToHundred
        {
            get { return new Range(0f, 100f); }
        }

        #endregion

        #region OVERRIDES

        public override string ToString()
        {
            return string.Format("[Min - {0:##.###} , Max - {1:##.###},Current - {2:##.###}]", this.min, this.max, this._current);
        }

        #endregion

        public float this[int index]
        {
            get
            {
                if (index == 0)
                    return this.min;
                else if (index == 1)
                    return this.max;
                else if (index == 2)
                    return this.Current;
                else
                    throw new System.ArgumentOutOfRangeException("index");
            }
            set
            {
                if (index == 0)
                    this.min = value;
                else if (index == 1)
                    this.max = value;
                else if (index == 2)
                    this.Current = value;
                else
                    throw new System.ArgumentOutOfRangeException("index");
            }
        }

        public static implicit operator RangeInt(Range range)
        {
            return new RangeInt((int)range.min, (int)range.max, (int)range.Current);
        }


        public static implicit operator Range(RangeInt rangeInt)
        {
            return new Range(rangeInt.min, rangeInt.max, rangeInt.Current);
        }

        public static implicit operator float(Range range)
        {
            return range.Current;
        }

    }

    /// <summary>
    /// Sortable Range Int. Min is always smaller or equal to Max
    /// </summary>
    [System.Serializable]
    public struct RangeInt
    {
        public int min;
        public int max;

        [SerializeField]
        private int _current;
        public int Current
        {
            get
            {
                return this._current;
            }
            set
            {
                if (value != this._current)
                {
                    this._current = Clamp(value);
                }
            }
        }


        public RangeInt(Vector2Int range)
            : this(Mathf.Min(range[0], range[1]), Mathf.Max(range[0], range[1]))
        {

        }

        public RangeInt(int maxFromZero)
            : this(0, maxFromZero)
        {

        }

        public RangeInt(int min, int max)
            : this(min, max, min)
        {

        }

        public RangeInt(int min, int max, int current)
        {
            this._current = 0;
            this.min = min;
            this.max = max;

            RangeInt.Sort(this);

            this.Current = current;
        }

        public RangeInt Clone()
        {
            return new RangeInt(this.min, this.max, this._current);
        }

        #region INSTANCE METHODS

        public bool IsEmpty
        {
            get
            {
                return min == 0 && max == 0;
            }
        }

        public bool IsZeroLength
        {
            get { return min == max; }
        }

        public bool AtMin
        {
            get { return this._current == this.min; }
        }

        public bool AtMax
        {
            get { return this._current == this.max; }
        }

        public float Avarege
        {
            get
            {
                return (this.min + this.max) / 2;
            }
        }

        public float MinToMaxRatio
        {
            get
            {
                if (max != 0)
                    return min / max;
                return this.min / float.Epsilon;
            }
        }

        public float MaxToMinRatio
        {
            get
            {
                if (min != 0)
                    return max / min;
                return max / float.Epsilon;
            }
        }

        //Returns current value to max value ratio
        public float CurrentToMaxRatio
        {
            get
            {
                if (max != 0)
                    return this.Current / this.max;
                return this.Current / float.Epsilon;
            }
        }

        // Returns current distance from minimum
        public int CurrentOffset
        {
            get
            {
                return this._current - this.min;
            }
        }

        //public float GetRandom()
        //{
        //    return UnityEngine.Random.Range(this.min, this.max);
        //}

        public int Random
        {
            get { return UnityEngine.Random.Range(this.min, this.max); }
        }

        //public int IntRandom
        //{
        //    get { return UnityEngine.Random.Range((int)this.min, (int)this.max + 1); }
        //}

        public int Length
        {
            get
            {
                return this.max - this.min;
            }
        }

        public float Percentage(float value)
        {
            if (!this.IsZeroLength)
                return Mathf.Clamp01((value - this.min) / (this.max - this.min));
            return 0;
        }

        public float CurrentPercentage
        {

            get
            {
                if (!this.IsZeroLength)
                    return (this._current - this.min) / (this.max - this.min);
                return 0;
            }
        }

        public float MappedPoint(float normalizedInput)
        {
            return (max - min) * normalizedInput + min;
        }

        public bool Contains(float point)
        {
            return (point >= min && point <= max);
        }

        public void Negate()
        {
            int _cache = 0;
            _cache = -this.min;
            this.min = -this.max;
            this.max = _cache;
        }

        /// <summary>
        /// Increments both min and max for given value
        /// </summary>
        /// <param name="offset"></param>
        public void Offset(int offset)
        {
            this.min += offset;
            this.max += offset;
        }

        /// <summary>
        /// Decreases min and increaces max so that length is increased for provided value.
        /// </summary>
        /// <param name="value"></param>
        public void Spread(int value)
        {
            float half = value * 0.5f;
            this.min -= (int)half;
            this.max += (int)half;
        }
        /// <summary>
        /// Multiplies min and max for provided value
        /// </summary>
        /// <param name="scale"></param>
        public void Scale(int scale)
        {
            this.min *= scale;
            this.max *= scale;
        }

        /// <summary>
        /// Clamps provided value between min and max.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public float Clamp(float value)
        {
            if (value < this.min)
                return min;
            if (value > this.max)
                return max;
            return value;
        }

        public int Clamp(int value)
        {

            if (value < this.min)
                return min;
            if (value > this.max)
                return max;
            return value;
        }

        #endregion

        #region STATIC METHODS

        public static void Sort(RangeInt range)
        {
            int _cache = range.min;
            if (range.min > range.max)
            {
                range.min = range.max;
                range.max = _cache;
            }
        }

        /// <summary>
        /// Constructs a 0 to 1 range.
        /// </summary>
        public static RangeInt ZeroToOne
        {
            get { return new RangeInt(0, 1); }
        }

        /// <summary>
        /// Constructs a 0 to 100 range.
        /// </summary>
        public static RangeInt ZeroToHundred
        {
            get { return new RangeInt(0, 100); }
        }

        #endregion

        #region OVERRIDES

        public override string ToString()
        {
            return string.Format("[Min - {0} , Max - {1},Current - {2}]", this.min, this.max, this._current);
        }

        #endregion

        public int this[int index]
        {
            get
            {
                if (index == 0)
                    return this.min;
                else if (index == 1)
                    return this.max;
                else if (index == 2)
                    return this.Current;
                else
                    throw new System.ArgumentOutOfRangeException("index");
            }
            set
            {
                if (index == 0)
                    this.min = value;
                else if (index == 1)
                    this.max = value;
                else if (index == 2)
                    this.Current = value;
                else
                    throw new System.ArgumentOutOfRangeException("index");
            }
        }

        public static implicit operator int(RangeInt range)
        {
            return range.Current;
        }

        public static RangeInt operator ++(RangeInt range)
        {
            range.Current++;
            return range;
        }
    }

}
