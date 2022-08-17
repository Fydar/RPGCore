using RPGCore.Behaviour.Fluent;
using System;

namespace RPGCore.Behaviour;

public struct GraphDefinitionNodeOutput
{
	public IOutput Output { get; }
	public int LocalId { get; }
	public string Name { get; } = string.Empty;
	public GraphDefinitionNodeOutputConnectedInput[] ConnectedInputIndexes { get; internal set; }

	internal GraphDefinitionNodeOutput(
		IOutput output,
		int localId,
		string name)
	{
		Output = output;
		LocalId = localId;
		Name = name;
		ConnectedInputIndexes = Array.Empty<GraphDefinitionNodeOutputConnectedInput>();
	}

	public override string ToString()
	{
		return $"{Name}";
	}
}
