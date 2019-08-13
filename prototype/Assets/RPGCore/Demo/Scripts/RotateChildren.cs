using UnityEngine;

namespace RPGCore.Utility
{
	public class RotateChildren : MonoBehaviour
	{
		[SerializeField]
		private float RotationSpeed = 5.0f;

		private Vector3 currentRotation;

		public Quaternion lastRotation;

		private void LateUpdate ()
		{
			currentRotation = new Vector3 (currentRotation.x,
				currentRotation.y + (RotationSpeed * Time.deltaTime),
				currentRotation.z);

			Quaternion rotator = Quaternion.Euler (currentRotation);

			lastRotation = rotator;

			foreach (Transform child in transform)
			{
				child.rotation = rotator;
			}
		}
	}
}

