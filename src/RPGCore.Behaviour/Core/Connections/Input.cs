using RPGCore.Events;
using System;

namespace RPGCore.Behaviour
{
	public readonly struct Input<T> : IReadOnlyEventField<T>
	{
		public IConnection<T> Connection { get; }
		public INodeInstance Parent { get; }

		public bool IsConnected => Connection != null;

		public T Value => Connection != null
			? Connection.Value
			: default;

		public EventFieldHandlerCollection Handlers => Connection.Handlers;

		public InputSource Source => Parent.Graph.GetSource(this);

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

		public Input(INodeInstance parent, IConnection connection)
		{
			Parent = parent;
			Connection = (IConnection<T>)connection;
		}

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
}
