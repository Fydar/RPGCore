using System;
using System.Collections.Generic;

namespace RPGCore.Inventories
{
	[Serializable]
	public class EndlessInventory : Inventory
	{
		public EndlessInventory (RPGCharacter owner, ItemSlotBehaviour[] behaviours = null, ItemCondition[] conditions = null)
			: base (owner, behaviours, conditions)
		{

		}

		public override AddResult Add (ItemSurrogate newItem)
		{
			List<ItemSlot> emptySlots = new List<ItemSlot> (Items.Count);

			//bool partiallyComplete = false;

			for (int i = 0; i < Items.Count; i++)
			{
				ItemSlot slot = Items[i];

				if (slot.Item == null)
				{
					emptySlots.Add (slot);
					continue;
				}

				AddResult result = slot.Add (newItem);

				if (result == AddResult.Complete)
				{
					if (OnAddItem != null)
						OnAddItem ();

					return AddResult.Complete;
				}
				//else if (result == AddResult.Partial)
				//	partiallyComplete = true;
			}

			/*for (int i = 0; i < emptySlots.Count; i++)
			{
				ItemSlot slot = emptySlots [i];

				AddResult result = slot.Add (newItem);

				if (result == AddResult.Complete) 
				{
					if (OnAddItem != null)
						OnAddItem ();

					return AddResult.Complete;
				}
				else if (result == AddResult.Partial)
					partiallyComplete = true;
			}*/

			AddResult lastAddResult = AddResult.None;

			while (lastAddResult != AddResult.Complete)
			{
				ItemSlot newSlot = AddSlot ();

				lastAddResult = newSlot.Add (newItem);

				if (OnAddItem != null)
					OnAddItem ();
			}

			return AddResult.Complete;

			/*if (partiallyComplete)
			{
				if (OnAddItem != null)
					OnAddItem ();

				return AddResult.Partial;
			}
			else
				return AddResult.None;*/
		}

		public override bool Remove (ItemSurrogate newItem)
		{
			for (int i = 0; i < Items.Count; i++)
			{
				ItemSlot slot = Items[i];

				if (slot.Item == newItem)
				{
					slot.Item.RemoveAllReferenceSlots ();

					slot.Item = null;

					if (OnSlotRemoved != null)
						OnSlotRemoved (slot);

					Items.Remove (slot);

					return true;
				}
			}

			return false;
		}
	}
}

