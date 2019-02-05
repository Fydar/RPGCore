using System;

namespace Behaviour
{
	public class EventField<T>
	{
		public Action OnBeforeChanged;
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
				if (OnBeforeChanged != null)
					OnBeforeChanged();

				internalValue = value;

				if (OnAfterChanged != null)
					OnAfterChanged();
			}
		}

		public EventField<B> Watch<B>(Func<T, EventField<B>> chain)
		{
			var watcher = new EventField<B>();
			OnAfterChanged += () => {
				var target = chain (Value);
				if(target == null)
					return;
					
				target.OnAfterChanged += () => {
					watcher.Value = target.Value;
				};
				watcher.Value = target.Value;
			};
			return watcher;
		}
	}
}
