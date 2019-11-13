using System;

namespace RPGCore.Behaviour
{
	public interface IEventFieldHandler : IDisposable
	{
		void OnBeforeChanged();
		void OnAfterChanged();
	}
}
