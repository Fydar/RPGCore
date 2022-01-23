using System;

namespace RPGCore.Behaviour;

public readonly struct InputSource
{
	public readonly INodeInstance Node;
	public readonly OutputMap OutputInformation;

	public InputSource(INodeInstance instance, OutputMap outputMapping)
	{
		Node = instance ?? throw new ArgumentNullException(nameof(instance));
		OutputInformation = outputMapping;
	}

	/// <inheritdoc/>
	public override string ToString()
	{
		if (Node == null)
		{
			return $"No Output";
		}
		else
		{
			return $"Output {OutputInformation.ConnectionId} from node {Node}";
		}
	}
}
