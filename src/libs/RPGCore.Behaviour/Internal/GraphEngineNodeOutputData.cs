namespace RPGCore.Behaviour.Internal;

internal struct GraphEngineNodeOutputData
{
	internal IOutput output;
	internal string name;
	internal int outputConnectedInputsStartIndex;
	internal int outputConnectedInputsCount;

	public GraphEngineNodeOutputData(
		IOutput output,
		string name,
		int outputConnectedInputsStartIndex,
		int outputConnectedInputsCount)
	{
		this.output = output;
		this.name = name;
		this.outputConnectedInputsStartIndex = outputConnectedInputsStartIndex;
		this.outputConnectedInputsCount = outputConnectedInputsCount;
	}
}
