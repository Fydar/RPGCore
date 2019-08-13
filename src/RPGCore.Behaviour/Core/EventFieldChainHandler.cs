using System;

namespace RPGCore.Behaviour
{
	public sealed class EventFieldChainHandler<T, B> : IEventFieldHandler
	{
		public EventField<T> SourceField;
		public EventField<B> TargetField;
		public Func<T, EventField<B>> Chain;

		private EventField<B> ChainedField;

		public EventFieldChainHandler (EventField<T> source, EventField<B> target, Func<T, EventField<B>> chain)
		{
			SourceField = source;
			TargetField = target;
			Chain = chain;

			ChainedField = Chain (SourceField.Value);
		}

		public void OnBeforeChanged ()
		{
			if (ChainedField == null)
			{
				return;
			}

			ChainedField.Handlers[this].Clear ();
		}

		public void OnAfterChanged ()
		{
			ChainedField = Chain (SourceField.Value);
			if (ChainedField == null)
			{
				TargetField.Value = default (B);
				return;
			}

			ChainedField.Handlers[this] += new EventFieldMirrorHandler<B> (ChainedField, TargetField);
			TargetField.Value = ChainedField.Value;
		}

		public void Dispose ()
		{
			SourceField.Handlers[TargetField].Clear ();

			if (ChainedField == null)
			{
				return;
			}

			ChainedField.Handlers[this].Clear ();
		}
	}
}
