using RPGCore.Behaviour;
using System.Linq;

namespace RPGCore.View
{
	public class GameView
	{
		public EventCollection<EntityRef, Entity> Entities = new EventCollection<EntityRef, Entity> ();

		public void Apply (ViewPacket packet)
		{
			switch (packet.PacketType)
			{
				case ViewPacket.ViewPacketType.CreateEntity:
					var clonedEntity = packet.Data.ToObject<ViewCharacter> ();
					Entities.Add (clonedEntity.Id, clonedEntity);
					break;

				case ViewPacket.ViewPacketType.DestroyEntity:
					Entities.Remove (packet.Entity);
					break;

				case ViewPacket.ViewPacketType.UpdateEntity:

					var type = typeof (string);

					object newValue = packet.Data.ToObject (type);

					Entities[packet.Entity].SyncedObjects.Where (s => s.Key == packet.FieldPath)
						.First ().Value.SetValue (newValue);


					break;
			}
		}
	}
}
