using System;

namespace RPGCore.Behaviour
{
	public interface IInput<T> : ILazyInput<T>
	{
		event Action OnAfterChanged;
	}
}
