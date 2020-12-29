using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Events
{
	public class EventCollectionHandlerCollection<TKey, TValue> : IEnumerable<KeyValuePair<object, IEventCollectionHandler<TKey, TValue>>>
	{
		public readonly ref struct ContextWrapped
		{
			private readonly IEventCollection<TKey, TValue> collection;
			private readonly object context;

			public ContextWrapped(IEventCollection<TKey, TValue> collection, object context)
			{
				this.collection = collection;
				this.context = context;
			}

			public void Clear()
			{
				collection.Handlers.Clear(context);
			}

			public void Add(IEventCollectionHandler<TKey, TValue> handler)
			{
				collection.Handlers.internalHandlers.Add(new KeyValuePair<object, IEventCollectionHandler<TKey, TValue>>(context, handler));
			}

			public void Remove(IEventCollectionHandler<TKey, TValue> handler)
			{
				collection.Handlers.internalHandlers.Remove(new KeyValuePair<object, IEventCollectionHandler<TKey, TValue>>(context, handler));
			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly IEventCollection<TKey, TValue> collection;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly List<KeyValuePair<object, IEventCollectionHandler<TKey, TValue>>> internalHandlers;

		public EventCollectionHandlerCollection(IEventCollection<TKey, TValue> collection)
		{
			this.collection = collection;
			internalHandlers = new List<KeyValuePair<object, IEventCollectionHandler<TKey, TValue>>>();
		}

		public ContextWrapped this[object context]
		{
			get => new ContextWrapped(collection, context);
		}

		public void Clear(object context)
		{
			for (int i = internalHandlers.Count - 1; i >= 0; i--)
			{
				if (internalHandlers[i].Key == context)
				{
					internalHandlers.RemoveAt(i);
				}
			}
		}

		public void Clear()
		{
			internalHandlers.Clear();
		}

		public void InvokeAdd(TKey key, TValue value)
		{
			if (internalHandlers == null)
			{
				return;
			}

			for (int i = 0; i < internalHandlers.Count; i++)
			{
				internalHandlers[i].Value.OnAdd(key, value);
			}
		}

		public void InvokeRemoved(TKey key, TValue value)
		{
			if (internalHandlers == null)
			{
				return;
			}

			for (int i = 0; i < internalHandlers.Count; i++)
			{
				internalHandlers[i].Value.OnRemove(key, value);
			}
		}

		public IEnumerator<KeyValuePair<object, IEventCollectionHandler<TKey, TValue>>> GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<object, IEventCollectionHandler<TKey, TValue>>>)internalHandlers).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<object, IEventCollectionHandler<TKey, TValue>>>)internalHandlers).GetEnumerator();
		}
	}
}
