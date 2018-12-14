using System;

namespace Behaviour
{
	public interface IInput<T> : ILazyInput<T>
	{
		Action OnAfterChanged { get; set; }
	}
}
