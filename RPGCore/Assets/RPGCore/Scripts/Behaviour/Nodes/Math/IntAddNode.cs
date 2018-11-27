using RPGCore.Behaviour.Connections;
using System;
using UnityEngine;

namespace RPGCore.Behaviour.Math
{
	[NodeInformation ("Int/Add")]
	public class IntAddNode : BehaviourNode
	{
		public IntInput ValueA;
		public IntInput ValueB;

		public IntOutput Output;

		protected override void OnSetup (IBehaviourContext context)
		{
			ConnectionEntry<int> valueAInput = ValueA[context];
			ConnectionEntry<int> valueBInput = ValueB[context];
			ConnectionEntry<int> output = Output[context];

			Action updateHandler = () =>
			{
				output.Value = valueAInput.Value + valueBInput.Value;
			};

			valueAInput.OnAfterChanged += updateHandler;
			valueBInput.OnAfterChanged += updateHandler;

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