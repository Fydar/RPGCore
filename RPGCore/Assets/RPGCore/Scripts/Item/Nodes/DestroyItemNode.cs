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
		public IntInput Count;

		protected override void OnSetup (IBehaviourContext context)
		{
			ConnectionEntry<int> countInput = Count[context];
			EventEntry eventInput = Event[context];

			ItemSurrogate item = (ItemSurrogate)context;

			Action updateHandler = () =>
			{
				item.data.quantity.Value -= Mathf.Max (0, countInput.Value);
			};

			eventInput.OnEventFired += updateHandler;
		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}
	}
}