using RPGCore.Behaviour;

namespace RPGCore.Traits
{
	public class StateInstance : IEventField<float>
	{
		public StateIdentifier Identifier { get; set; }

		public float Value { get; set; }

		public EventFieldHandlerCollection Handlers { get; }

		public StateTemplate Template;

		public StateInstance()
		{
			Handlers = new EventFieldHandlerCollection (this);
		}

		public void Dispose()
		{

		}
	}
}
