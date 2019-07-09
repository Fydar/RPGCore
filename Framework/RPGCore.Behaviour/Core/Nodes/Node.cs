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

	public abstract class Node<T> : Node
		where T : INodeInstance, new()
	{
		public override Type MetadataType => typeof (T);

		public abstract InputMap[] Inputs (IGraphConnections graph, T instance);
		public abstract OutputMap[] Outputs (IGraphConnections graph, T instance);

		public override INodeInstance Create ()
		{
			return new T ();
		}

		public sealed override InputMap[] Inputs (IGraphConnections graph, object instance)
		{
			return Inputs (graph, (T)instance);
		}

		public sealed override OutputMap[] Outputs (IGraphConnections graph, object instance)
		{
			return Outputs (graph, (T)instance);
		}

		public sealed override void Setup (IGraphInstance graph, INodeInstance metadata, Actor target)
		{
			metadata.Setup (graph, this, target);
		}
	}
}
