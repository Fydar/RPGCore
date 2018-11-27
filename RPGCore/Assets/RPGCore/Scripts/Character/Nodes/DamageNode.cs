using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation ("Character/Damage")]
	public class DamageNode : BehaviourNode
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
				
				targetInput.Value.TakeDamage (effectInput.Value);
			};
		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}
	}
}