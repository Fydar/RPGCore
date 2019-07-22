using System;
using System.Diagnostics;

namespace RPGCore.Behaviour
{
	public abstract class Connection
	{
		public event Action OnAfterChanged;

		public abstract object ObjectValue { get; set; }

		protected void InvokeAfterChanged()
		{
			OnAfterChanged?.Invoke();
		}
	}

	public sealed class Connection<T> : Connection
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private T internalValue;

		public override object ObjectValue
		{
			get
			{
				return internalValue;
			}
			set
			{
				internalValue = (T)value;
				InvokeAfterChanged();
			}
		}

		public T Value
		{
			get => internalValue;
			set
			{
				internalValue = value;
				InvokeAfterChanged();
			}
		}

		public override string ToString() => $"Connection, Value = {internalValue}";
	}
}
