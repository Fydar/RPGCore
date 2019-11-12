using Newtonsoft.Json.Linq;
using RPGCore.Behaviour;
using System;

namespace RPGCore.View
{
	public class ViewDispatcher
	{
		public event Action<ViewPacket> OnPacketGenerated;

		public GameView View { get; }

		public ViewDispatcher (GameView view)
		{
			View = view;

			View.Entities.Handlers[this].Add (new EntitySyncCollectionHandler (this));
		}

		private class EntitySyncCollectionHandler : IEventCollectionHandler<EntityRef, Entity>
		{
			private readonly ViewDispatcher dispatcher;

			public EntitySyncCollectionHandler (ViewDispatcher dispatcher)
			{
				this.dispatcher = dispatcher;
			}

			public void Dispose ()
			{
			}

			public void OnAdd (EntityRef key, Entity value)
			{
				var packet = new ViewPacket
				{
					PacketType = ViewPacket.ViewPacketType.CreateEntity,
					EntityType = value.GetType ().ToString (),
					Data = JObject.FromObject (value),
					Entity = key,
				};

				dispatcher.OnPacketGenerated?.Invoke (packet);
			}

			public void OnRemove (EntityRef key, Entity value)
			{
				var packet = new ViewPacket
				{
					PacketType = ViewPacket.ViewPacketType.CreateEntity,
					EntityType = value.GetType ().ToString (),
					Entity = key
				};

				dispatcher.OnPacketGenerated?.Invoke (packet);
			}
		}
	}
}
