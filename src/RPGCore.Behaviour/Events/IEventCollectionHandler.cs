using System;

namespace RPGCore.Behaviour
{
	public interface IEventCollectionHandler : IDisposable
	{
		void OnAdd ();
		void OnRemove ();
	}
}
