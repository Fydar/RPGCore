using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation ("Item/Grant", "Inventory")]
	public class GrantItem : BehaviourNode
	{
		public ItemGenerator Item;
		public CharacterInput Character;
		public EventInput Event;

		protected override void OnSetup (IBehaviourContext context)
		{
			EventEntry eventInput = Event[context];
			ConnectionEntry<RPGCharacter> characterInput = Character[context];

			Action updateHandler = () =>
			{
				if (characterInput.Value == null)
					return;

				ItemSurrogate item = Item.Generate ();

				characterInput.Value.inventory.Add (item);
			};

			eventInput.OnEventFired += updateHandler;
		}

		protected override void OnRemove (IBehaviourContext character)
		{

		}
	}
}