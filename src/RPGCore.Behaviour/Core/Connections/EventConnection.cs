using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace RPGCore.Behaviour
{
	public class EventConnection : IConnection
	{
		[JsonIgnore]
		public List<ConnectionSubscription> Consumers;

		[JsonIgnore]
		public List<IConnectionTypeConverter> Converters;

		[JsonIgnore]
		public int ConnectionId { get; }

		[JsonIgnore]
		public virtual Type ConnectionType => null;

		public EventConnection (int connectionId)
		{
			ConnectionId = connectionId;
		}

		public void RegisterInput (INodeInstance node)
		{
			if (Consumers == null)
			{
				Consumers = new List<ConnectionSubscription> ();
			}

			Consumers.Add (new ConnectionSubscription (node));
		}

		public void RegisterConverter (IConnectionTypeConverter converter)
		{
			if (Converters == null)
			{
				Converters = new List<IConnectionTypeConverter> ();
			}

			Converters.Add (converter);
		}

		public virtual void Subscribe (INodeInstance node, Action callback)
		{
			if (Consumers == null)
			{
				Consumers = new List<ConnectionSubscription> ();
			}

			var subscription = new ConnectionSubscription ();
			foreach (var previousSubscribers in Consumers)
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
				Consumers.Add (subscription);
			}
			else
			{
				subscription.Callbacks.Add (callback);
			}
		}

		public virtual void Unsubscribe (INodeInstance node, Action callback)
		{
			if (Consumers == null)
			{
				return;
			}

			for (int i = Consumers.Count - 1; i >= 0; i--)
			{
				var subscriber = Consumers[i];

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

		protected void InvokeAfterChanged ()
		{
			if (Consumers == null)
			{
				return;
			}

			foreach (var subscriber in Consumers)
			{
				subscriber.Node.OnInputChanged ();

				foreach (var callback in subscriber.Callbacks)
				{
					callback?.Invoke ();
				}
			}
		}

		public override string ToString () => $"Connection {ConnectionId}";
	}
}
