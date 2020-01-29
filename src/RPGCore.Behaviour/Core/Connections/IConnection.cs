using System;
using System.Diagnostics;

namespace RPGCore.Behaviour
{
	public interface IConnection
	{
		int ConnectionId { get; }

		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		Type ConnectionType { get; }

		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		bool BufferEvents { get; set; }

		IConnectionTypeConverter UseConverter(Type converterType);
		void RegisterInput(INodeInstance node);
		void Subscribe(INodeInstance node, Action callback);
		void Unsubscribe(INodeInstance node, Action callback);
	}

	public interface IConnection<T> : IConnection, IReadOnlyEventField<T>
	{
	}
}
