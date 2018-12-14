using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using System;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation ("Item/Destroy", "Inventory")]
	public class DestroyItemNode : BehaviourNode
	{
		public EventInput Event;
		public ItemInput Item;
		public IntInput Count;

		protected override void OnSetup (IBehaviourContext context)
		{
			ConnectionEntry<int> countInput = Count[context];
			ConnectionEntry<ItemSurrogate> itemInput = Item[context];
			EventEntry eventInput = Event[context];

			Action updateHandler = () =>
			{
				itemInput.Value.data.quantity.Value -= Mathf.Max (0, countInput.Value);
			};

			eventInput.OnEventFired += updateHandler;
		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}
	}
}
