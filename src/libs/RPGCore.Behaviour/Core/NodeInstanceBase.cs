using Newtonsoft.Json;
using System.Diagnostics;

namespace RPGCore.Behaviour
{
	public abstract class NodeInstanceBase : INodeInstance
	{
		[JsonIgnore]
		public IGraphInstance Graph { get; internal set; }

		[JsonIgnore]
		internal abstract NodeTemplate TemplateBase { get; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		NodeTemplate INodeInstance.Template => TemplateBase;

		public virtual void OnInputChanged()
		{
		}

		public abstract void Remove();

		public abstract void Setup();

		public abstract InputMap[] Inputs(ConnectionMapper connections);

		public abstract OutputMap[] Outputs(ConnectionMapper connections);
	}
}
