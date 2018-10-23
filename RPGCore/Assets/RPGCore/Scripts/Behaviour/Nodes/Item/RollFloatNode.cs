using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation ("Item/Roll Float")]
	public class RollFloatNode : BehaviourNode
	{
		public FloatInput Min;
		public FloatInput Max;

		public FloatOutput Output;

		protected override void OnSetup (IBehaviourContext character)
		{
			ConnectionEntry<float> minInput = Min.GetEntry (character);
			ConnectionEntry<float> maxInput = Max.GetEntry (character);
			ConnectionEntry<float> output = Output.GetEntry (character);

			Action updateHandler = () =>
			{
				output.Value = UnityEngine.Random.Range (minInput.Value, maxInput.Value);
			};

			minInput.OnAfterChanged += updateHandler;
			maxInput.OnAfterChanged += updateHandler;

			updateHandler ();
		}

		protected override void OnRemove (IBehaviourContext character)
		{

		}

#if UNITY_EDITOR
		public override Vector2 GetDiamentions ()
		{
			return new Vector2 (140, 54);
		}
#endif
	}
}