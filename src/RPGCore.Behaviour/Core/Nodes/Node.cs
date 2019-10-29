using Newtonsoft.Json;
using System;

namespace RPGCore.Behaviour
{
	public abstract class Node
	{
		[JsonIgnore]
		public LocalId Id { get; set; }

		[JsonIgnore]
		public abstract Type InstanceType { get; }
		public abstract INodeInstance CreateInstance ();
		public abstract void Setup (IGraphInstance graph, INodeInstance metadata);

		public abstract InputMap[] Inputs (IGraphConnections graph, INodeInstance instance);
		public abstract OutputMap[] Outputs (IGraphConnections graph, INodeInstance instance);
	}

	public abstract class Node<TNode, TInstance> : Node
		where TNode : Node<TNode, TInstance>
		where TInstance : Node<TNode, TInstance>.Instance
	{
		public abstract class Instance : INodeInstance
		{
			[JsonIgnore]
			public TNode Node { get; internal set; }

			Node INodeInstance.Node => Node;

			public virtual void OnInputChanged () { }
			public abstract void Remove ();
			public abstract void Setup (IGraphInstance graph);
		}

		public override Type InstanceType => typeof (TInstance);

		public abstract InputMap[] Inputs (IGraphConnections graph, TInstance instance);
		public abstract OutputMap[] Outputs (IGraphConnections graph, TInstance instance);
		public abstract TInstance Create ();

		public sealed override INodeInstance CreateInstance () => Create ();

		public sealed override InputMap[] Inputs (IGraphConnections graph, INodeInstance instance)
		{
			return Inputs (graph, (TInstance)instance);
		}

		public sealed override OutputMap[] Outputs (IGraphConnections graph, INodeInstance instance)
		{
			return Outputs (graph, (TInstance)instance);
		}

		public sealed override void Setup (IGraphInstance graph, INodeInstance metadata)
		{
			var instance = (Instance)metadata;
			instance.Node = (TNode)this;

			metadata.Setup (graph);
		}
	}
}
