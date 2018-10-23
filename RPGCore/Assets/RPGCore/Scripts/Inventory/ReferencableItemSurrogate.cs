using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCore.Inventories;

namespace RPGCore
{
	public partial class ItemSurrogate
	{
		public event Action OnReferenceChanged;

		private List<ItemReferenceSlot> referenceSlots = new List<ItemReferenceSlot> ();

		// public BoolEventField Equipped = new BoolEventField (false);

		public void OnBeforeSerialize ()
		{
			for (int i = 0; i < referenceSlots.Count; i++)
			{
				ItemReferenceSlot slot = referenceSlots[i];

				slot.SetSerializedID (i);
			}
		}

		public void AddReferenceSlot (ItemReferenceSlot slot)
		{
			referenceSlots.Clear ();
			referenceSlots.Add (slot);

			if (OnReferenceChanged != null)
				OnReferenceChanged ();
		}

		public void RemoveReferenceSlot (ItemReferenceSlot slot)
		{
			referenceSlots.Remove (slot);

			if (OnReferenceChanged != null)
				OnReferenceChanged ();
		}

		public bool IsReferencedBySlot (ItemReferenceSlot slot)
		{
			for (int i = 0; i < referenceSlots.Count; i++)
			{
				if (referenceSlots[i] == slot)
					return true;
			}
			return false;
		}

		public void RemoveAllReferenceSlots ()
		{
			referenceSlots.Clear ();

			if (OnReferenceChanged != null)
				OnReferenceChanged ();
		}
	}
}