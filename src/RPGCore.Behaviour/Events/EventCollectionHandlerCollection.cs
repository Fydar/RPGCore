using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Behaviour
{
	public class EventCollectionHandlerCollection : IEnumerable<KeyValuePair<object, IEventCollectionHandler>>, IDisposable
	{
		public readonly struct ContextWrapped
		{
			private readonly IEventCollection Collection;
			private readonly object Context;

			public ContextWrapped (IEventCollection collection, object context)
			{
				Collection = collection;
				Context = context;
			}

			public void Clear ()
			{
				Collection.Handlers.Clear (Context);
			}

			public void Add (IEventCollectionHandler handler)
			{
				Collection.Handlers.InternalHandlers.Add (new KeyValuePair<object, IEventCollectionHandler> (Context, handler));
			}

			public void Remove (IEventCollectionHandler handler)
			{
				Collection.Handlers.InternalHandlers.Remove (new KeyValuePair<object, IEventCollectionHandler> (Context, handler));
			}
		}

		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		private readonly IEventCollection Collection;

		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		private readonly List<KeyValuePair<object, IEventCollectionHandler>> InternalHandlers;

		public EventCollectionHandlerCollection (IEventCollection collection)
		{
			Collection = collection;
			InternalHandlers = new List<KeyValuePair<object, IEventCollectionHandler>> ();
		}

		public ContextWrapped this[object context]
		{
			get => new ContextWrapped (Collection, context);
		}

		public void Clear (object context)
		{
			for (int i = InternalHandlers.Count - 1; i >= 0; i--)
			{
				if (InternalHandlers[i].Key == context)
				{
					InternalHandlers.RemoveAt (i);
				}
			}
		}

		public void Clear ()
		{
			InternalHandlers.Clear ();
		}

		public void InvokeAdd ()
		{
			if (InternalHandlers == null)
			{
				return;
			}

			for (int i = 0; i < InternalHandlers.Count; i++)
			{
				InternalHandlers[i].Value.OnAdd ();
			}
		}

		public void InvokeRemoved ()
		{
			if (InternalHandlers == null)
			{
				return;
			}

			for (int i = 0; i < InternalHandlers.Count; i++)
			{
				InternalHandlers[i].Value.OnRemove ();
			}
		}

		public void Dispose ()
		{
			if (InternalHandlers == null)
			{
				return;
			}

			for (int i = 0; i < InternalHandlers.Count; i++)
			{
				InternalHandlers[i].Value.Dispose ();
			}
		}

		public IEnumerator<KeyValuePair<object, IEventCollectionHandler>> GetEnumerator ()
		{
			return ((IEnumerable<KeyValuePair<object, IEventCollectionHandler>>)InternalHandlers).GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return ((IEnumerable<KeyValuePair<object, IEventCollectionHandler>>)InternalHandlers).GetEnumerator ();
		}
	}
}
