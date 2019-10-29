using System;

namespace RPGCore.Behaviour
{
	public abstract class ConnectionTypeConverter<TInput, TOutput> : IConnection<TOutput>, IConnectionTypeConverter
	{
		public int ConnectionId => Source.ConnectionId;

		public IConnection<TInput> Source;

		public TOutput Value
		{
			set => throw new InvalidOperationException ("Cannot read value of type converter");
			get => Convert (Source.Value);
		}

		protected abstract TOutput Convert (TInput original);

		public Type ConnectionType => Source.ConnectionType;

		public Type ConvertFromType => typeof (TInput);

		public Type ConvertToType => typeof (TOutput);

		public HandlerCollection Handlers => Source.Handlers;

		public IReadOnlyEventField<TOutput> Mirroring => null;

		public ConnectionTypeConverter ()
		{
		}

		public ConnectionTypeConverter (IConnection source)
		{
			Source = (IConnection<TInput>)source;
		}

		public void Subscribe (INodeInstance node, Action callback) =>
			Source.Subscribe (node, callback);

		public void Unsubscribe (INodeInstance node, Action callback) =>
			Source.Unsubscribe (node, callback);

		public void SetSource (IConnection source)
		{
			Source = (IConnection<TInput>)source;
		}

		public void RegisterInput (INodeInstance node) => Source.RegisterInput (node);

		public void RegisterConverter (IConnectionTypeConverter converter) => Source.RegisterConverter (converter);
		public void StartMirroring (IReadOnlyEventField<TOutput> target) => throw new InvalidOperationException ("Cannot read value of type converter");
		public void StopMirroring () => throw new InvalidOperationException ("Cannot read value of type converter");
		public void Dispose ()
		{
		}
	}
}
