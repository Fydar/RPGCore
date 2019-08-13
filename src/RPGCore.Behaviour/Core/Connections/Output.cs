using Newtonsoft.Json;

namespace RPGCore.Behaviour
{
	public struct Output<T>
	{
		public IConnection<T> Connection { get; }

		[JsonIgnore]
		public bool IsConnected => Connection != null;

		public T Value
		{
			set
			{
				if (Connection == null)
				{
					return;
				}

				Connection.Value = value;
			}
		}

		public Output (IConnection<T> connection)
		{
			Connection = connection;
		}

		public override string ToString ()
		{
			if (IsConnected)
			{
				return $"Outputted {Connection}";
			}
			else
			{
				return "Unconnected Output";
			}
		}
	}
}
