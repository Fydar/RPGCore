using RPGCore.Behaviour.Connections;
using System;
using UnityEngine;

namespace RPGCore.Behaviour.Math
{
	[NodeInformation ("Int/Clamp")]
	public class IntClampNode : BehaviourNode
	{
		public IntInput Value;
		public IntInput Min;
		public IntInput Max;

		public IntOutput Output;

		protected override void OnSetup (IBehaviourContext context)
		{
			ConnectionEntry<int> valueInput = Value[context];
			ConnectionEntry<int> minInput = Min[context];
			ConnectionEntry<int> maxInput = Max[context];
			ConnectionEntry<int> output = Output[context];

			Action updateHandler = () =>
			{
				output.Value = Mathf.Clamp (valueInput.Value, minInput.Value, maxInput.Value);
			};

			valueInput.OnAfterChanged += updateHandler;
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
			return new Vector2 (140, 70);
		}
#endif
	}
}

