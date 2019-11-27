using RPGCore.Behaviour;
using System;
using System.Linq;

namespace RPGCore.View
{
	public class GameView
	{
		public EventCollection<EntityRef, Entity> Entities = new EventCollection<EntityRef, Entity> ();

		public void Apply(ViewPacket packet)
		{
			Type entityType = null;
			if (!string.IsNullOrEmpty (packet.EntityType))
			{
				foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies ())
				{
					entityType = assembly.GetType (packet.EntityType);
					if (entityType != null)
					{
						break;
					}
				}
			}

			switch (packet.PacketType)
			{
				case ViewPacket.ViewPacketType.CreateEntity:

					var clonedEntity = (Entity)packet.Data.ToObject (entityType);
					clonedEntity.Id = packet.Entity;
					Entities.Add (clonedEntity.Id, clonedEntity);
					break;

				case ViewPacket.ViewPacketType.DestroyEntity:
					Entities.Remove (packet.Entity);
					break;

				case ViewPacket.ViewPacketType.SetFieldValue:

					var targetEntity = Entities[packet.Entity];

					string[] path = packet.FieldPath.Split ('.');

					var targetedEventWrapper = targetEntity.SyncedObjects.Where (s => s.Key == path[0]).First ().Value;

					for (int i = 1; i < path.Length; i++)
					{
						if (targetedEventWrapper is IEventCollection<string, ISyncField> collection)
						{
							targetedEventWrapper = collection[path[i]];
						}
						else
						{
							throw new InvalidOperationException ("Something went wrong!");
						}
					}

					if (targetedEventWrapper is IEventField field)
					{
						object newValue = packet.Data.ToObject (entityType);
						field.SetValue (newValue);
					}
					break;
			}
		}
	}
}
