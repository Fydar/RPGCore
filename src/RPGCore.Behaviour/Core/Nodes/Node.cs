using Newtonsoft.Json;
using System.Diagnostics;

namespace RPGCore.Behaviour
{
	public abstract class Node
	{
		[JsonIgnore]
		public LocalId Id { get; set; }

		public abstract INodeInstance CreateInstance ();
		public abstract void Setup (IGraphInstance graph, INodeInstance metadata);

		internal abstract InputMap[] Inputs (IGraphConnections connections, INodeInstance instance);
		internal abstract OutputMap[] Outputs (IGraphConnections connections, INodeInstance instance);
	}

	public abstract class Node<TNode> : Node
		where TNode : Node<TNode>
	{
		public abstract class Instance : NodeInstanceBase
		{
			[JsonIgnore]
			public TNode Node { get; internal set; }

			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			internal override Node NodeBase => Node;
		}

		public abstract Instance Create ();

		public sealed override INodeInstance CreateInstance () => Create ();

		internal sealed override InputMap[] Inputs (IGraphConnections connections, INodeInstance instance)
		{
			var castedInstance = (Instance)instance;
			castedInstance.Node = (TNode)this;

			return castedInstance.Inputs (connections);
		}

		internal sealed override OutputMap[] Outputs (IGraphConnections connections, INodeInstance instance)
		{
			var castedInstance = (Instance)instance;
			castedInstance.Node = (TNode)this;

			return castedInstance.Outputs (connections);
		}

		public sealed override void Setup (IGraphInstance graph, INodeInstance metadata)
		{
			var instance = (Instance)metadata;
			instance.Node = (TNode)this;
			instance.Graph = graph;

			metadata.Setup ();
		}
	}
}
