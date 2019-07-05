using System;

namespace RPGCore.Behaviour
{
	public class Connection
	{
		public event Action OnAfterChanged;

		protected void InvokeAfterChanged ()
		{
			OnAfterChanged?.Invoke ();
		}
	}

	public class Connection<T> : Connection
	{
		private T internalValue;

		public T Value
		{
			get => internalValue;
			set
			{
				internalValue = value;
				InvokeAfterChanged ();
			}
		}
	}
}
