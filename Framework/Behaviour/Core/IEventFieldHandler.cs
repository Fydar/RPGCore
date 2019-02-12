using System;

namespace Behaviour
{
    public interface IEventFieldHandler : IDisposable
	{
		void OnBeforeChanged();
		void OnAfterChanged();
	}
}
