using System;

namespace RPGCore.Behaviour
{
	public class ConnectionEntry
	{
		public event Action OnBeforeChanged;
		public event Action OnAfterChanged;

		protected void InvokeBeforeChanged ()
		{
			if (OnBeforeChanged != null)
				OnBeforeChanged ();
		}

		protected void InvokeAfterChanged ()
		{
			if (OnAfterChanged != null)
				OnAfterChanged ();
		}
	}

	public class ConnectionEntry<T> : ConnectionEntry, ISocketConvertable<T>, ISocketType<T>
	{
		private T lastValue;

		public T Value
		{
			get
			{
				return lastValue;
			}
			set
			{
				InvokeBeforeChanged ();

				lastValue = value;

				InvokeAfterChanged ();
			}
		}

		public T Convert
		{
			get
			{
				return lastValue;
			}
		}
	}
}
