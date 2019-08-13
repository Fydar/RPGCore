using System;
using UnityEngine;

namespace RPGCore.Inventories
{
	[Serializable]
	public class ItemStorageSlot : ItemSlot
	{
		public uint StackSize = 0;

		public ItemStorageSlot (RPGCharacter owner, ItemSlotBehaviour[] behaviours = null, ItemCondition[] conditions = null)
			: base (owner, behaviours, conditions)
		{
		}

		public ItemStorageSlot (RPGCharacter owner, uint stackSize, ItemSlotBehaviour[] behaviours = null, ItemCondition[] conditions = null)
			: base (owner, behaviours, conditions)
		{
			StackSize = stackSize;
		}

		public override AddResult Add (ItemSurrogate newItem)
		{
			if (!IsValid (newItem))
				return AddResult.None;

			if (Item == null)
			{
				int amountCanAdd = Mathf.Min (newItem.template.StackSize, newItem.Quantity);

				if (amountCanAdd == newItem.Quantity)
				{
					Item = newItem;
					return AddResult.Complete;
				}
				else if (amountCanAdd != 0)
				{
					newItem.Quantity -= amountCanAdd;
					Item = newItem.Template.GenerateItem ();
					Item.Quantity += amountCanAdd - 1;

					return AddResult.Partial;
				}
				else
				{
					Item = newItem;

					return AddResult.Complete;
				}
			}

			if (Item.template == newItem.template)
			{
				if (Item.template.StackSize == 0)
				{
					Item.Quantity += newItem.Quantity;
					newItem.Quantity = 0;
					return AddResult.Complete;
				}

				int amountCanAdd = Mathf.Min (Item.template.StackSize - Item.Quantity, newItem.Quantity);

				if (amountCanAdd != 0)
				{
					newItem.Quantity -= amountCanAdd;
					Item.Quantity += amountCanAdd;

					if (newItem.Quantity == 0)
						return AddResult.Complete;
					else
						return AddResult.Partial;
				}
				else
				{
					return AddResult.None;
				}
			}

			//Check if stackable with previous stack

			//Add item to slot

			return AddResult.None;
		}

		public override void Swap (ItemSlot other)
		{
			// other is the start slot
			// this is the end slot

			if (other == null)
			{
				Return ();
				return;
			}

			if (!IsValid (other.Item))
				return;

			if (!other.IsValid (Item))
				return;

			if (other.Item == null)
			{
				ItemSurrogate swapItem = Item;

				Item = null;
				other.Item = swapItem;
				return;
			}

			ItemSurrogate tempItem = other.Item;
			other.Item = null;

			AddResult result = Add (tempItem);

			if (result == AddResult.None)
			{
				ItemSurrogate tempBItem = Item;
				Item = null;

				other.Item = tempBItem;
				Item = tempItem;
			}
			else if (result == AddResult.Partial || result == AddResult.Referenced)
			{
				other.Item = tempItem;
			}
		}

		public override void Remove ()
		{
			Item = null;
		}

		public override void Return ()
		{
			if (IsDefault)
				return;

			if (Item != null)
				Owner.inventory.Add (Item);

			Remove ();
		}
	}
}

