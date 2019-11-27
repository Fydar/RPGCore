using Newtonsoft.Json.Linq;
using RPGCore.Behaviour;

namespace RPGCore.View
{
	internal class EntitySyncCollectionHandler : IEventCollectionHandler<EntityRef, Entity>
	{
		private readonly ViewDispatcher dispatcher;

		public EntitySyncCollectionHandler(ViewDispatcher dispatcher)
		{
			this.dispatcher = dispatcher;
		}

		public void Dispose()
		{
		}

		public void OnAdd(EntityRef key, Entity value)
		{
			var packet = new ViewPacket
			{
				PacketType = ViewPacket.ViewPacketType.CreateEntity,
				EntityType = value.GetType ().ToString (),
				Data = JObject.FromObject (value),
				Entity = key,
			};

			dispatcher.OnPacketGenerated?.Invoke (packet);

			foreach (var eventWrapper in value.SyncedObjects)
			{
				if (eventWrapper.Value == null)
				{
					continue;
				}
				eventWrapper.Value.AddSyncHandler (dispatcher, key, eventWrapper.Key);

				if (eventWrapper.Value is IEventCollection<string, ISyncField> withChildren)
				{
					foreach (var child in withChildren)
					{
						if (child.Value == null)
						{
							continue;
						}
						child.Value.AddSyncHandler (dispatcher, key, eventWrapper.Key + "." + child.Key);
					}
				}
			}
		}

		public void OnRemove(EntityRef key, Entity value)
		{
			var packet = new ViewPacket
			{
				PacketType = ViewPacket.ViewPacketType.DestroyEntity,
				EntityType = value.GetType ().ToString (),
				Entity = key
			};

			dispatcher.OnPacketGenerated?.Invoke (packet);
		}
	}
}
