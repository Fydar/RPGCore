using Newtonsoft.Json;
using System.Diagnostics;

namespace RPGCore.Behaviour
{
	public abstract class NodeTemplate
	{
		[JsonIgnore]
		public LocalId Id { get; set; }

		public abstract INodeInstance CreateInstance();
		public abstract void Setup(IGraphInstance graph, INodeInstance metadata);

		internal abstract InputMap[] Inputs(ConnectionMapper connections, INodeInstance instance);
		internal abstract OutputMap[] Outputs(ConnectionMapper connections, INodeInstance instance);
	}

	public abstract class NodeTemplate<TNode> : NodeTemplate
		where TNode : NodeTemplate<TNode>
	{
		public abstract class Instance : NodeInstanceBase
		{
			[JsonIgnore]
			public TNode Template { get; internal set; }

			[DebuggerBrowsable (DebuggerBrowsableState.Never)]
			internal override NodeTemplate TemplateBase => Template;
		}

		public abstract Instance Create();

		public sealed override INodeInstance CreateInstance() => Create ();

		internal sealed override InputMap[] Inputs(ConnectionMapper connections, INodeInstance instance)
		{
			var castedInstance = (Instance)instance;
			castedInstance.Template = (TNode)this;

			return castedInstance.Inputs (connections);
		}

		internal sealed override OutputMap[] Outputs(ConnectionMapper connections, INodeInstance instance)
		{
			var castedInstance = (Instance)instance;
			castedInstance.Template = (TNode)this;

			return castedInstance.Outputs (connections);
		}

		public sealed override void Setup(IGraphInstance graph, INodeInstance metadata)
		{
			var instance = (Instance)metadata;
			instance.Template = (TNode)this;
			instance.Graph = graph;

			metadata.Setup ();
		}
	}
}
