﻿using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using System;

namespace RPGCore
{
	[NodeInformation("Item/Grant", "Inventory")]
	public class GrantItem : BehaviourNode
	{
		public ItemGenerator Item;
		public CharacterInput Character;
		public EventInput Event;

		protected override void OnSetup(IBehaviourContext context)
		{
			var eventInput = Event[context];
			var characterInput = Character[context];

			Action updateHandler = () =>
			{
				if (characterInput.Value == null)
				{
					return;
				}

				var item = Item.Generate();

				characterInput.Value.inventory.Add(item);
			};

			eventInput.OnEventFired += updateHandler;
		}

		protected override void OnRemove(IBehaviourContext context)
		{
		}
	}
}
