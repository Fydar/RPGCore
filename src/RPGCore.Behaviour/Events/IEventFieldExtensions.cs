using System;

namespace RPGCore.Behaviour
{
	public static class IEventFieldExtensions
	{
		public static EventField<B> Watch<T, B> (this IEventField<T> field, Func<T, IEventField<B>> chain)
		{
			var watcher = new EventField<B> ();
			field.Handlers[watcher].Add (new EventFieldChainHandler<T, B> (field, watcher, chain));
			return watcher;
		}
	}
}
