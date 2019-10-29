using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace RPGCore.Behaviour
{
	public class BasicConnection<T> : EventConnection, IConnection<T>
	{
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		private T GenericValue;

		public virtual T Value
		{
			get => GenericValue;
			set
			{
				if (!BufferEventsInternal)
				{
					Handlers.InvokeBeforeChanged ();
					GenericValue = value;
					Handlers.InvokeAfterChanged ();

					InvokeAfterChanged ();
				}
				else
				{
					Buffer = value;
					BufferUsed = true;
				}
			}
		}

		private bool BufferUsed;
		private T Buffer;

		[JsonIgnore]
		public override bool BufferEvents
		{
			get
			{
				return BufferEventsInternal;
			}
			set
			{
				if (BufferEventsInternal && !value)
				{
					if (BufferUsed)
					{
						BufferEventsInternal = value;
						Value = Buffer;
						BufferUsed = false;
					}
				}
				BufferEventsInternal = value;
			}
		}
		private bool BufferEventsInternal;

		[JsonIgnore]
		public override Type ConnectionType => typeof (T);

		[JsonIgnore]
		public HandlerCollection Handlers { get; }

		[JsonIgnore]
		public IReadOnlyEventField<T> Mirroring { get; private set; }

		private EventFieldMirrorHandler<T> MirrorHandler;

		public BasicConnection (int connectionId)
			: base (connectionId)
		{
			Handlers = new HandlerCollection (this);
		}

		public void StartMirroring (IReadOnlyEventField<T> target)
		{
			if (Mirroring != null)
			{
				throw new InvalidOperationException ("Connection cannot mirror as it is already mirroring.");
			}

			Mirroring = target;

			Value = Mirroring.Value;
			MirrorHandler = new EventFieldMirrorHandler<T> (Mirroring, this);
			Mirroring.Handlers[this].Add (MirrorHandler);
		}

		public void StopMirroring ()
		{
			if (Mirroring == null)
			{
				throw new InvalidOperationException ("Connection cannot stop mirroring as it is currently not mirroring anything.");
			}
			Mirroring.Handlers[this].Remove (MirrorHandler);
			MirrorHandler = null;
			Mirroring = null;
		}

		public override string ToString () => $"Connection {ConnectionId}, Value = {GenericValue}";
		public void Dispose ()
		{
			Mirroring?.Dispose ();
			MirrorHandler?.Dispose ();

			Handlers.Dispose ();
		}
	}
}
