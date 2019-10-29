using System;

namespace RPGCore.Behaviour
{
	public interface IEventField : IDisposable
	{
		HandlerCollection Handlers { get; }
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
