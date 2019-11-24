using Newtonsoft.Json.Linq;
using RPGCore.Behaviour;

namespace RPGCore.View
{
	public class SyncEventCollectionHandler<TKey, TValue> : IEventCollectionHandler<TKey, TValue>
	{
		private readonly ViewDispatcher dispatcher;
		private readonly EntityRef parent;
		private readonly string path;

		public SyncEventCollectionHandler(ViewDispatcher dispatcher, EntityRef parent, string path)
		{
			this.dispatcher = dispatcher;
			this.parent = parent;
			this.path = path;
		}

		public void Dispose()
		{
		}

		public void OnAdd(TKey key, TValue value)
		{
			var packet = new ViewPacket
			{
				PacketType = ViewPacket.ViewPacketType.AddCollectionItem,
				Data = JObject.FromObject (value),
				Entity = parent,
				FieldPath = $"{path}.{key}"
			};

			dispatcher.OnPacketGenerated?.Invoke (packet);

			if (value is ISyncField wrapper)
			{
				wrapper.AddSyncHandler (dispatcher, parent, $"{path}.{key}");
			}
		}

		public void OnRemove(TKey key, TValue value)
		{
			var packet = new ViewPacket
			{
				PacketType = ViewPacket.ViewPacketType.RemoveCollectionItem,
				Entity = parent,
				FieldPath = $"{path}.{key}"
			};

			dispatcher.OnPacketGenerated?.Invoke (packet);
		}
	}
}
