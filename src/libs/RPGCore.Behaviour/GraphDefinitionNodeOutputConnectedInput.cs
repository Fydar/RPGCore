namespace RPGCore.Behaviour;

public struct GraphDefinitionNodeOutputConnectedInput
{
	public int InputNode { get; }
	public int InputIndex { get; }

	public GraphDefinitionNodeOutputConnectedInput(int inputNode, int inputIndex)
	{
		InputNode = inputNode;
		InputIndex = inputIndex;
	}
}
