using System;
using UnityEngine;

namespace RPGCore.Audio
{
	[Serializable]
	public struct DampenInterpolator : IInterpolator
	{
		private const float SPEED_THRESHHOLD = 0.005f;

		public float Speed;

		private float targetValue;
		private float currentValue;

		public DampenInterpolator (float speed)
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
				return Mathf.Abs (targetValue - currentValue) < SPEED_THRESHHOLD;
			}
		}

		public void Update (float deltaTime)
		{
			if (Sleeping)
				return;

			currentValue = Mathf.Lerp (currentValue, targetValue, deltaTime * Speed);
		}
	}
}

