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

		protected override void OnSetup (IBehaviourContext character)
		{
			ConnectionEntry<int> valueInput = Value.GetEntry (character);
			ConnectionEntry<int> minInput = Min.GetEntry (character);
			ConnectionEntry<int> maxInput = Max.GetEntry (character);
			ConnectionEntry<int> output = Output.GetEntry (character);

			Action updateHandler = () =>
			{
				output.Value = Mathf.Clamp(valueInput.Value, minInput.Value, maxInput.Value);
			};

			valueInput.OnAfterChanged += updateHandler;
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
			return new Vector2 (140, 70);
		}
#endif
	}
}