using Newtonsoft.Json;

namespace RPGCore.Behaviour
{
	public abstract class NodeInstanceBase : INodeInstance
	{
		[JsonIgnore]
		public IGraphInstance Graph { get; internal set; }
		[JsonIgnore]
		internal abstract Node NodeBase { get; }

		Node INodeInstance.NodeBase => NodeBase;

		public virtual void OnInputChanged() { }
		public abstract void Remove();
		public abstract void Setup();

		public abstract InputMap[] Inputs(ConnectionMapper connections);
		public abstract OutputMap[] Outputs(ConnectionMapper connections);
	}
}
