using System;
using System.Diagnostics;

namespace RPGCore.Behaviour
{
	public abstract class Connection
	{
		public virtual int ConnectionId { get; set; }

		public abstract event Action OnAfterChanged;

		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		public abstract object ObjectValue { get; set; }
	}

	public class Connection<T> : Connection
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private T GenericValue;

		public override event Action OnAfterChanged;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public override object ObjectValue
		{
			get
			{
				return GenericValue;
			}
			set
			{
				GenericValue = (T)value;
				InvokeAfterChanged();
			}
		}

		public virtual T Value
		{
			get => GenericValue;
			set
			{
				GenericValue = value;
				InvokeAfterChanged();
			}
		}

		public Connection(int connectionId)
		{
			ConnectionId = connectionId;
		}

		protected void InvokeAfterChanged ()
		{
			OnAfterChanged?.Invoke ();
		}

		public override string ToString() => $"Connection {ConnectionId}, Value = {GenericValue}";
	}
}
