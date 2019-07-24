using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Behaviour
{
	public class BasicConnection<T> : IConnection<T>
	{
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		private T GenericValue;

		[JsonIgnore]
		public List<ConnectionSubscription> Subscribers;

		public int ConnectionId { get; }

		public virtual void Subscribe (INodeInstance node, Action callback)
		{
			if (Subscribers == null)
				Subscribers = new List<ConnectionSubscription> ();

			var subscription = new ConnectionSubscription();
			foreach (var previousSubscribers in Subscribers)
			{
				if (subscription.Node == previousSubscribers.Node)
				{
					subscription = previousSubscribers;
				}
			}
			if (subscription.Node == null)
			{
				subscription = new ConnectionSubscription (node);
				subscription.Callbacks.Add (callback);
				Subscribers.Add (subscription);
			}
			else
			{
				subscription.Callbacks.Add (callback);
			}
		}

		public virtual void Unsubscribe (INodeInstance node, Action callback)
		{
			if (Subscribers == null)
				return;

			for (int i = Subscribers.Count - 1; i >= 0; i--)
			{
				var subscriber = Subscribers[i];

				if (subscriber.Node == node)
				{
					for (int j = subscriber.Callbacks.Count - 1; j >= 0; j--)
					{
						var findCallback = subscriber.Callbacks[j];

						if (callback == findCallback)
						{
							subscriber.Callbacks.RemoveAt (j);
						}
					}
				}
			}
		}

		public virtual T Value
		{
			get => GenericValue;
			set
			{
				GenericValue = value;
				InvokeAfterChanged ();
			}
		}

		public Type ConnectionType => typeof (T);

		public BasicConnection (int connectionId)
		{
			ConnectionId = connectionId;
		}

		protected void InvokeAfterChanged ()
		{
			if (Subscribers == null)
				return;

			foreach (var subscriber in Subscribers)
			{
				foreach (var callback in subscriber.Callbacks)
				{
					callback?.Invoke ();
				}
			}
		}

		public override string ToString () => $"Connection {ConnectionId}, Value = {GenericValue}";
	}
}
