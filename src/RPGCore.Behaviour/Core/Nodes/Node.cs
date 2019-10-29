using Newtonsoft.Json;
using System;

namespace RPGCore.Behaviour
{
	public abstract class Node
	{
		[JsonIgnore]
		public LocalId Id { get; set; }
		public abstract INodeInstance CreateInstance ();
		public abstract void Setup (IGraphInstance graph, INodeInstance metadata);

		internal abstract InputMap[] Inputs (IGraphConnections graph, INodeInstance instance);
		internal abstract OutputMap[] Outputs (IGraphConnections graph, INodeInstance instance);
	}

	public abstract class Node<TNode> : Node
		where TNode : Node<TNode>
	{
		public abstract class Instance : INodeInstance
		{
			[JsonIgnore]
			public TNode Node { get; internal set; }

			Node INodeInstance.Node => Node;

			public virtual void OnInputChanged () { }
			public abstract void Remove ();
			public abstract void Setup (IGraphInstance graph);

			public abstract InputMap[] Inputs (IGraphConnections graph, TNode node);
			public abstract OutputMap[] Outputs (IGraphConnections graph, TNode node);
		}

		public abstract Instance Create ();

		public sealed override INodeInstance CreateInstance () => Create ();

		internal sealed override InputMap[] Inputs (IGraphConnections graph, INodeInstance instance)
		{
			return ((Instance)instance).Inputs (graph, (TNode)this);
		}

		internal sealed override OutputMap[] Outputs (IGraphConnections graph, INodeInstance instance)
		{
			return ((Instance)instance).Outputs (graph, (TNode)this);
		}

		public sealed override void Setup (IGraphInstance graph, INodeInstance metadata)
		{
			var instance = (Instance)metadata;
			instance.Node = (TNode)this;

			metadata.Setup (graph);
		}
	}
}
