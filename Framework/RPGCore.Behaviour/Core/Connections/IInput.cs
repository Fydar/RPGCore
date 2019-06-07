using System;
using Newtonsoft.Json;

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
