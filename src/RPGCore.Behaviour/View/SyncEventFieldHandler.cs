using Newtonsoft.Json.Linq;
using RPGCore.Behaviour;

namespace RPGCore.View
{
	public class SyncEventFieldHandler : IEventFieldHandler
	{
		private readonly ViewDispatcher dispatcher;
		private readonly EntityRef entity;
		private readonly string path;
		private readonly IEventField field;

		public SyncEventFieldHandler(ViewDispatcher dispatcher, EntityRef entity, string path, IEventField field)
		{
			this.dispatcher = dispatcher;
			this.entity = entity;
			this.path = path;
			this.field = field;
		}

		public void OnAfterChanged()
		{
			object value = field.GetValue ();
			var valueType = value.GetType ();

			var packet = new ViewPacket
			{
				PacketType = ViewPacket.ViewPacketType.SetFieldValue,
				EntityType = valueType.ToString (),
				Data = JToken.FromObject (value),
				Entity = entity,
				FieldPath = path
			};

			dispatcher.OnPacketGenerated?.Invoke (packet);
		}

		public void OnBeforeChanged()
		{
		}

		public void Dispose()
		{
		}
	}
}
