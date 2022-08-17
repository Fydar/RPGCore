namespace RPGCore.Behaviour;

public struct GraphInstanceInput<TType>
{
	internal GraphInstanceData graphInstance;
	internal IInput<TType> input;

	public TType Value { get; }
	public bool HasChanged { get; }

	public GraphInstanceInput(
		GraphInstanceData graphInstance,
		IInput<TType> input,
		TType value,
		bool hasChanged)
	{
		this.graphInstance = graphInstance;
		this.input = input;
		Value = value;
		HasChanged = hasChanged;
	}
}
