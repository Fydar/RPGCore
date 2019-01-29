using System;

namespace Behaviour
{
	public class EventField<T>
	{
		public Action OnAfterChanged;

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

				if (OnAfterChanged != null)
					OnAfterChanged();
			}
		}
	}
}
