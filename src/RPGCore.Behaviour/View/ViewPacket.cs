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

			SetFieldValue,
			AddCollectionItem,
			RemoveCollectionItem
		}

		public ViewPacketType PacketType;
		public EntityRef Entity;
		public string FieldPath;
		public string EntityType;
		public JToken Data;

		public override string ToString()
		{
			return $"{PacketType} {Entity} - {FieldPath}({EntityType}) {Data}";
		}
	}
}
