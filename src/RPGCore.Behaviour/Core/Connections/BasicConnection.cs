using System;
using System.Diagnostics;

namespace RPGCore.Behaviour
{
	public class BasicConnection<T> : EventConnection, IConnection<T>
	{
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		private T GenericValue;

		public virtual T Value
		{
			get => GenericValue;
			set
			{
				GenericValue = value;
				InvokeAfterChanged ();
			}
		}

		public override Type ConnectionType => typeof (T);

		public BasicConnection (int connectionId)
			: base (connectionId)
		{
		}

		public override string ToString () => $"Connection {ConnectionId}, Value = {GenericValue}";
	}
}
