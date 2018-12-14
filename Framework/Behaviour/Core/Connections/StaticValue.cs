using System;

namespace Behaviour
{
	public class StaticValue<T> : IInput<T>
	{
		private event Action onAfterChanged;
		private readonly T internalValue;

		public StaticValue(T value)
		{
			internalValue = value;
		}

		public T Value
		{
			get
			{
				return internalValue;
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
	}
}