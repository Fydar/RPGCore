using System;

namespace RPGCore.Behaviour
{
	public sealed class EventFieldChainHandler<T, B> : IEventFieldHandler
	{
		public IReadOnlyEventField<T> SourceField;
		public IEventField<B> TargetField;
		public Func<T, IReadOnlyEventField<B>> Chain;

		private IReadOnlyEventField<B> ChainedField;

		public EventFieldChainHandler (IReadOnlyEventField<T> source, IEventField<B> target, Func<T, IReadOnlyEventField<B>> chain)
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

			ChainedField.Handlers[this].Add(new EventFieldMirrorHandler<B> (ChainedField, TargetField));
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
