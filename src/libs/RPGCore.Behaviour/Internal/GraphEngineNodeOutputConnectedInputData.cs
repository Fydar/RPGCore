namespace RPGCore.Behaviour.Internal;

internal struct GraphEngineNodeOutputConnectedInputData
{
	internal int connectedInputIndex;

	public GraphEngineNodeOutputConnectedInputData(
		int connectedInputIndex)
	{
		this.connectedInputIndex = connectedInputIndex;
	}
}
