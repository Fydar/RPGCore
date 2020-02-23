using System;

namespace RPGCore.Behaviour
{
	public interface IEventCollectionHandler<TKey, TValue> : IDisposable
	{
		void OnAdd(TKey key, TValue value);

		void OnRemove(TKey key, TValue value);
	}
}
