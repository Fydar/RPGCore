namespace RPGCore.Behaviour
{
	public class EventFieldMirrorHandler<T> : IEventFieldHandler
	{
		public EventField<T> SourceField;
		public EventField<T> Target;

		public EventFieldMirrorHandler (EventField<T> source, EventField<T> target)
		{
			SourceField = source;
			Target = target;
		}

		public void OnBeforeChanged ()
		{
		}

		public void OnAfterChanged ()
		{
			Target.Value = SourceField.Value;
		}

		public void Dispose ()
		{

		}
	}
}
