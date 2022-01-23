using System;

namespace RPGCore.Events.Chaining;

public static class IEventFieldChainingExtensions
{
	public static IReadOnlyEventField<B> Watch<T, B>(this IReadOnlyEventField<T> field, Func<T, IReadOnlyEventField<B>> targetSelector)
	{
		var watcher = new EventField<B>();
		field.Handlers[watcher].Add(new EventFieldChainHandler<T, B>(field, watcher, targetSelector));
		return watcher;
	}
}
