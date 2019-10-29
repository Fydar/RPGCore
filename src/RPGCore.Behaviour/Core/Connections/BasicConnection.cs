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
				Handlers.InvokeBeforeChanged ();
				GenericValue = value;
				Handlers.InvokeAfterChanged ();

				InvokeAfterChanged ();
			}
		}

		public override Type ConnectionType => typeof (T);

		public HandlerCollection Handlers { get; }

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
