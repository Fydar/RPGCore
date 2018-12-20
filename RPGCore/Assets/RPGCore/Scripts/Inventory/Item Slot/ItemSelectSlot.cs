using System;
using UnityEngine;

namespace RPGCore.Inventories
{
	[Serializable]
	public class ItemSelectSlot : ItemSlot
	{
		[SerializeField]
		[HideInInspector]
		public int serializedID = -1;

		public ItemSlot[] OriginalSlot;

		private ItemSlot currentSlot;
		private int currentSlotID = 0;
		private bool avoidEmptySlots = false;

		public ItemSlot CurrentSlot
		{
			get
			{
				return currentSlot;
			}
			set
			{
				CurrentSlotID = IndexOfSlot (value);
			}
		}

		public int CurrentSlotID
		{
			get
			{
				return currentSlotID;
			}
			set
			{

				if (Behaviours != null)
				{
					for (int i = 0; i < Behaviours.Length; i++)
					{
						ItemSlotBehaviour behaviour = Behaviours[i];

						behaviour.OnExitSlot (this);

						if (Enabled)
						{
							behaviour.OnSlotDisable (this);
						}
					}
				}

				if (onBeforeChanged != null)
				{
					onBeforeChanged ();
				}

				currentSlotID = value;
				currentSlot = OriginalSlot[value];

				if (Behaviours != null)
				{
					for (int i = 0; i < Behaviours.Length; i++)
					{
						ItemSlotBehaviour behaviour = Behaviours[i];

						behaviour.OnEnterSlot (this);

						if (Enabled)
						{
							behaviour.OnSlotEnable (this);
						}
					}
				}

				if (onAfterChanged != null)
				{
					onAfterChanged ();
				}

			}
		}

		public override void Return ()
		{

		}

		public override ItemSurrogate Item
		{
			get
			{
				if (currentSlot == null)
					return null;

				return currentSlot.Item;
			}
			set
			{
				if (currentSlot == null)
					return;

				currentSlot.Item = value;
			}
		}

		public ItemSelectSlot (bool _avoidEmptySlots = false, ItemSlotBehaviour[] behaviours = null,
			ItemCondition[] conditions = null, params ItemSlot[] originalSlot)
			: base (originalSlot[1].Owner, behaviours, conditions)
		{
			avoidEmptySlots = _avoidEmptySlots;
			OriginalSlot = originalSlot;

			foreach (ItemSlot slot in OriginalSlot)
			{
				if (slot == null)
					continue;

				slot.onAfterChanged += () =>
				{
					if (Behaviours != null)
					{
						for (int i = 0; i < Behaviours.Length; i++)
						{
							ItemSlotBehaviour behaviour = Behaviours[i];

							behaviour.OnEnterSlot (this);

							if (Enabled)
							{
								behaviour.OnSlotEnable (this);
							}
						}
					}

					if (onAfterChanged != null)
						onAfterChanged ();
				};

				slot.onBeforeChanged += () =>
				{
					if (Behaviours != null)
					{
						for (int i = 0; i < Behaviours.Length; i++)
						{
							ItemSlotBehaviour behaviour = Behaviours[i];

							behaviour.OnExitSlot (this);

							if (Enabled)
							{
								behaviour.OnSlotDisable (this);
							}
						}
					}

					if (onBeforeChanged != null)
						onBeforeChanged ();
				};
				//slot.onQuantityChanged += () => onQuantityChanged ();
			}
			if (Behaviours != null)
			{
				for (int i = 0; i < Behaviours.Length; i++)
				{
					ItemSlotBehaviour behaviour = Behaviours[i];

					behaviour.OnEnterSlot (this);

					if (Enabled)
					{
						behaviour.OnSlotEnable (this);
					}
				}
			}

			currentSlot = OriginalSlot[currentSlotID];
		}

		public override AddResult Add (ItemSurrogate newItem)
		{
			return currentSlot.Add (newItem);
		}

		public override void Swap (ItemSlot other)
		{
			currentSlot.Swap (other);
		}

		public override void Remove ()
		{
			currentSlot.Remove ();
		}

		public void SetSerializedID (int id)
		{
			serializedID = id;
		}

		public void NextSlot ()
		{
			int originalID = currentSlotID;
			do
			{
				currentSlotID++;
				if (currentSlotID < OriginalSlot.Length - 1)
				{
					currentSlotID = 0;
				}

				if (!avoidEmptySlots)
					break;

				if (OriginalSlot[currentSlotID].Item != null)
				{
					break;
				}
			}
			while (originalID != currentSlotID);

			//Set the current slot in stone, instead of using the temporary float.
			CurrentSlotID = currentSlotID;
		}

		public void PreviousSlot ()
		{
			int originalID = currentSlotID;
			do
			{
				currentSlotID--;
				if (currentSlotID < 0)
				{
					currentSlotID = OriginalSlot.Length - 1;
				}

				if (!avoidEmptySlots)
					break;

				if (OriginalSlot[currentSlotID].Item != null)
				{
					break;
				}
			}
			while (originalID != currentSlotID);

			//Set the current slot in stone, instead of using the temporary float.
			CurrentSlotID = currentSlotID;
		}

		private int IndexOfSlot (ItemSlot slot)
		{
			for (int i = 0; i < OriginalSlot.Length; i++)
			{
				if (OriginalSlot[i] == slot)
					return i;
			}

			return -1;
		}

		public override T GetSlotBehaviour<T> ()
		{
			if (CurrentSlot != null && CurrentSlot.Behaviours != null)
			{
				for (int i = 0; i < CurrentSlot.Behaviours.Length; i++)
				{
					ItemSlotBehaviour behaviour = CurrentSlot.Behaviours[i];

					if (behaviour.GetType () == typeof (T))
					{
						return (T)behaviour;
					}
				}
			}

			if (Behaviours != null)
			{
				for (int i = 0; i < Behaviours.Length; i++)
				{
					ItemSlotBehaviour behaviour = Behaviours[i];

					if (behaviour.GetType () == typeof (T))
					{
						return (T)behaviour;
					}
				}
			}
			return null;
		}
	}
}

