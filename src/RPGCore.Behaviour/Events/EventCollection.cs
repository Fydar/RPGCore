using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace RPGCore.Behaviour
{
	public class EventCollection<TKey, TValue> : IEventCollection<TKey, TValue>, IDisposable
	{
		[JsonIgnore]
		public EventCollectionHandlerCollection Handlers { get; set; }

		private readonly Dictionary<TKey, TValue> Collection;

		public TValue this[TKey key] => Collection[key];

		public EventCollection ()
		{
			Handlers = new EventCollectionHandlerCollection (this);
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

			Handlers.InvokeAdd ();
		}

		public bool Remove (TKey key)
		{
			bool result = Collection.Remove (key);
			
			if (result)
			{
				Handlers.InvokeRemoved ();
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
