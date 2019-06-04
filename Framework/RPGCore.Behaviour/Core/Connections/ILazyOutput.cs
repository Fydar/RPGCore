using System;

namespace RPGCore.Behaviour
{
	public interface ILazyOutput<T> : IOutput<T>
	{
		event Action OnRequested;
	}
}