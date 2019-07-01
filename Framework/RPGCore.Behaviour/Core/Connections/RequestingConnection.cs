using System;

namespace RPGCore.Behaviour
{
	public class RequestingConnection<T> : IInput<T>, IOutput<T>, ILazyOutput<T>
	{
		public event Action OnAfterChanged;
		public event Action OnRequested;

		private T internalValue;

		public T Value
		{
			get
			{
				if (OnRequested != null)
				{
					OnRequested ();
				}

				return internalValue;
			}
			set => internalValue = value;
		}
	}
}