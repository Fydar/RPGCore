namespace RPGCore.Behaviour;

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

	/// <inheritdoc/>
	public override string ToString()
	{
		return $"{Instance?.Template?.Id}: {Instance?.Template.GetType().Name}";
	}
}
