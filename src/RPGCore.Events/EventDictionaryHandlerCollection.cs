using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Events
{
	public sealed class EventDictionaryHandlerCollection<TKey, TValue> : IEnumerable<KeyValuePair<object, IEventDictionaryHandler<TKey, TValue>>>
	{
		public readonly ref struct ContextWrapped
		{
			private readonly IEventDictionary<TKey, TValue> collection;
			private readonly object context;

			public ContextWrapped(IEventDictionary<TKey, TValue> collection, object context)
			{
				this.collection = collection;
				this.context = context;
			}

			public void Clear()
			{
				collection.Handlers.Clear(context);
			}

			public void Add(IEventDictionaryHandler<TKey, TValue> handler)
			{
				collection.Handlers.internalHandlers.Add(new KeyValuePair<object, IEventDictionaryHandler<TKey, TValue>>(context, handler));
			}

			public void Remove(IEventDictionaryHandler<TKey, TValue> handler)
			{
				collection.Handlers.internalHandlers.Remove(new KeyValuePair<object, IEventDictionaryHandler<TKey, TValue>>(context, handler));
			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly IEventDictionary<TKey, TValue> collection;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly List<KeyValuePair<object, IEventDictionaryHandler<TKey, TValue>>> internalHandlers;

		public ContextWrapped this[object context]
		{
			get => new ContextWrapped(collection, context);
		}

		public EventDictionaryHandlerCollection(IEventDictionary<TKey, TValue> collection)
		{
			this.collection = collection;
			internalHandlers = new List<KeyValuePair<object, IEventDictionaryHandler<TKey, TValue>>>();
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
			for (int i = 0; i < internalHandlers.Count; i++)
			{
				internalHandlers[i].Value.OnAdd(key, value);
			}
		}

		public void InvokeRemoved(TKey key, TValue value)
		{
			for (int i = 0; i < internalHandlers.Count; i++)
			{
				internalHandlers[i].Value.OnRemove(key, value);
			}
		}

		public IEnumerator<KeyValuePair<object, IEventDictionaryHandler<TKey, TValue>>> GetEnumerator()
		{
			return internalHandlers.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return internalHandlers.GetEnumerator();
		}
	}
}
