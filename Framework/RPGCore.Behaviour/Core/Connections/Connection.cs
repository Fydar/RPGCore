using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Behaviour
{
	public abstract class Connection
	{
		public virtual int ConnectionId { get; set; }

		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		public abstract object ObjectValue { get; set; }
	}

	public struct InputCallback
	{
		public INodeInstance Node;
		public Action Callback;

		public InputCallback(INodeInstance node, Action callback)
		{
			Node = node;
			Callback = callback;
		}
	}

	public class Connection<T> : Connection
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private T GenericValue;
		
		[JsonIgnore]
		public List<InputCallback> Subscribers;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public override object ObjectValue
		{
			get
			{
				return GenericValue;
			}
			set
			{
				GenericValue = (T)value;
				InvokeAfterChanged();
			}
		}

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

		public Connection(int connectionId)
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
