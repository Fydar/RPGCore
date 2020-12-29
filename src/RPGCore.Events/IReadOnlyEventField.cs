namespace RPGCore.Events
{
	public interface IReadOnlyEventField<T> : IEventField
	{
		T Value { get; }
	}
}
