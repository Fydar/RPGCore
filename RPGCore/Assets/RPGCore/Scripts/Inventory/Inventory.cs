using System;
using System.Collections.Generic;
using RPGCore;

namespace RPGCore.Inventories
{
	public abstract class Inventory : IItemCollection
	{
		public Action OnAddItem;
		public Action<ItemSlot> OnSlotAdded;
		public Action<ItemSlot> OnSlotRemoved;

		public List<ItemSlot> Items;

		private ItemSlotBehaviour[] Behaviours = null;
		private ItemCondition[] Conditions = null;

		private RPGCharacter owner;

		public int Size
		{
			get
			{
				return Items.Count;
			}
		}

		public Inventory (RPGCharacter _owner, ItemSlotBehaviour[] behaviours = null, ItemCondition[] conditions = null)
		{
			owner = _owner;
			Behaviours = behaviours;
			Conditions = conditions;

			Items = new List<ItemSlot> ();
		}

		public abstract AddResult Add (ItemSurrogate newItem);
		public abstract bool Remove (ItemSurrogate newItem);

		public virtual void Clear ()
		{
			foreach (ItemSlot slot in Items)
			{
				slot.Remove ();
			}
		}

		public void Resize (int size)
		{
			while (Items.Count < size)
			{
				ItemSlot slot = CreateItemSlot ();

				Items.Add (slot);

				if (OnSlotAdded != null)
					OnSlotAdded (slot);
			}

			while (Items.Count > size)
			{
				if (OnSlotRemoved != null)
					OnSlotRemoved (Items[Items.Count - 1]);

				Items.RemoveAt (Items.Count - 1);
			}

		}

		protected ItemSlot AddSlot ()
		{
			ItemSlot newSlot = CreateItemSlot ();
			Items.Add (newSlot);

			if (OnSlotAdded != null)
				OnSlotAdded (newSlot);

			return newSlot;
		}

		protected void AddSlots (int size)
		{
			for (int i = 0; i < size; i++)
			{
				ItemSlot slot = CreateItemSlot ();
				Items.Add (slot);

				if (OnSlotAdded != null)
					OnSlotAdded (slot);
			}

		}

		private ItemSlot CreateItemSlot ()
		{
			return new ItemStorageSlot (owner, Behaviours, Conditions);
		}
	}
}