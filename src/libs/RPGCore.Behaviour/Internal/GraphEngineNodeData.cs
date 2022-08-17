namespace RPGCore.Behaviour.Internal;

internal struct GraphEngineNodeData
{
	internal Node node;
	internal NodeRuntime nodeRuntime;
	internal int componentsStartIndex;
	internal int componentsCount;
	internal int nodeOutputStartIndex;
	internal int nodeOutputCount;
	internal int nodeConnectedInputStartIndex;
	internal int nodeConnectedInputCount;

	public GraphEngineNodeData(
		Node node,
		NodeRuntime nodeRuntime,
		int componentsStartIndex,
		int componentsCount,
		int nodeOutputStartIndex,
		int nodeOutputCount,
		int nodeConnectedInputStartIndex,
		int nodeConnectedInputCount)
	{
		this.node = node;
		this.nodeRuntime = nodeRuntime;
		this.componentsStartIndex = componentsStartIndex;
		this.componentsCount = componentsCount;
		this.nodeOutputStartIndex = nodeOutputStartIndex;
		this.nodeOutputCount = nodeOutputCount;
		this.nodeConnectedInputStartIndex = nodeConnectedInputStartIndex;
		this.nodeConnectedInputCount = nodeConnectedInputCount;
	}
}
