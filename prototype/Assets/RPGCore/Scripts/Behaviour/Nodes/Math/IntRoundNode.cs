using RPGCore.Behaviour.Connections;
using System;
using UnityEngine;

namespace RPGCore.Behaviour.Math
{
	[NodeInformation ("Int/Round")]
	public class IntRoundNode : BehaviourNode
	{
		public enum Rounding
		{
			Round,
			Floor,
			Ceiling
		}

		public Rounding Method;
		public int Multiple = 1;
		public FloatInput Value;

		public IntOutput Output;

		protected override void OnSetup (IBehaviourContext context)
		{
			ConnectionEntry<float> valueInput = Value[context];
			ConnectionEntry<int> output = Output[context];

			Action updateHandler = () =>
			{
				if (Method == Rounding.Round)
				{
					output.Value = Mathf.RoundToInt (valueInput.Value / Multiple) * Multiple;
				}
				else if (Method == Rounding.Floor)
				{
					output.Value = Mathf.FloorToInt (valueInput.Value / Multiple) * Multiple;
				}
				else if (Method == Rounding.Ceiling)
				{
					output.Value = Mathf.CeilToInt (valueInput.Value / Multiple) * Multiple;
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

