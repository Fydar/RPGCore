using System;
using UnityEngine;

namespace RPGCore.Inventories
{
	public abstract class ItemSlot
	{
		public Action onBeforeChanged;
		public Action onAfterChanged;

		[NonSerialized]
		public Sprite SlotDecoration = null;

		protected readonly RPGCharacter owner = null;
		public readonly ItemSlotBehaviour[] Behaviours = null;
		public readonly ItemCondition[] Conditions = null;

		[NonSerialized]
		private ItemSurrogate item;
		private bool enabled = true;

		public bool IsActivatable
		{
			get
			{
				if (item == null)
					return false;

				return item.template.GetNode<ActivatableNode> () != null;
			}
		}

		public void TryUse ()
		{
			if (item == null)
				return;

			item.TryUse ();
		}

		public RPGCharacter Owner
		{
			get
			{
				return owner;
			}
		}

		public bool Enabled
		{
			get
			{
				return enabled;
			}
			set
			{
				if (enabled == value)
					return;

				enabled = value;

				if (Item != null && Behaviours != null)
				{
					if (enabled)
					{
						for (int i = 0; i < Behaviours.Length; i++)
						{
							ItemSlotBehaviour behaviour = Behaviours[i];

							behaviour.OnSlotEnable (this);
						}
					}
					else
					{
						for (int i = 0; i < Behaviours.Length; i++)
						{
							ItemSlotBehaviour behaviour = Behaviours[i];

							behaviour.OnSlotDisable (this);
						}
					}
				}
			}
		}

		public virtual ItemSurrogate Item
		{
			get
			{
				return item;
			}
			set
			{
				if (item != null)
				{
					item.data.quantity.onChanged -= OnQuantityChanged;
				}

				ItemSurrogate nextItem = (value == null) ? Default : value;

				if (item != null && Behaviours != null)
				{
					for (int i = 0; i < Behaviours.Length; i++)
					{
						ItemSlotBehaviour behaviour = Behaviours[i];

						behaviour.OnExitSlot (this);

						if (Enabled)
							behaviour.OnSlotDisable (this);
					}
				}

				if (onBeforeChanged != null)
					onBeforeChanged ();

				if (nextItem != null)
				{
					nextItem.owner.Value = owner;
					nextItem.data.quantity.onChanged += OnQuantityChanged;
				}

				item = nextItem;

				if (nextItem != null && Behaviours != null)
				{
					for (int i = 0; i < Behaviours.Length; i++)
					{
						ItemSlotBehaviour behaviour = Behaviours[i];

						behaviour.OnEnterSlot (this);

						if (Enabled)
							behaviour.OnSlotEnable (this);
					}
				}

				if (onAfterChanged != null)
					onAfterChanged ();
			}
		}

		public virtual ItemSurrogate Default
		{
			get
			{
				return null;
			}
		}

		public bool IsDefault
		{
			get
			{
				return Item == Default;
			}
		}

		public ItemSlot (RPGCharacter _owner, ItemSlotBehaviour[] behaviours = null, ItemCondition[] conditions = null)
		{
			owner = _owner;
			Behaviours = behaviours;
			Conditions = conditions;

			foreach (ItemSlotBehaviour behaviour in Behaviours)
			{
				behaviour.Setup (this);
			}
		}

		/// <summary>
		/// Add an item to the slot.
		/// </summary>
		/// <param name="newItem">The new item to add.</param>
		public abstract AddResult Add (ItemSurrogate newItem);

		/// <summary>
		/// Remove the item from the slot.
		/// </summary>
		public abstract void Remove ();

		/// <summary>
		/// Return the item back to the main inventory of this slot.
		/// </summary>
		public abstract void Return ();

		/// <summary>
		/// Swap the item in this slot to another slot, allowing for stack sizes and slot constraints.
		/// </summary>
		/// <param name="other">The other slot to swap to.</param>
		public abstract void Swap (ItemSlot other);

		/// <summary>
		/// Check if an item is valid in this slot
		/// </summary>
		/// <returns><c>true</c> if the item is valid in this slot; otherwise, <c>false</c>.</returns>
		/// <param name="newItem">The item to compare.</param>
		public bool IsValid (ItemSurrogate newItem)
		{
			if (Conditions == null)
				return true;

			return Conditions.IsValid (newItem);
		}

		public virtual T GetSlotBehaviour<T> ()
			where T : ItemSlotBehaviour
		{
			for (int i = 0; i < Behaviours.Length; i++)
			{
				ItemSlotBehaviour behaviour = Behaviours[i];

				if (behaviour.GetType () == typeof (T))
				{
					return (T)behaviour;
				}
			}
			return null;
		}

		private void OnQuantityChanged ()
		{
			if (Item == null)
				return;

			if (Item.Quantity == 0)
				Item = null;
		}
	}
}