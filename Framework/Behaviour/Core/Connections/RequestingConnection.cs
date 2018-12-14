using System;

namespace Behaviour
{
	public class RequestingConnection<T> : IInput<T>, IOutput<T>, ILazyOutput<T>
	{
		private Action onAfterChanged;
		private T internalValue;

		public T Value
		{
			get
			{
				if (onAfterChanged != null)
					onAfterChanged();

				return internalValue;
			}
			set
			{
				internalValue = value;
			}
		}

		public Action OnAfterChanged
		{
			get
			{
				return onAfterChanged;
			}
			set
			{
				onAfterChanged = value;
			}
		}

		public Action OnRequested
		{
			get
			{
				return onAfterChanged;
			}
			set
			{
				onAfterChanged = value;
			}
		}
	}
}