namespace RPGCore.Events;

public sealed class EventFieldMirrorHandler<T> : IEventFieldHandler
{
	public IReadOnlyEventField<T> SourceField { get; }
	public IEventField<T> Target { get; }

	public EventFieldMirrorHandler(IReadOnlyEventField<T> source, IEventField<T> target)
	{
		SourceField = source;
		Target = target;
	}

	public void OnBeforeChanged()
	{
	}

	public void OnAfterChanged()
	{
		Target.Value = SourceField.Value;
	}
}
