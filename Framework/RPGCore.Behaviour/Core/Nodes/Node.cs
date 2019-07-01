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

		public abstract InputMap[] Inputs (IGraphInstance graph, object instance);
		public abstract OutputMap[] Outputs (IGraphInstance graph, object instance);
	}

	public abstract class Node<T> : Node
		where T : INodeInstance, new()
	{
		public override Type MetadataType => typeof (T);

		public abstract InputMap[] Inputs (IGraphInstance graph, T instance);
		public abstract OutputMap[] Outputs (IGraphInstance graph, T instance);

		public override INodeInstance Create ()
		{
			return new T ();
		}

		public sealed override InputMap[] Inputs (IGraphInstance graph, object instance)
		{
			return Inputs (graph, (T)instance);
		}

		public sealed override OutputMap[] Outputs (IGraphInstance graph, object instance)
		{
			return Outputs (graph, (T)instance);
		}

		public sealed override void Setup (IGraphInstance graph, INodeInstance metadata, Actor target)
		{
			metadata.Setup (graph, this, target);
		}
	}
}
