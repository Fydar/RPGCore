namespace RPGCore.Behaviour
{
	public struct GraphInstanceNode
	{
		public INodeInstance Instance;
		public InputMap[] Inputs;
		public OutputMap[] Outputs;

		internal GraphInstanceNode(INodeInstance instance, InputMap[] inputs, OutputMap[] outputs)
		{
			Instance = instance;
			Inputs = inputs;
			Outputs = outputs;
		}

		public override string ToString()
		{
			return $"{Instance?.NodeBase?.Id}: {Instance?.NodeBase.GetType().Name}";
		}
	}
}
