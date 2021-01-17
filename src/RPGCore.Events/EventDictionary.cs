using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace RPGCore.Events
{
	[JsonObject]
	public sealed class EventDictionary<TKey, TValue> : IEventDictionary<TKey, TValue>
	{
		[JsonProperty]
		private Dictionary<TKey, TValue> Collection { get; set; }

		[JsonIgnore]
		public int Count => Collection.Count;

		[JsonIgnore]
		public EventDictionaryHandlerCollection<TKey, TValue> Handlers { get; }

		public TValue this[TKey key] => Collection[key];

		public EventDictionary()
		{
			Handlers = new EventDictionaryHandlerCollection<TKey, TValue>(this);
			Collection = new Dictionary<TKey, TValue>();
		}

		public bool ContainsKey(TKey key)
		{
			return Collection.ContainsKey(key);
		}

		public void Add(TKey key, TValue value)
		{
			Collection.Add(key, value);

			Handlers.InvokeAdd(key, value);
		}

		public bool Remove(TKey key)
		{
			if (!Collection.TryGetValue(key, out var eventObject))
			{
				return false;
			}

			bool result = Collection.Remove(key);
			if (result)
			{
				Handlers.InvokeRemoved(key, eventObject);
			}

			return result;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return Collection.TryGetValue(key, out value);
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return Collection.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Collection.GetEnumerator();
		}
	}
}
