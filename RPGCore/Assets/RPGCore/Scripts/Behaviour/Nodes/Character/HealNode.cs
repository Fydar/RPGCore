using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation ("Character/Heal")]
	public class HealNode : BehaviourNode
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

				targetInput.Value.Heal (effectInput.Value);
			};
		}

		protected override void OnRemove (IBehaviourContext character)
		{

		}
	}
}