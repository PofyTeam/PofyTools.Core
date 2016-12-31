using UnityEngine;
using System.Collections;

namespace PofyTools
{
	//[ExecuteInEditMode]
	public class Rotator : MonoBehaviour
	{
		public Vector3 axis;
		public float speed;
		public bool randomize = false;

		void Awake ()
		{
			if (randomize)
				this.axis = Random.onUnitSphere;
		}
		// Update is called once per frame
		void Update ()
		{
			this.transform.Rotate (this.axis * this.speed * Time.smoothDeltaTime);
		}
	}
}
