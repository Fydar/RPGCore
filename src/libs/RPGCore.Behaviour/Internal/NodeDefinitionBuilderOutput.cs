namespace RPGCore.Behaviour.Internal;

internal sealed class NodeDefinitionBuilderOutput
{
	internal IOutput output;

	public string Name { get; } = string.Empty;

	internal NodeDefinitionBuilderOutput(
		IOutput output,
		string name)
	{
		this.output = output;
		Name = name;
	}
}
