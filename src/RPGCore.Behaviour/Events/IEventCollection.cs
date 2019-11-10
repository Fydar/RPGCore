using System;
using System.Collections.Generic;

namespace RPGCore.Behaviour
{
	public interface IEventCollection : IDisposable
	{
		EventCollectionHandlerCollection Handlers { get; }
	}

	public interface IEventCollection<TKey, TValue> : IEventCollection, IEnumerable<KeyValuePair<TKey, TValue>>
	{
		TValue this[TKey key] { get; }

		bool ContainsKey (TKey key);
		void Add (TKey key, TValue value);
		bool Remove (TKey key);
		bool TryGetValue (TKey key, out TValue value);
	}
}
