using System;

namespace RPGCore.Behaviour
{
	public sealed class StaticValue<T>
	{
		public event Action OnAfterChanged;

		private readonly T internalValue;

		public StaticValue(T value)
		{
			internalValue = value;
		}

		public T Value => internalValue;
	}
}
