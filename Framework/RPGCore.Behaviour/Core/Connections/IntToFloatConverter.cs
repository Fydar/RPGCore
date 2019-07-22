using System;

namespace RPGCore.Behaviour
{
	public class IntToFloatConverter : Connection<float>
	{
		public override event Action OnAfterChanged
		{
			add => Source.OnAfterChanged += value;
			remove => Source.OnAfterChanged -= value;
		}

		public override int ConnectionId => Source.ConnectionId;

		public Connection<int> Source;

		public override float Value
		{
			get => Source.Value;
			set => Source.Value = (int)value;
		}

		public IntToFloatConverter (int connectionId)
			: base (connectionId)
		{
		}

		public void SetSource (Connection source)
		{
			Source = (Connection<int>)source;
		}
	}
}
