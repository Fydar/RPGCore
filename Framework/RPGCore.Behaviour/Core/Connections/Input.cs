using System;

namespace RPGCore.Behaviour
{
	public struct Input<T>
	{
		private IConnectionConverter<T> Converter { get; set; }

		public Connection Connection { get; private set; }

		public bool IsConnected => Connection != null;

		public T Value => Connection != null
			? Converter.Value
			: default (T);

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

		public void SetConnection(Connection connection, IConnectionConverter<T> converter)
		{
			Connection = connection;
			
			Converter = converter;
			Converter.SetSource(connection);
		}

		public override string ToString () => $"Inputted {Connection}";
	}
}
