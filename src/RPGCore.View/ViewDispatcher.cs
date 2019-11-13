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

		private class EntityFieldSyncHandler : IEventFieldHandler
		{
			private readonly ViewDispatcher dispatcher;
			private readonly EntityRef entity;
			private readonly string path;
			private readonly IEventField field;

			public EntityFieldSyncHandler (ViewDispatcher dispatcher, EntityRef entity, string path, IEventField field)
			{
				this.dispatcher = dispatcher;
				this.entity = entity;
				this.path = path;
				this.field = field;
			}

			public void OnAfterChanged ()
			{
				object value = field.GetValue ();

				var packet = new ViewPacket
				{
					PacketType = ViewPacket.ViewPacketType.UpdateEntity,
					EntityType = value.GetType ().ToString (),
					Data = JToken.FromObject (value),
					Entity = entity,
					FieldPath = path,
				};

				dispatcher.OnPacketGenerated?.Invoke (packet);
			}

			public void OnBeforeChanged ()
			{
			}

			public void Dispose ()
			{
			}
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

				foreach (var field in value.SyncedObjects)
				{
					field.Value.Handlers[this].Add (new EntityFieldSyncHandler (dispatcher, key, field.Key, field.Value));
				}
			}

			public void OnRemove (EntityRef key, Entity value)
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
}
