using System;
using UnityEngine;

namespace RPGCore.Utility
{
	[Serializable]
	public class DuelFadeBool
	{
		public bool Target;
		public float NegativeSpeed = 1.0f;
		public float PositiveSpeed = 1.0f;

		//[NonSerialized]
		public float Value = 1.0f;

		public float Update ()
		{
			if (Target)
			{
				float movementAmount = PositiveSpeed * Time.deltaTime;
				Value = Mathf.Min (Value + movementAmount, 1.0f);
			}
			else
			{
				float movementAmount = NegativeSpeed * Time.deltaTime;
				Value = Mathf.Max (Value - movementAmount, 0.0f);
			}

			return Value;
		}
	}
}