namespace RPGCore.Behaviour.Internal;

internal struct GraphEngineNodeConnectedInputData
{
	internal IInput input;
	internal int outputIndex;

	public GraphEngineNodeConnectedInputData(
		IInput input,
		int outputIndex)
	{
		this.input = input;
		this.outputIndex = outputIndex;
	}
}
