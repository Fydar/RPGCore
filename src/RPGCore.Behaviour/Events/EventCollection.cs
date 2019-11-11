using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace RPGCore.Behaviour
{
	public class EventCollection<TKey, TValue> : IEventCollection<TKey, TValue>, IDisposable
	{
		[JsonIgnore]
		public EventCollectionHandlerCollection<TKey, TValue> Handlers { get; set; }

		private readonly Dictionary<TKey, TValue> Collection;

		public TValue this[TKey key] => Collection[key];

		public EventCollection ()
		{
			Handlers = new EventCollectionHandlerCollection<TKey, TValue> (this);
			Collection = new Dictionary<TKey, TValue> ();
		}

		public void Dispose ()
		{
			Handlers.Dispose ();
		}

		public bool ContainsKey (TKey key)
		{
			return Collection.ContainsKey (key);
		}

		public void Add (TKey key, TValue value)
		{
			Collection.Add (key, value);

			Handlers.InvokeAdd (key, value);
		}

		public bool Remove (TKey key)
		{
			if (!Collection.TryGetValue (key, out var eventObject))
			{
				return false;
			}

			bool result = Collection.Remove (key);
			if (result)
			{
				Handlers.InvokeRemoved (key, eventObject);
			}

			return result;
		}

		public bool TryGetValue (TKey key, out TValue value)
		{
			return Collection.TryGetValue (key, out value);
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator ()
		{
			return Collection.GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return Collection.GetEnumerator ();
		}
	}
}
