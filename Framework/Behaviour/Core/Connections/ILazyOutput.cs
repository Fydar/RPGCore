using System;

namespace Behaviour
{
	public interface ILazyOutput<T> : IOutput<T>
	{
		Action OnRequested { get; set; }
	}
}