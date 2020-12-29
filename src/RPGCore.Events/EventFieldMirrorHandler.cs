namespace RPGCore.Events
{
	public sealed class EventFieldMirrorHandler<T> : IEventFieldHandler
	{
		public IReadOnlyEventField<T> SourceField;
		public IEventField<T> Target;

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
}
