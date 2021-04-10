namespace RPGCore.Events
{
	public interface IReadOnlyEventField : IEventWrapper
	{
		EventFieldHandlerCollection Handlers { get; }
	}

	public interface IReadOnlyEventField<T> : IReadOnlyEventField
	{
		T? Value { get; }
	}
}
