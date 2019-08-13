using System;
using UnityEngine;

namespace RPGCore.Utility
{
	[Serializable]
	public class FadeBool
	{
		public bool Target;
		public float Speed = 1.0f;

		//[NonSerialized]
		public float Value = 1.0f;

		public float Update ()
		{
			float movementAmount = Speed * Time.deltaTime;

			if (Target)
				Value = Mathf.Min (Value + movementAmount, 1.0f);
			else
				Value = Mathf.Max (Value - movementAmount, 0.0f);

			return Value;
		}
	}
}

