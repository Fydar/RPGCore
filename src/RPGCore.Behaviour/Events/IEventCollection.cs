using System.Collections.Generic;

namespace RPGCore.Behaviour
{
	public interface IEventCollection<TKey, TValue> : IEventWrapper, IEnumerable<KeyValuePair<TKey, TValue>>
	{
		EventCollectionHandlerCollection<TKey, TValue> Handlers { get; }
		TValue this[TKey key] { get; }

		bool ContainsKey(TKey key);

		void Add(TKey key, TValue value);

		bool Remove(TKey key);

		bool TryGetValue(TKey key, out TValue value);
	}
}
