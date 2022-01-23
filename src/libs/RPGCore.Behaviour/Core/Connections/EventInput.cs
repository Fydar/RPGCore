using System;

namespace RPGCore.Behaviour;

public readonly struct EventInput
{
	public IConnection Connection { get; }
	public INodeInstance Parent { get; }

	public bool IsConnected => Connection != null;

	public event Action OnAfterChanged
	{
		add
		{
			if (Connection == null)
			{
				return;
			}

			Connection.Subscribe(Parent, value);
		}
		remove
		{
			if (Connection == null)
			{
				return;
			}

			Connection.Unsubscribe(Parent, value);
		}
	}

	public EventInput(INodeInstance parent, IConnection connection)
	{
		Parent = parent;
		Connection = connection;
	}

	/// <inheritdoc/>
	public override string ToString()
	{
		if (IsConnected)
		{
			return $"Inputted {Connection}";
		}
		else
		{
			return "Unconnected Input";
		}
	}
}
