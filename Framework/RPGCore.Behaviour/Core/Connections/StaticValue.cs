using System;

namespace RPGCore.Behaviour
{
	public class StaticValue<T> : IInput<T>
	{
		public event Action OnAfterChanged;

		private readonly T internalValue;

		public StaticValue (T value)
		{
			internalValue = value;
		}

		public T Value => internalValue;
	}
}
