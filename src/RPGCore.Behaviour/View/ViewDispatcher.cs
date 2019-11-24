using System;

namespace RPGCore.View
{
	public class ViewDispatcher
	{
		public Action<ViewPacket> OnPacketGenerated;

		public GameView View { get; }

		public ViewDispatcher(GameView view)
		{
			View = view;

			View.Entities.Handlers[this].Add (new EntitySyncCollectionHandler (this));
		}
	}
}
