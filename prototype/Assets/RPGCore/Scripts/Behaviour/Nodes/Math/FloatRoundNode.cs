using RPGCore.Behaviour.Connections;
using System;
using UnityEngine;

namespace RPGCore.Behaviour.Math
{
	[NodeInformation ("Float/Round")]
	public class FloatRoundNode : BehaviourNode
	{
		public enum Rounding
		{
			Round,
			Floor,
			Ceiling
		}

		public Rounding Method;
		public float Multiple = 1.0f;
		public FloatInput Value;

		public FloatOutput Output;

		protected override void OnSetup (IBehaviourContext context)
		{
			ConnectionEntry<float> valueInput = Value[context];
			ConnectionEntry<float> output = Output[context];

			Action updateHandler = () =>
			{
				if (Method == Rounding.Round)
				{
					output.Value = Mathf.Round (valueInput.Value / Multiple) * Multiple;
				}
				else if (Method == Rounding.Floor)
				{
					output.Value = Mathf.Floor (valueInput.Value / Multiple) * Multiple;
				}
				else if (Method == Rounding.Ceiling)
				{
					output.Value = Mathf.Ceil (valueInput.Value / Multiple) * Multiple;
				}
			};

			valueInput.OnAfterChanged += updateHandler;

			updateHandler ();
		}

		protected override void OnRemove (IBehaviourContext context)
		{
		}

#if UNITY_EDITOR
		public override Vector2 GetDiamentions ()
		{
			return new Vector2 (140, 70);
		}
#endif
	}
}

