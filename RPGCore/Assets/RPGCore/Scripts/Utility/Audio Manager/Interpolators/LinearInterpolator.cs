using System;
using UnityEngine;

namespace RPGCore.Audio
{
	[Serializable]
	public struct LinearInterpolator : IInterpolator
	{
		public float Speed;

		private float targetValue;
		private float currentValue;

		public LinearInterpolator (float speed)
		{
			Speed = speed;

			targetValue = 0.0f;
			currentValue = 0.0f;
		}

		public float Value
		{
			get
			{
				return currentValue;
			}
			set
			{
				currentValue = value;
			}
		}

		public float TargetValue
		{
			set
			{
				targetValue = value;
			}
		}

		public bool Sleeping
		{
			get
			{
				return currentValue == targetValue;
			}
		}

		public void Update (float deltaTime)
		{
			if (Sleeping)
				return;

			float movementAmount = Speed * deltaTime;

			if (currentValue < targetValue)
				currentValue = Mathf.Min (currentValue + movementAmount, targetValue);
			else
				currentValue = Mathf.Max (currentValue - movementAmount, targetValue);
		}
	}
}