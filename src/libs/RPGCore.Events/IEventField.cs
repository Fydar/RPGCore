namespace RPGCore.Events;

public interface IEventField<T> : IReadOnlyEventField<T>
{
	new T? Value { get; set; }
}
