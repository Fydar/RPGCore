using RPGCore.Behaviour.Connections;
using System;
using UnityEngine;

namespace RPGCore.Behaviour.Math
{
	[NodeInformation ("Int/Random")]
	public class IntRandomNode : BehaviourNode
	{
		public EventInput Reroll;
		public IntInput Min;
		public IntInput Max;

		public IntOutput Output;

		protected override void OnSetup (IBehaviourContext context)
		{
			EventEntry rerollInput = Reroll[context];
			ConnectionEntry<int> minInput = Min[context];
			ConnectionEntry<int> maxInput = Max[context];
			ConnectionEntry<int> output = Output[context];

			float roll = 0.0f;

			Action outputHandlder = () =>
			{
				output.Value = Mathf.RoundToInt (Mathf.Lerp (minInput.Value, maxInput.Value, roll));
			};

			Action rerollHandler = () =>
			{
				roll = UnityEngine.Random.Range (0.0f, 1.0f);

				outputHandlder ();
			};

			rerollInput.OnEventFired += rerollHandler;
			minInput.OnAfterChanged += outputHandlder;
			maxInput.OnAfterChanged += outputHandlder;

			rerollHandler ();
		}

		protected override void OnRemove (IBehaviourContext context)
		{
		}

#if UNITY_EDITOR
		public override Vector2 GetDiamentions ()
		{
			return new Vector2 (140, base.GetDiamentions ().y);
		}
#endif
	}
}

