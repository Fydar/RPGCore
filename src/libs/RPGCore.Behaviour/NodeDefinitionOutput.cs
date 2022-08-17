namespace RPGCore.Behaviour;

public struct NodeDefinitionOutput
{
	public IOutput Output { get; }
	public string Name { get; }

	internal NodeDefinitionOutput(
		IOutput output,
		string name)
	{
		Output = output;
		Name = name;
	}
}
