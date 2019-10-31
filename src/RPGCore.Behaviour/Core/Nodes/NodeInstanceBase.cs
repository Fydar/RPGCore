namespace RPGCore.Behaviour
{
	public abstract class NodeInstanceBase : INodeInstance
	{
		public IGraphInstance Graph { get; internal set; }
		internal abstract Node NodeBase { get; }

		Node INodeInstance.NodeBase => NodeBase;

		public virtual void OnInputChanged () { }
		public abstract void Remove ();
		public abstract void Setup ();

		public abstract InputMap[] Inputs (IGraphConnections connections);
		public abstract OutputMap[] Outputs (IGraphConnections connections);
	}
}
