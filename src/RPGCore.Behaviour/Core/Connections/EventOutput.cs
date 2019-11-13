using Newtonsoft.Json;

namespace RPGCore.Behaviour
{
	public struct EventOutput
	{
		public IConnection Connection { get; }

		[JsonIgnore]
		public bool IsConnected => Connection != null;

		public EventOutput(IConnection connection)
		{
			Connection = connection;
		}

		public override string ToString()
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
