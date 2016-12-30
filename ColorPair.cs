namespace PofyTools
{
	using UnityEngine;
	using System.Collections;

	[System.Serializable]
	public class ColorPair
	{
		public Color color1;
		public Color color2;

		public ColorPair () : this (Color.black, Color.white)
		{
		}

		public ColorPair (ColorPair source) : this (source.color1, source.color2)
		{
		}

		public ColorPair (Color color1, Color color2)
		{
			this.color1 = color1;
			this.color2 = color2;
		}

		public void SwitchColors ()
		{
			Color _temp = this.color1;
			this.color1 = this.color2;
			this.color2 = _temp;
		}

		public void Set (ColorPair source)
		{
			Set (source.color1, source.color2);
		}

		public void Set (Color color1, Color color2)
		{
			this.color1 = color1;
			this.color2 = color2;
		}

		public void Lerp (ColorPair first, ColorPair second, float time)
		{
			this.color1 = Color.Lerp (first.color1, second.color1, time);
			this.color2 = Color.Lerp (first.color2, second.color2, time);
		}
	}
}
