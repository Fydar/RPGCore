namespace RPGCore.Behaviour
{
	public interface IEventField
	{
		HandlerCollection Handlers { get; }
	}

	public interface IEventField<T> : IEventField
	{
		T Value { get; }
	}
}
