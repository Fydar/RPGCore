using System;

namespace RPGCore.Behaviour
{
	public struct Input<T>
	{
		public Connection<T> Connection { get; }

		public T Value => Connection != null
			? Connection.Value
			: default(T);

		public event Action OnAfterChanged
		{
			add
			{
				if (Connection == null)
					return;

				Connection.OnAfterChanged += value;
			}
			remove
			{
				if (Connection == null)
					return;

				Connection.OnAfterChanged -= value;
			}
		}

		public Input (Connection<T> connection)
		{
			Connection = connection;
		}
	}
}
