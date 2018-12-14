using System;

namespace Behaviour
{

	public class Connection<T> : IInput<T>, IOutput<T>, ILazyOutput<T>
	{
		private Action onAfterChanged;
		private T internalValue;

		public T Value
		{
			get
			{
				return internalValue;
			}
			set
			{
				internalValue = value;

				if (onAfterChanged != null)
					onAfterChanged();
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