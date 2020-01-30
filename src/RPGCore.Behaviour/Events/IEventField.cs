namespace RPGCore.Behaviour
{
	public interface IEventField : IEventWrapper
	{
		EventFieldHandlerCollection Handlers { get; }
	}

	public interface IEventField<T> : IReadOnlyEventField<T>
	{
		new T Value { get; set; }
	}
}
