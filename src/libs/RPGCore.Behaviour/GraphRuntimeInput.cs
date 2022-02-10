namespace RPGCore.Behaviour;

public struct GraphRuntimeInput<TType>
{
	internal GraphRuntimeData graphInstance;
	internal IInput<TType> input;

	public TType Value { get; }
	public bool HasChanged { get; }

	public GraphRuntimeInput(GraphRuntimeData graphInstance, IInput<TType> input, TType value, bool hasChanged)
	{
		this.graphInstance = graphInstance;
		this.input = input;
		Value = value;
		HasChanged = hasChanged;
	}
}
