using RPGCore.Behaviour.Connections;
using System;
using UnityEngine;

namespace RPGCore.Behaviour.Math
{
	[NodeInformation ("Float/Minus")]
	public class FloatMinusNode : BehaviourNode
	{
		public FloatInput ValueA;
		public FloatInput ValueB;

		public FloatOutput Output;

		protected override void OnSetup (IBehaviourContext character)
		{
			ConnectionEntry<float> valueAInput = ValueA.GetEntry (character);
			ConnectionEntry<float> valueBInput = ValueB.GetEntry (character);
			ConnectionEntry<float> output = Output.GetEntry (character);

			Action updateHandler = () =>
			{
				output.Value = valueAInput.Value - valueBInput.Value;
			};

			valueAInput.OnAfterChanged += updateHandler;
			valueBInput.OnAfterChanged += updateHandler;

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