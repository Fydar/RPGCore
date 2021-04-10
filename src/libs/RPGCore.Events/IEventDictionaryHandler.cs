using System;

namespace RPGCore.Events
{
	public interface IEventDictionaryHandler<TKey, TValue> : IDisposable
	{
		void OnAdd(TKey key, TValue value);

		void OnRemove(TKey key, TValue value);
	}
}
