using System;

namespace RPGCore.Behaviour
{
	public class IntToFloatConverter : Connection<float>
	{
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

		public override void Subscribe (INodeInstance node, Action callback) =>
			Source.Subscribe (node, callback);

		public override void Unsubscribe (INodeInstance node, Action callback) =>
			Source.Unsubscribe (node, callback);

		public void SetSource (Connection source)
		{
			Source = (Connection<int>)source;
		}
	}
}
