using System;

namespace RPGCore.Behaviour;

public readonly struct OutputSource
{
	public readonly INodeInstance Instance;
	public readonly InputMap InputInformation;
	public NodeTemplate Node => Instance.Template;

	public OutputSource(INodeInstance instance, InputMap outputMapping)
	{
		Instance = instance ?? throw new ArgumentNullException(nameof(instance));
		InputInformation = outputMapping;
	}

	/// <inheritdoc/>
	public override string ToString()
	{
		if (Instance == null)
		{
			return $"No Input";
		}
		else
		{
			return $"Input {InputInformation.ConnectionId} from node {Instance}";
		}
	}
}
