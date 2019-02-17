using System;

namespace RPGCore.Behaviour
{
	public interface IInput<T> : ILazyInput<T>
	{
		Action OnAfterChanged { get; set; }
	}
}
