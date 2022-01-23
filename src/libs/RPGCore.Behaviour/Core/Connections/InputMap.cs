using System;

namespace RPGCore.Behaviour;

public readonly struct InputMap
{
	public readonly int ConnectionId;
	public readonly Type ConnectionType;

	public InputMap(InputSocket source, Type connectionType)
	{
		ConnectionType = connectionType;
		ConnectionId = source.ConnectionId;
	}

	/// <inheritdoc/>
	public override string ToString()
	{
		if (ConnectionId == -1)
		{
			return $"Unmapped Input of type {ConnectionType}";
		}
		else
		{
			return $"Input {ConnectionId} of type {ConnectionType}";
		}
	}
}
