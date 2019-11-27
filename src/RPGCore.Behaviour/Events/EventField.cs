using Newtonsoft.Json;
using RPGCore.View;
using System;
using System.Diagnostics;

namespace RPGCore.Behaviour
{
	public class EventField<T> : IEventField<T>, IDisposable, ISyncField
	{
		[JsonIgnore]
		public EventFieldHandlerCollection Handlers { get; set; }
		[JsonIgnore]
		public Action OnBeforeChanged;
		[JsonIgnore]
		public Action OnAfterChanged;

		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		private T InternalValue;

		public T Value
		{
			get => InternalValue;
			set
			{
				Handlers.InvokeBeforeChanged ();
				OnBeforeChanged?.Invoke ();

				InternalValue = value;

				Handlers.InvokeAfterChanged ();
				OnAfterChanged?.Invoke ();
			}
		}

		public EventField()
		{
			Handlers = new EventFieldHandlerCollection (this);
		}

		public EventField(T value)
			: this ()
		{
			InternalValue = value;
		}

		public void Dispose()
		{
			Handlers.Dispose ();
		}

		object IEventField.GetValue()
		{
			return Value;
		}

		void IEventField.SetValue(object value)
		{
			Value = (T)value;
		}

		public object AddSyncHandler(ViewDispatcher viewDispatcher, EntityRef root, string path)
		{
			var handler = new SyncEventFieldHandler (viewDispatcher, root, path, this);
			Handlers[this].Add (handler);
			return handler;
		}

		public void Apply(ViewPacket packet)
		{
			Value = packet.Data.ToObject<T> ();
		}
	}
}
