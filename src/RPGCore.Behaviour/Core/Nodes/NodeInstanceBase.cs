using Newtonsoft.Json;
using System.Diagnostics;

namespace RPGCore.Behaviour
{
	public abstract class NodeInstanceBase : INodeInstance
	{
		[JsonIgnore]
		public IGraphInstance Graph { get; internal set; }
		[JsonIgnore]
		internal abstract Node NodeBase { get; }

		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		Node INodeInstance.NodeBase => NodeBase;

		public virtual void OnInputChanged() { }
		public abstract void Remove();
		public abstract void Setup();

		public abstract InputMap[] Inputs(ConnectionMapper connections);
		public abstract OutputMap[] Outputs(ConnectionMapper connections);
	}
}
