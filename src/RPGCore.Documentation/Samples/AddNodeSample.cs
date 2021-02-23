using RPGCore.Behaviour;

namespace RPGCore.Documentation.Samples.AddNodeSample
{
	#region default
	// Node Template Definition
	public class AddNode : NodeTemplate<AddNode>
	{
		// Node Sockets
		public InputSocket ValueA;
		public InputSocket ValueB;

		public OutputSocket Output;

		// Boilerplate Create Method
		public override Instance Create() => new AddInstance();

		// Node Instance Definition
		public class AddInstance : Instance
		{
			// Node Instance Connectors
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

			// Node Logic
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
	}
	#endregion default
}
