using System;

namespace RPGCore.Events
{
	public static class IEventFieldExtensions
	{
		public static IReadOnlyEventField<B> Watch<T, B>(this IReadOnlyEventField<T> field, Func<T, IReadOnlyEventField<B>> chain)
		{
			var watcher = new EventField<B>();
			field.Handlers[watcher].Add(new EventFieldChainHandler<T, B>(field, watcher, chain));
			return watcher;
		}
	}
}
