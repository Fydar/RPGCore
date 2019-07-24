using System;
using System.Diagnostics;

namespace RPGCore.Behaviour
{
	public interface IConnection
	{
		int ConnectionId { get; }

		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		Type ConnectionType { get; }

		void Subscribe (INodeInstance node, Action callback);
		void Unsubscribe (INodeInstance node, Action callback);
	}

	public interface IConnection<T> : IConnection
	{
		T Value { get; set; }
	}
}
