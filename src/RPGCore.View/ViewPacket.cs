using Newtonsoft.Json.Linq;

namespace RPGCore.View
{
	public struct ViewPacket
	{
		public enum ViewPacketType
		{
			None,

			CreateEntity,
			DestroyEntity,
			UpdateEntity
		}

		public ViewPacketType PacketType;
		public EntityRef Entity;
		public string FieldPath;
		public string EntityType;
		public JObject Data;
	}
}
