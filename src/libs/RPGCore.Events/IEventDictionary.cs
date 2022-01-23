using System.Collections.Generic;

namespace RPGCore.Events;

public interface IEventDictionary<TKey, TValue> : IEventWrapper, IEnumerable<KeyValuePair<TKey, TValue>>
{
	int Count { get; }
	EventDictionaryHandlerCollection<TKey, TValue> Handlers { get; }

	TValue this[TKey key] { get; }

	bool ContainsKey(TKey key);

	void Add(TKey key, TValue value);

	bool Remove(TKey key);

	bool TryGetValue(TKey key, out TValue value);
}
