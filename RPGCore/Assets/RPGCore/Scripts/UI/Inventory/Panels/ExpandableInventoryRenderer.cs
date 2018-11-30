using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPGCore.Inventories
{
	public abstract class ExpandableInventoryRenderer : InventoryRendererBase
	{
		public enum SortingMode : int
		{
			Type = 0,
			Name = 1,
			DateAquired = 2,
		}

		[Header ("Setup")]
		public RectTransform slotHolder;
		public ItemSlotManager SlotPrefab;

		private SortingMode lastSorting;
		private ItemCondition[] LastFilters;
		private List<ItemSlot> sorted = new List<ItemSlot> ();

		public bool AutoFilter = false;
		public bool HideEmpty = false;

		private bool filterDirty = false;

		public override void Setup (Inventory inventory)
		{
			if (current != null)
				return;

			current = inventory;
			current.OnSlotAdded += SlotAddCallback;
			current.OnSlotRemoved += SlotRemoveCallback;

			current.OnAddItem += () =>
			{
				filterDirty = true;
			};

			managers = new List<ItemSlotManager> (current.Size);

			for (int i = 0; i < current.Items.Count; i++)
			{
				ItemSlot slot = current.Items[i];

				CreateManager (slot);
			}

			if (AutoFilter)
				Filter (lastSorting);
		}

		private void Update ()
		{
			if (filterDirty)
			{
				if (AutoFilter)
					Filter (lastSorting, LastFilters);
			}
		}

		private void SlotAddCallback (ItemSlot slot)
		{
			CreateManager (slot);

			filterDirty = true;
		}

		private void SlotRemoveCallback (ItemSlot slot)
		{
			for (int i = 0; i < managers.Count; i++)
			{
				ItemSlotManager manager = managers[i];

				if (manager.slot == slot)
				{
					manager.Detatch ();
					Destroy (manager.gameObject);
					managers.RemoveAt (i);

					OnSlotRemove (manager);

					return;
				}
			}

			filterDirty = true;
		}

		protected ItemSlotManager CreateManager (ItemSlot slot)
		{
			ItemSlotManager clone = Instantiate (SlotPrefab, slotHolder);
			clone.transform.localRotation = Quaternion.identity;
			clone.transform.localScale = Vector3.one;
			clone.GetComponent<RectTransform> ().anchoredPosition3D = Vector3.zero;

			clone.Setup (slot);
			managers.Add (clone);

			OnSlotAdd (clone);

			return clone;
		}

		public void SortSet (int mode)
		{
			Filter ((SortingMode)mode);
		}

		public void Filter ()
		{
			Filter (lastSorting, LastFilters);
		}

		public void Filter (ItemCondition[] filters)
		{
			Filter (lastSorting, filters);
		}

		public void Filter (SortingMode mode)
		{
			Filter (mode, LastFilters);
		}

		public void Filter (SortingMode mode, ItemCondition[] Filters)
		{
			lastSorting = mode;
			LastFilters = Filters;

			sorted = new List<ItemSlot> ();

			for (int i = 0; i < current.Items.Count; i++)
			{
				ItemSlot slot = current.Items[i];

				if (HideEmpty && slot.Item == null)
					continue;

				if (Filters == null)
				{
					sorted.Add (slot);
					continue;
				}

				if (Filters.IsValid (slot.Item))
				{
					sorted.Add (slot);
				}
			}

			if (mode == SortingMode.Name)
			{
				sorted.Sort (NameComparison);
			}
			else if (mode == SortingMode.Type)
			{
				sorted.Sort (TypeComparison);
			}

			for (int i = 0; i < managers.Count; i++)
			{
				ItemSlotManager manager = managers[i];

				if (EventSystem.current.currentSelectedGameObject == manager.gameObject)
				{
					EventSystem.current.SetSelectedGameObject (null);
				}
				manager.Detatch ();
				manager.gameObject.SetActive (false);
			}

			for (int i = 0; i < sorted.Count; i++)
			{
				ItemSlotManager manager = managers[i];

				manager.gameObject.SetActive (true);

				manager.Setup (sorted[i]);

				/*if (i == 0)
                {
                    EventSystem.current.SetSelectedGameObject(manager.gameObject);
                    var pointer = new PointerEventData(EventSystem.current);
                    //ExecuteEvents.Execute(manager.gameObject, pointer, ExecuteEvents.pointerEnterHandler);
                    //ExecuteEvents.Execute(manager.gameObject, pointer, ExecuteEvents.selectHandler);
                }*/
			}

			filterDirty = false;
		}

		private int NameComparison (ItemSlot a, ItemSlot b)
		{
			if (a == null)
				return 1;

			if (b == null)
				return -1;

			if (a.Item == null)
				return 1;

			if (b.Item == null)
				return -1;

			return string.Compare (a.Item.BaseName, b.Item.BaseName);
		}

		private int TypeComparison (ItemSlot a, ItemSlot b)
		{
			if (a == null)
				return 1;

			if (b == null)
				return -1;

			if (a.Item == null)
				return 1;

			if (b.Item == null)
				return -1;

			Slot aSlot = a.Item.EquiptableSlot;
			Slot bSlot = b.Item.EquiptableSlot;

			if (aSlot == bSlot)
			{
				return NameComparison (a, b);
			}

			if (((int)aSlot) > ((int)bSlot))
				return 1;
			else
				return -1;
		}
	}
}

