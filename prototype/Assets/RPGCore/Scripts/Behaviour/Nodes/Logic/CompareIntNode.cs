using RPGCore.Behaviour.Connections;
using System;

namespace RPGCore.Behaviour.Logic
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

		protected override void OnSetup (IBehaviourContext context)
		{
			ConnectionEntry<int> valueAInput = ValueA[context];
			ConnectionEntry<int> valueBInput = ValueB[context];
			ConnectionEntry<bool> output = Output[context];

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

		protected override void OnRemove (IBehaviourContext context)
		{
		}
	}
}

