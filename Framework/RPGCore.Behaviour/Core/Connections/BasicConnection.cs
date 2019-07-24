using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Behaviour
{
	public class BasicConnection<T> : IConnection<T>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private T GenericValue;
		
		[JsonIgnore]
		public List<InputCallback> Subscribers;

		public int ConnectionId { get; }

		public virtual void Subscribe (INodeInstance node, Action callback)
		{
			if (Subscribers == null)
				Subscribers = new List<InputCallback> ();

			Subscribers.Add (new InputCallback (node, callback));
		}

		public virtual void Unsubscribe (INodeInstance node, Action callback)
		{
			if (Subscribers == null)
				return;

			for (int i = Subscribers.Count - 1; i >= 0; i--)
			{
				var subscriber = Subscribers[i];

				if (subscriber.Node == node
					&& subscriber.Callback == callback)
				{
					Subscribers.RemoveAt (i);
				}
			}
		}

		public virtual T Value
		{
			get => GenericValue;
			set
			{
				GenericValue = value;
				InvokeAfterChanged();
			}
		}

		public Type ConnectionType => typeof (T);

		public BasicConnection(int connectionId)
		{
			ConnectionId = connectionId;
		}

		protected void InvokeAfterChanged ()
		{
			if (Subscribers == null)
				return;

			foreach (var subscriber in Subscribers)
			{
				subscriber.Callback?.Invoke ();
			}
		}

		public override string ToString() => $"Connection {ConnectionId}, Value = {GenericValue}";
	}
}
