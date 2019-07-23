using System;

namespace RPGCore.Behaviour
{
	public struct Input<T>
	{
		public Connection<T> Connection { get; private set; }
		public INodeInstance Parent { get; private set; }

		public bool IsConnected => Connection != null;

		public T Value => Connection != null
			? Connection.Value
			: default (T);

		public event Action OnAfterChanged
		{
			add
			{
				if (Connection == null)
					return;

				Connection.Subscribe (Parent, value);
			}
			remove
			{
				if (Connection == null)
					return;

				Connection.Unsubscribe (Parent, value);
			}
		}

		public Input(INodeInstance parent, Connection connection)
		{
			Parent = parent;
			Connection = (Connection<T>)connection;
		}

		public override string ToString ()
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
