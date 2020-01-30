using Newtonsoft.Json;

namespace RPGCore.Behaviour
{
	public readonly struct Output<T>
	{
		public OutputConnection<T> Connection { get; }

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

		public Output(OutputConnection<T> connection)
		{
			Connection = connection;
		}

		public void StartMirroring(IReadOnlyEventField<T> target)
		{
			Connection.StartMirroring(target);
		}

		public void StopMirroring()
		{
			Connection.StopMirroring();
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
