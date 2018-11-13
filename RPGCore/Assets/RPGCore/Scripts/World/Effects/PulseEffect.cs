using UnityEngine;

namespace RPGCore
{
	public class PulseEffect : MonoBehaviour
	{
		public float DestroyAfter = 2.0f;

		private void Start ()
		{
			Destroy (gameObject, DestroyAfter);
		}
	}
}