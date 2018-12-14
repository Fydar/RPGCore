using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using System;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation ("Item/Roll Float")]
	public class RollFloatNode : BehaviourNode
	{
		public FloatInput Min;
		public FloatInput Max;

		public FloatOutput Output;

		protected override void OnSetup (IBehaviourContext context)
		{
			ConnectionEntry<float> minInput = Min[context];
			ConnectionEntry<float> maxInput = Max[context];
			ConnectionEntry<float> output = Output[context];

			Action updateHandler = () =>
			{
				output.Value = UnityEngine.Random.Range (minInput.Value, maxInput.Value);
			};

			minInput.OnAfterChanged += updateHandler;
			maxInput.OnAfterChanged += updateHandler;

			updateHandler ();
		}

		protected override void OnRemove (IBehaviourContext context)
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
