using System;

namespace RPGCore.Behaviour
{
	public sealed class ActivatableItemNode : Node<ActivatableItemNode, ActivatableItemNode.Metadata>
	{
		public OutputSocket Output = new OutputSocket ();

		public override InputMap[] Inputs (IGraphConnections graph, Metadata instance) => null;

		public override OutputMap[] Outputs (IGraphConnections graph, Metadata instance) => new[]
		{
			graph.Connect(ref Output, ref instance.Output),
		};

		public override INodeInstance Create () => new Metadata ();

		public sealed class Metadata : Instance
		{
			public Output<int> Output;

			public override void Setup (IGraphInstance graph, Actor target)
			{

			}

			public override void Remove ()
			{

			}

			public override void OnInputChanged ()
			{

			}
		}
	}
}
