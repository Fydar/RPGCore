using RPGCore.Behaviour.Connections;
using System;
using UnityEngine;

namespace RPGCore.Behaviour.Math
{
	[NodeInformation("Float/Multiply")]
	public class FloatMultiplyNode : BehaviourNode
	{
		public FloatInput ValueA;
		public FloatInput ValueB;

		public FloatOutput Output;

		protected override void OnSetup(IBehaviourContext context)
		{
			var valueAInput = ValueA[context];
			var valueBInput = ValueB[context];
			var output = Output[context];

			Action updateHandler = () =>
			{
				output.Value = valueAInput.Value * valueBInput.Value;
			};

			valueAInput.OnAfterChanged += updateHandler;
			valueBInput.OnAfterChanged += updateHandler;

			updateHandler();
		}

		protected override void OnRemove(IBehaviourContext context)
		{
		}

#if UNITY_EDITOR
		public override Vector2 GetDiamentions()
		{
			return new Vector2(140, 54);
		}
#endif
	}
}

