using Newtonsoft.Json;
using System.Diagnostics;

namespace RPGCore.Events
{
	public sealed class EventField<T> : IEventField<T>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private T internalValue;

		[JsonIgnore]
		public EventFieldHandlerCollection Handlers { get; }

		public T Value
		{
			get => internalValue;
			set
			{
				Handlers.InvokeBeforeChanged();
				internalValue = value;
				Handlers.InvokeAfterChanged();
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
