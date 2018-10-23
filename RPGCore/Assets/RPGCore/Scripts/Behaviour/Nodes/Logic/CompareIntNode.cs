using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation ("Logic/Compare Int")]
	public class CompareIntNode : BehaviourNode
	{
		public enum Comparison
		{
			Greater,
			Less,
			Equal,
		}

		public IntInput ValueA;
		public Comparison Operator;
		public IntInput ValueB;

		public BoolOutput Output;


		protected override void OnSetup (IBehaviourContext character)
		{
			ConnectionEntry<int> valueAInput = ValueA.GetEntry (character);
			ConnectionEntry<int> valueBInput = ValueB.GetEntry (character);
			ConnectionEntry<bool> output = Output.GetEntry (character);

			Action updateHandler = () =>
			{
				if (Operator == Comparison.Equal)
					output.Value = valueAInput.Value == valueBInput.Value;
				else if (Operator == Comparison.Greater)
					output.Value = valueAInput.Value > valueBInput.Value;
				else if (Operator == Comparison.Less)
					output.Value = valueAInput.Value < valueBInput.Value;
			};

			valueAInput.OnAfterChanged += updateHandler;
			valueBInput.OnAfterChanged += updateHandler;

			updateHandler ();
		}

		protected override void OnRemove (IBehaviourContext character)
		{
		}
	}
}