using System;

namespace RPGCore.Behaviour
{
	public class EventField<T> : IEventField<T>, IDisposable
	{
		public HandlerCollection<T> Handlers { get; set; }
		public Action OnBeforeChanged;
		public Action OnAfterChanged;

		private T internalValue;

		public T Value
		{
			get => internalValue;
			set
			{
				Handlers.InvokeBeforeChanged ();
				OnBeforeChanged?.Invoke ();

				internalValue = value;

				Handlers.InvokeAfterChanged ();
				OnAfterChanged?.Invoke ();
			}
		}

		public EventField ()
		{
			Handlers = new HandlerCollection<T> (this);
		}

		public void Dispose ()
		{
			Handlers.Dispose ();
		}
	}
}
