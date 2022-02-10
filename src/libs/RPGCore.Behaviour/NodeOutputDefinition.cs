using RPGCore.Behaviour.Internal;

namespace RPGCore.Behaviour;

public sealed class NodeOutputDefinition
{
	internal IOutput output;

	public string Name { get; } = string.Empty;

	internal NodeOutputDefinition(IOutput output, string name)
	{
		this.output = output;
		Name = name;
	}
}
