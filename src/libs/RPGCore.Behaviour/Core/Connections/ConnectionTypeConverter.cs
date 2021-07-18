using RPGCore.Events;
using System;

namespace RPGCore.Behaviour
{
	public abstract class ConnectionTypeConverter<TInput, TOutput> : IConnection<TOutput>, IConnectionTypeConverter
	{
		public int ConnectionId => Source.ConnectionId;

		public IConnection<TInput> Source;

		public TOutput Value => Convert(Source.Value);

		protected abstract TOutput Convert(TInput original);

		public Type ConnectionType => Source.ConnectionType;

		public Type ConvertFromType => typeof(TInput);

		public Type ConvertToType => typeof(TOutput);

		public EventFieldHandlerCollection Handlers => Source.Handlers;

		public IReadOnlyEventField<TOutput> Mirroring => null;

		public bool BufferEvents { get; set; }

		public ConnectionTypeConverter()
		{
		}

		public ConnectionTypeConverter(IConnection source)
		{
			Source = (IConnection<TInput>)source;
		}

		public void Subscribe(INodeInstance node, Action callback)
		{
			Source.Subscribe(node, callback);
		}

		public void Unsubscribe(INodeInstance node, Action callback)
		{
			Source.Unsubscribe(node, callback);
		}

		public void SetSource(IConnection source)
		{
			Source = (IConnection<TInput>)source;
		}

		public void RegisterInput(INodeInstance node)
		{
			Source.RegisterInput(node);
		}

		public IConnectionTypeConverter UseConverter(Type converterType)
		{
			return Source.UseConverter(converterType);
		}
	}
}
