namespace RPGCore.Behaviour
{
	public struct Output<T>
	{
		public Connection<T> Connection { get; }

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
