using Newtonsoft.Json;
using RPGCore.Events;
using System;
using System.Diagnostics;

namespace RPGCore.Behaviour
{
	public class OutputConnection<T> : EventConnection, IConnection<T>, IEventField<T>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private T valueInternal;

		public virtual T Value
		{
			get => valueInternal;
			set
			{
				if (!bufferEvents)
				{
					Handlers.InvokeBeforeChanged();
					valueInternal = value;
					Handlers.InvokeAfterChanged();

					InvokeAfterChanged();
				}
				else
				{
					buffer = value;
					bufferUsed = true;
				}
			}
		}

		private bool bufferUsed;
		private T buffer;

		[JsonIgnore]
		public override bool BufferEvents
		{
			get
			{
				return bufferEvents;
			}
			set
			{
				if (bufferEvents && !value)
				{
					if (bufferUsed)
					{
						bufferEvents = value;
						Value = buffer;
						bufferUsed = false;
					}
				}
				bufferEvents = value;
			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool bufferEvents;

		[JsonIgnore]
		public override Type ConnectionType => typeof(T);

		[JsonIgnore]
		public EventFieldHandlerCollection Handlers { get; }

		[JsonIgnore]
		public IReadOnlyEventField<T> Mirroring { get; private set; }

		private EventFieldMirrorHandler<T> mirrorHandler;

		public OutputConnection(int connectionId)
			: base(connectionId)
		{
			Handlers = new EventFieldHandlerCollection(this);
		}

		public void StartMirroring(IReadOnlyEventField<T> target)
		{
			if (Mirroring != null)
			{
				throw new InvalidOperationException("Connection cannot mirror as it is already mirroring.");
			}

			Mirroring = target;

			Value = Mirroring.Value;
			mirrorHandler = new EventFieldMirrorHandler<T>(Mirroring, this);
			Mirroring.Handlers[this].Add(mirrorHandler);
		}

		public void StopMirroring()
		{
			if (Mirroring == null)
			{
				throw new InvalidOperationException("Connection cannot stop mirroring as it is currently not mirroring anything.");
			}
			Mirroring.Handlers[this].Remove(mirrorHandler);
			mirrorHandler = null;
			Mirroring = null;
		}

		public override string ToString() => $"Connection {ConnectionId}, Value = {valueInternal}";

		public object GetValue()
		{
			return Value;
		}

		public void SetValue(object value)
		{
			Value = (T)value;
		}
	}
}
