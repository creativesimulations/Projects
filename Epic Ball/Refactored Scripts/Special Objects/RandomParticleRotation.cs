using UnityEngine;

namespace not
{
	//randomize object rotation when particle system is inactive (which requires "Looping" unchecked as well).
	public class RandomParticleRotation : MonoBehaviour
	{
		public bool x = false;
		public bool y = false;
		public bool z = false;

		void OnEnable()
		{
			if (x)
			{
				transform.localEulerAngles += new Vector3(Random.value * 360f, 0f, 0f);
			}
			if (y)
			{
				transform.localEulerAngles += new Vector3(0f, Random.value * 360f, 0f);
			}
			if (z)
			{
				transform.localEulerAngles += new Vector3(0f, 0f, Random.value * 360f);
			}
		}
	}
}