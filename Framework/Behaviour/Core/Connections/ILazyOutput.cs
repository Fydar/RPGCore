using System;

namespace RPGCore.Behaviour
{
	public interface ILazyOutput<T> : IOutput<T>
	{
		Action OnRequested { get; set; }
	}
}