using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation ("Float/Random")]
	public class FloatRandomNode : BehaviourNode
	{
		public EventInput Reroll;
		public FloatInput Min;
		public FloatInput Max;

		public FloatOutput Output;

		protected override void OnSetup (IBehaviourContext context)
		{
			EventEntry rerollInput = Reroll[context];
			ConnectionEntry<float> minInput = Min[context];
			ConnectionEntry<float> maxInput = Max[context];
			ConnectionEntry<float> output = Output[context];

			float roll = 0.0f;

			Action outputHandlder = () =>
			{
				output.Value = Mathf.Lerp (minInput.Value, maxInput.Value, roll);
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

		protected override void OnRemove (IBehaviourContext character)
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