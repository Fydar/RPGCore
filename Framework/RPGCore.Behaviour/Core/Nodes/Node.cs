using Newtonsoft.Json;
using System;

namespace RPGCore.Behaviour
{
	public abstract class Node
	{
		[JsonIgnore]
		public LocalId Id { get; set; }

		public abstract Type MetadataType { get; }
		public abstract INodeInstance Create ();
		public abstract void Setup (IGraphInstance graph, INodeInstance metadata, Actor target);

		public abstract InputMap[] Inputs (IGraphConnections graph, object instance);
		public abstract OutputMap[] Outputs (IGraphConnections graph, object instance);
	}

	public abstract class Node<TNode, TInstance> : Node
		where TNode : Node<TNode, TInstance>
		where TInstance : Node<TNode, TInstance>.Instance
	{
		public abstract class Instance : INodeInstance
		{
			public TNode Node;

			public abstract void OnInputChanged ();
			public abstract void Remove ();
			public abstract void Setup (IGraphInstance graph, Actor target);
		}

		public override Type MetadataType => typeof (TInstance);

		public abstract InputMap[] Inputs (IGraphConnections graph, TInstance instance);
		public abstract OutputMap[] Outputs (IGraphConnections graph, TInstance instance);

		public sealed override InputMap[] Inputs (IGraphConnections graph, object instance)
		{
			return Inputs (graph, (TInstance)instance);
		}

		public sealed override OutputMap[] Outputs (IGraphConnections graph, object instance)
		{
			return Outputs (graph, (TInstance)instance);
		}

		public sealed override void Setup (IGraphInstance graph, INodeInstance metadata, Actor target)
		{
			var instance = (Instance)metadata;
			instance.Node = (TNode)this;

			metadata.Setup (graph, target);
		}
	}
}
