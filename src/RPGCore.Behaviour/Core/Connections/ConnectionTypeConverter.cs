using System;

namespace RPGCore.Behaviour
{
	public abstract class ConnectionTypeConverter<A, B> : IConnection<B>, IConnectionTypeConverter
	{
		public int ConnectionId => Source.ConnectionId;

		public IConnection<A> Source;

		public B Value
		{
			set => throw new InvalidOperationException ("Cannot read value of type converter");
			get => Convert (Source.Value);
		}

		protected abstract B Convert (A original);

		public Type ConnectionType => Source.ConnectionType;

		public Type ConvertFromType => typeof (A);

		public Type ConvertToType => typeof (B);

		public ConnectionTypeConverter ()
		{
		}

		public ConnectionTypeConverter (IConnection source)
		{
			Source = (IConnection<A>)source;
		}

		public void Subscribe (INodeInstance node, Action callback) =>
			Source.Subscribe (node, callback);

		public void Unsubscribe (INodeInstance node, Action callback) =>
			Source.Unsubscribe (node, callback);

		public void SetSource (IConnection source)
		{
			Source = (IConnection<A>)source;
		}

		public void RegisterInput (INodeInstance node) => Source.RegisterInput (node);

		public void RegisterConverter (IConnectionTypeConverter converter) => Source.RegisterConverter (converter);
	}
}
