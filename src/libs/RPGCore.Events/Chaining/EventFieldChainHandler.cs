using System;

namespace RPGCore.Events
{
	public sealed class EventFieldChainHandler<T, B> : IEventFieldHandler
	{
		public IReadOnlyEventField<T> SourceField { get; }
		public IEventField<B> TargetField { get; }
		public Func<T, IReadOnlyEventField<B>> Chain { get; }

		private IReadOnlyEventField<B> chainedField;

		public EventFieldChainHandler(IReadOnlyEventField<T> source, IEventField<B> target, Func<T, IReadOnlyEventField<B>> chain)
		{
			SourceField = source;
			TargetField = target;
			Chain = chain;

			chainedField = Chain(SourceField.Value);
		}

		public void OnBeforeChanged()
		{
			if (chainedField == null)
			{
				return;
			}

			chainedField.Handlers[this].Clear();
		}

		public void OnAfterChanged()
		{
			chainedField = Chain(SourceField.Value);
			if (chainedField == null)
			{
				TargetField.Value = default;
				return;
			}

			chainedField.Handlers[this].Add(new EventFieldMirrorHandler<B>(chainedField, TargetField));
			TargetField.Value = chainedField.Value;
		}
	}
}
