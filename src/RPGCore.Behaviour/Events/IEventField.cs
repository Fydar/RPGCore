using System;

namespace RPGCore.Behaviour
{
	public interface IEventField : IEventWrapper, IDisposable
	{
		EventFieldHandlerCollection Handlers { get; }

		object GetValue();
		void SetValue(object value);
	}

	public interface IReadOnlyEventField<T> : IEventField
	{
		T Value { get; }
	}

	public interface IEventField<T> : IReadOnlyEventField<T>
	{
		new T Value { get; set; }
	}
}
