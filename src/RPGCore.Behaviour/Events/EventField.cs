using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace RPGCore.Behaviour
{
	public class EventField<T> : IEventField<T>
	{
		[JsonIgnore]
		public EventFieldHandlerCollection Handlers { get; set; }

		[JsonIgnore]
		public Action OnBeforeChanged;

		[JsonIgnore]
		public Action OnAfterChanged;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private T internalValue;

		public T Value
		{
			get => internalValue;
			set
			{
				Handlers.InvokeBeforeChanged();
				OnBeforeChanged?.Invoke();

				internalValue = value;

				Handlers.InvokeAfterChanged();
				OnAfterChanged?.Invoke();
			}
		}

		public EventField()
		{
			Handlers = new EventFieldHandlerCollection(this);
		}

		public EventField(T value)
			: this()
		{
			internalValue = value;
		}
	}
}
