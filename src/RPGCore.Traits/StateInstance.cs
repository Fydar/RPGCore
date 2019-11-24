using RPGCore.Behaviour;
using RPGCore.View;

namespace RPGCore.Traits
{
	public class StateInstance : IEventField<float>, ISyncField
	{
		public StateIdentifier Identifier { get; set; }

		public float Value { get; set; }

		public EventFieldHandlerCollection Handlers { get; }

		public StateTemplate Template;

		public StateInstance()
		{
			Handlers = new EventFieldHandlerCollection (this);
		}

		public object GetValue()
		{
			return Value;
		}

		public void SetValue(object value)
		{
			Value = (float)value;
		}

		public void Dispose()
		{
			
		}

		public object AddSyncHandler(ViewDispatcher viewDispatcher, EntityRef root, string path)
		{
			var handler = new SyncEventFieldHandler (viewDispatcher, root, path, this);
			Handlers[this].Add (handler);
			return handler;
		}

		public void Apply(ViewPacket packet)
		{
			Value = packet.Data.ToObject<float> ();
		}
	}
}
