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

		protected override void OnSetup (IBehaviourContext character)
		{
			EventEntry eventInput = Event.GetEntry (character);
			ConnectionEntry<RPGCharacter> targetInput = Target.GetEntry (character);
			ConnectionEntry<int> effectInput = Effect.GetEntry (character);

			eventInput.OnEventFired += () =>
			{
				if (targetInput.Value == null)
					return;
				
				targetInput.Value.TakeDamage (effectInput.Value);
			};
		}

		protected override void OnRemove (IBehaviourContext character)
		{

		}
	}
}