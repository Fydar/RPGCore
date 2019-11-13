﻿using RPGCore.Behaviour.Connections;
using System;
using UnityEngine;

namespace RPGCore.Behaviour.Math
{
	[NodeInformation("Float/Random")]
	public class FloatRandomNode : BehaviourNode
	{
		public EventInput Reroll;
		public FloatInput Min;
		public FloatInput Max;

		public FloatOutput Output;

		protected override void OnSetup(IBehaviourContext context)
		{
			var rerollInput = Reroll[context];
			var minInput = Min[context];
			var maxInput = Max[context];
			var output = Output[context];

			float roll = 0.0f;

			Action outputHandlder = () =>
			{
				output.Value = Mathf.Lerp(minInput.Value, maxInput.Value, roll);
			};

			Action rerollHandler = () =>
			{
				roll = UnityEngine.Random.Range(0.0f, 1.0f);

				outputHandlder();
			};

			rerollInput.OnEventFired += rerollHandler;
			minInput.OnAfterChanged += outputHandlder;
			maxInput.OnAfterChanged += outputHandlder;

			rerollHandler();
		}

		protected override void OnRemove(IBehaviourContext character)
		{
		}

#if UNITY_EDITOR
		public override Vector2 GetDiamentions()
		{
			return new Vector2(140, base.GetDiamentions().y);
		}
#endif
	}
}

