using Newtonsoft.Json;

namespace RPGCore.Behaviour
{
	public struct Output<T>
	{
		public Connection<T> Connection { get; }

		[JsonIgnore]
		public bool IsConnected => Connection != null;

		public T Value
		{
			set
			{
				if (Connection == null)
					return;

				Connection.Value = value;
			}
		}

		public Output (Connection<T> connection)
		{
			Connection = connection;
		}
	}
}
