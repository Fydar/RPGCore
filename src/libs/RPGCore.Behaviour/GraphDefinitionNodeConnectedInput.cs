namespace RPGCore.Behaviour;

public struct GraphDefinitionNodeConnectedInput
{
	public ConnectedInput Input { get; internal set; }
	public int ConnectedToNode { get; internal set; }
	public int ConnectedToNodeOutput { get; internal set; }

	internal GraphDefinitionNodeConnectedInput(
		ConnectedInput input)
	{
		Input = input;
		ConnectedToNode = 0;
		ConnectedToNodeOutput = 0;
	}

	public override string ToString()
	{
		return $"{Input}";
	}
}
