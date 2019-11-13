﻿using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using RPGCore.Utility;

namespace RPGCore
{
	[NodeInformation("Buff/Clear")]
	public class ClearBuffs : BehaviourNode
	{
		[ErrorIfNull]
		public BuffTemplate Search;
		public CharacterInput Target;
		public EventInput Apply;

		protected override void OnSetup(IBehaviourContext context)
		{
			var applyInput = Apply[context];
			var targetInput = Target[context];

			applyInput.OnEventFired += () =>
			{
				if (targetInput.Value == null)
				{
					return;
				}

				var buff = targetInput.Value.Buffs.Find(Search);

				if (buff != null)
				{
					// Debug.Log ("Removing: " + buff.buffTemplate.name);
					buff.RemoveBuff();
				}
			};
		}

		protected override void OnRemove(IBehaviourContext context)
		{
		}
	}
}

