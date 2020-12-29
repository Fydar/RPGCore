using System;

namespace RPGCore.Events
{
	public interface IEventCollectionHandler<TKey, TValue> : IDisposable
	{
		void OnAdd(TKey key, TValue value);

		void OnRemove(TKey key, TValue value);
	}
}
