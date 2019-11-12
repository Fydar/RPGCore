using RPGCore.Behaviour;

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
					break;
			}
		}
	}
}
