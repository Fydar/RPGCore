using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using System;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation ("Item/Roll")]
	public class RollNode : BehaviourNode
	{
		public IntInput Min;
		public IntInput Max;

		public IntOutput Output;

		protected override void OnSetup (IBehaviourContext context)
		{
			ConnectionEntry<int> minInput = Min[context];
			ConnectionEntry<int> maxInput = Max[context];
			ConnectionEntry<int> output = Output[context];

			Action updateHandler = () =>
			{
				output.Value = UnityEngine.Random.Range (minInput.Value, maxInput.Value + 1);
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

