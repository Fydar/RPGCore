using UnityEngine;

namespace RPGCore.Inventories
{
	public class ItemReferenceSlot : ItemSlot
	{
		[SerializeField]
		[HideInInspector]
		public int serializedID = -1;

		private ItemSurrogate defaultItem = null;

		public override ItemSurrogate Default
		{
			get
			{
				return defaultItem;
			}
		}

		public ItemReferenceSlot (RPGCharacter owner, ItemSlotBehaviour[] behaviours = null, ItemCondition[] conditions = null,
			ItemSurrogate _defaultItem = null)
			: base (owner, behaviours, conditions)
		{
			defaultItem = _defaultItem;
		}

		public override AddResult Add (ItemSurrogate newItem)
		{
			if (Item == newItem)
				return AddResult.None;

			if (!IsValid (newItem))
				return AddResult.None;

			RemoveTargetSlot (Item);
			SetTargetSlot (newItem);

			Item = newItem;

			return AddResult.Referenced;
		}

		public override void Swap (ItemSlot other)
		{
			// other is the start slot
			// this is the end slot

			if (other == null)
			{
				//Trying to swap top no slot, means remove item or send item to inventory

				RemoveTargetSlot (Item);

				Item = Default;

				return;
			}

			if (!IsValid (other.Item))
			{
				return;
			}

			RemoveTargetSlot (Item);
			SetTargetSlot (other.Item);

			Item = other.Item;
		}

		public override void Remove ()
		{
			if (Item != null)
			{
				RemoveTargetSlot (Item);

				Item = Default;
			}
		}

		public override void Return ()
		{
			Remove ();
		}

		public void SetSerializedID (int id)
		{
			serializedID = id;
		}

		private void SetTargetSlot (ItemSurrogate item)
		{
			if (item != null)
			{
				item.AddReferenceSlot (this);

				item.OnReferenceChanged += DetachListener;
			}
		}

		private void RemoveTargetSlot (ItemSurrogate item)
		{
			if (item != null)
			{
				item.OnReferenceChanged -= DetachListener;

				item.RemoveReferenceSlot (this);
			}
		}

		private void DetachListener ()
		{
			if (!Item.IsReferencedBySlot (this))
			{
				Remove ();
			}
		}
	}
}