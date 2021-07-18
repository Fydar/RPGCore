using RPGCore.Behaviour;

namespace RPGCore.Documentation.Samples.RPGCore.Behaviour
{
	#region default
	public class AddNode : NodeTemplate<AddNode>
	{
		public InputSocket ValueA;
		public InputSocket ValueB;

		public OutputSocket Output;

		public override Instance Create()
		{
			return new AddInstance();
		}

		#region node_instance
		public class AddInstance : Instance
		{
			public Input<float> ValueA;
			public Input<float> ValueB;

			public Output<float> Output;

			// Socket to Connector Mapping
			public override InputMap[] Inputs(ConnectionMapper connections) => new[]
			{
				connections.Connect(ref Template.ValueA, ref ValueA),
				connections.Connect(ref Template.ValueB, ref ValueB)
			};

			public override OutputMap[] Outputs(ConnectionMapper connections) => new[]
			{
				connections.Connect(ref Template.Output, ref Output)
			};

			public override void Setup()
			{
				// Subscribe to events, setup variables...
			}

			public override void OnInputChanged()
			{
				// Fires when an input value changes.
				Output.Value = ValueA.Value + ValueB.Value;
			}

			public override void Remove()
			{
				// Unsubscribe from events, dispose members...
			}
		}
		#endregion node_instance
	}
	#endregion default
}
