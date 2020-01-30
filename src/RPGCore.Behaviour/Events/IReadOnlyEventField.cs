namespace RPGCore.Behaviour
{
	public interface IReadOnlyEventField<T> : IEventField
	{
		T Value { get; }
	}
}
