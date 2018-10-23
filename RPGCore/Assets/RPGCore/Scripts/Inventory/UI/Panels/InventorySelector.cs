using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace RPGCore.Inventories
{
	public class InventorySelector : ExpandableInventoryRenderer
	{
		public RectTransform NoneSlot;
		public ItemRenderer DefaultSlot;

		Action<ItemSlot> returnCallback;
		Action<ItemSlot> hoveredCallback;
		Action<ItemSlot> unhoveredCallback;

		protected override void OnSlotAdd (ItemSlotManager manager)
		{
			manager.onMainAction += () =>
			{
				ReturnSlot (manager.slot);
				manager.Hovered = false;
			};

			manager.onHovered += () =>
			{
				HoveredSlot (manager.slot);
			};

			manager.onUnfocus += () =>
			{
				UnhoveredSlot (manager.slot);
			};
		}

		protected override void OnSlotRemove (ItemSlotManager slot)
		{

		}

		public void DisplaySelector (ItemCondition[] Conditions, Action<ItemSlot> onSelected)
		{
			Debug.Log ("Displaying Selector");
			Filter (SortingMode.Type, Conditions);
			gameObject.SetActive (true);

			returnCallback = onSelected;

			NoneSlot.gameObject.SetActive (true);
			DefaultSlot.gameObject.SetActive (false);
		}

		public void DisplaySelector (ItemSlot forSlot, Action<ItemSlot> onSelected)
		{
			Filter (SortingMode.Type, forSlot.Conditions);
			gameObject.SetActive (true);

			returnCallback = onSelected;

			if (forSlot.Default == null)
			{
				NoneSlot.gameObject.SetActive (true);
				DefaultSlot.gameObject.SetActive (false);
			}
			else
			{
				NoneSlot.gameObject.SetActive (false);
				DefaultSlot.gameObject.SetActive (true);

				DefaultSlot.RenderItem (forSlot.Default);

				if (forSlot.IsDefault)
				{
					DefaultSlot.ShowEquipped ();

					Slot_Pair pairSlot = forSlot.GetSlotBehaviour<Slot_Pair> ();
					if (pairSlot != null && pairSlot.pair.Item != null)
					{
						if (pairSlot.pair.Item.EquiptableSlot == Slot.TwoHanded)
						{
							DefaultSlot.HideEquipped ();
						}
					}
				}
			}
		}

		public void ReturnSlot (ItemSlot returnedSlot)
		{
			if (returnCallback != null)
			{
				returnCallback (returnedSlot);
				returnCallback = null;
			}
			gameObject.SetActive (false);
		}

		#region Hovered Support
		public void DisplaySelectorHovered (ItemSlot conditionSlot, Action<ItemSlot> onHovered, Action<ItemSlot> onUnhovered)
		{
			ListenToHovered (onHovered);
			ListenToUnhovered (onUnhovered);

			Filter (SortingMode.Type, conditionSlot.Conditions);
			gameObject.SetActive (true);

			NoneSlot.gameObject.SetActive (false);
			DefaultSlot.gameObject.SetActive (false);
		}

		public void DisplaySelectorHovered (ItemCondition[] conditions, Action<ItemSlot> onHovered, Action<ItemSlot> onUnhovered)
		{
			ListenToHovered (onHovered);
			ListenToUnhovered (onUnhovered);

			Filter (SortingMode.Type, conditions);
			gameObject.SetActive (true);

			NoneSlot.gameObject.SetActive (false);
			DefaultSlot.gameObject.SetActive (false);
		}

		public void HoveredSlot (ItemSlot hoveredSlot)
		{
			if (hoveredCallback != null)
			{
				hoveredCallback (hoveredSlot);
				//hoveredCallback = null;
			}
		}

		public void UnhoveredSlot (ItemSlot unhoveredSlot)
		{
			if (unhoveredCallback != null)
			{
				unhoveredCallback (unhoveredSlot);
				//unhoveredCallback = null;
			}
		}

		public void ListenToHovered (Action<ItemSlot> onHovered)
		{
			hoveredCallback = onHovered;
		}

		public void ListenToUnhovered (Action<ItemSlot> onUnhovered)
		{
			unhoveredCallback = onUnhovered;
		}
		#endregion
	}
}