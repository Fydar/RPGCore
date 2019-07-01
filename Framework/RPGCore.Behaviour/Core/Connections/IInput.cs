using System;

namespace RPGCore.Behaviour
{
	public interface IInput
	{
	}

	public interface IInput<T> : ILazyInput<T>, IInput
	{
		event Action OnAfterChanged;
	}
}
