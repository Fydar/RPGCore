using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Behaviour
{
	public class EventCollectionHandlerCollection<TKey, TValue> : IEnumerable<KeyValuePair<object, IEventCollectionHandler<TKey, TValue>>>, IDisposable
	{
		public readonly struct ContextWrapped
		{
			private readonly IEventCollection<TKey, TValue> Collection;
			private readonly object Context;

			public ContextWrapped (IEventCollection<TKey, TValue> collection, object context)
			{
				Collection = collection;
				Context = context;
			}

			public void Clear ()
			{
				Collection.Handlers.Clear (Context);
			}

			public void Add (IEventCollectionHandler<TKey, TValue> handler)
			{
				Collection.Handlers.InternalHandlers.Add (new KeyValuePair<object, IEventCollectionHandler<TKey, TValue>> (Context, handler));
			}

			public void Remove (IEventCollectionHandler<TKey, TValue> handler)
			{
				Collection.Handlers.InternalHandlers.Remove (new KeyValuePair<object, IEventCollectionHandler<TKey, TValue>> (Context, handler));
			}
		}

		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		private readonly IEventCollection<TKey, TValue> Collection;

		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		private readonly List<KeyValuePair<object, IEventCollectionHandler<TKey, TValue>>> InternalHandlers;

		public EventCollectionHandlerCollection (IEventCollection<TKey, TValue> collection)
		{
			Collection = collection;
			InternalHandlers = new List<KeyValuePair<object, IEventCollectionHandler<TKey, TValue>>> ();
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

		public void InvokeAdd (TKey key, TValue value)
		{
			if (InternalHandlers == null)
			{
				return;
			}

			for (int i = 0; i < InternalHandlers.Count; i++)
			{
				InternalHandlers[i].Value.OnAdd (key, value);
			}
		}

		public void InvokeRemoved (TKey key, TValue value)
		{
			if (InternalHandlers == null)
			{
				return;
			}

			for (int i = 0; i < InternalHandlers.Count; i++)
			{
				InternalHandlers[i].Value.OnRemove (key, value);
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

		public IEnumerator<KeyValuePair<object, IEventCollectionHandler<TKey, TValue>>> GetEnumerator ()
		{
			return ((IEnumerable<KeyValuePair<object, IEventCollectionHandler<TKey, TValue>>>)InternalHandlers).GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return ((IEnumerable<KeyValuePair<object, IEventCollectionHandler<TKey, TValue>>>)InternalHandlers).GetEnumerator ();
		}
	}
}
