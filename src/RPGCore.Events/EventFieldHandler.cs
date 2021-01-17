using System;

namespace RPGCore.Events
{
	public class EventFieldHandler<TValue> : IEventFieldHandler, IHandlerUsedCallback
	{
		private IReadOnlyEventField<TValue> eventField;

		protected virtual void OnBeforeChanged(TValue oldValue)
		{
		}

		protected virtual void OnAfterChanged(TValue newValue)
		{
		}

		void IEventFieldHandler.OnBeforeChanged()
		{
			OnBeforeChanged(eventField.Value);
		}

		void IEventFieldHandler.OnAfterChanged()
		{
			OnAfterChanged(eventField.Value);
		}

		void IHandlerUsedCallback.OnUse(IReadOnlyEventField field)
		{
			if (field == null)
			{
				throw new ArgumentNullException(nameof(field), $"{nameof(field)} is null");
			}
			if (eventField != null)
			{
				throw new InvalidOperationException($"Event handler of type {GetType()} cannot be used on multiple EventFields.");
			}

			if (field is IReadOnlyEventField<TValue> castedField)
			{
				eventField = castedField;
			}
			else
			{
				throw new InvalidOperationException($"Event handler for type {typeof(TValue)} cannot be used on event field of type {field.GetType()}.");
			}
		}
	}
}
