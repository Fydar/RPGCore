using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;

namespace RPGCore
{
	[NodeInformation ("Character/Heal")]
	public class HealNode : BehaviourNode
	{
		public CharacterInput Target;
		public EventInput Event;
		public IntInput Effect;

		protected override void OnSetup (IBehaviourContext context)
		{
			EventEntry eventInput = Event[context];
			ConnectionEntry<RPGCharacter> targetInput = Target[context];
			ConnectionEntry<int> effectInput = Effect[context];

			eventInput.OnEventFired += () =>
			{
				if (targetInput.Value == null)
					return;

				targetInput.Value.Heal (effectInput.Value);
			};
		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}
	}
}

