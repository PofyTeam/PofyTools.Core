namespace PofyTools
{
	using UnityEngine;
	using System.Collections;

	public static class Math
	{
		public static int ArithmeticSequenceSum (int n)
		{
			//int sign = (int)Mathf.Sign (n);	
			n = Mathf.Abs (n);

			return (int)(n * (n + 1) * .5f/* * sign*/);
		}

		//Returns random position offset based on Perlin Noise. Default component's (x,y) value range (-1, 1).

		public static Vector3 PerlinShake (float magnitude = 1f, float speed = 1f)
		{
			Vector3 shakeOffset = new Vector3 ();

			shakeOffset [0] = (Mathf.PerlinNoise (Time.time * speed, 1) - 0.5f) * 2f * magnitude;
			shakeOffset [1] = (Mathf.PerlinNoise (1, Time.time * speed) - 0.5f) * 2f * magnitude;
			shakeOffset [2] = 0;

			return shakeOffset;
		}

		public static Vector3 UnscaledPerlinShake (float magnitude = 1f, float speed = 1)
		{
			Vector3 shakeOffset = new Vector3 ();

			shakeOffset [0] = (Mathf.PerlinNoise (Time.unscaledTime * speed, 1) - 0.5f) * 2f * magnitude;
			shakeOffset [1] = (Mathf.PerlinNoise (1, Time.unscaledTime * speed) - 0.5f) * 2f * magnitude;
			shakeOffset [2] = 0;

			return shakeOffset;
		}

		public static int RandomSign{ get { return (Random.Range (0, 101) > 50) ? 1 : -1; } }

		public static float EvaluateConcave (float a = 1, float b = 0.5f, float t = 0)
		{
			return b * (Mathf.Pow ((t - a) / a, 2));
		}

		public static float EvaluateConvex (float a = 1, float b = 0.5f, float t = 0)
		{
			return b - (b * (Mathf.Pow ((t - a), 2) / a));
		}

		public static float EvaluateSqrt (float t)
		{
			return Mathf.Sqrt (t);
		}

		public static float EvaluateInverseSqrt (float t)
		{
			return 1 - Mathf.Sqrt (t);
		}

		public static float EvaluateLog (float a = 1, float n = 2, float t = 0)
		{
			return a * (1 - 1 / Mathf.Pow (t, n));
		}

	}
}