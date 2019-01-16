using RPGCore.UI;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPGCore.Inventories
{
	/// <summary>
	/// Controls the rendering of a slot from an inventory. 
	/// </summary>
	public class ItemSlotManager : MonoBehaviour, IPointerClickHandler, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
	{
		public static ItemSlotManager CurrentlyHovered;

		[Header ("Input")]
		public bool Hovered;

		[NonSerialized]
		public ItemSlot slot;
		[NonSerialized]
		public ItemRenderer slotRenderer;

		public event Action onMainAction;
		public event Action onSecondaryAction;
		public event Action onHovered;
		public event Action onUnfocus;

		private Action changeHandler;

		private Action slotBeforeChangeHandler;
		private Action slotAfterChangeHandler;

		[NonSerialized]
		private bool hasInitiated;

		private void Awake ()
		{
			Initiate ();
		}

		private void Initiate ()
		{
			if (hasInitiated)
				return;

			changeHandler = OnSlotChanged;
			slotRenderer = GetComponent<ItemRenderer> ();

			slotBeforeChangeHandler = () =>
			{
				if (slot.Item != null)
					slot.Item.data.quantity.onChanged -= changeHandler;
			};

			slotAfterChangeHandler = () =>
			{
				OnSlotChanged ();

				if (slot == null)
				{
					Debug.LogError ("Setting up on a null slot.");
					return;
				}

				if (slot.Item != null)
				{
					slot.Item.data.quantity.onChanged += changeHandler;
				}
			};

			hasInitiated = true;
		}

		private void Update ()
		{
			if (Hovered)
			{
				if (Input.GetMouseButtonDown (0))
				{
					if (onMainAction != null)
						onMainAction ();
				}

				if (Input.GetMouseButtonDown (1))
				{
					if (onSecondaryAction != null)
						onSecondaryAction ();
				}
			}
		}

		public void Setup (ItemSlot _slot)
		{
			Initiate ();
			Detatch ();
			slot = _slot;

			slot.onBeforeChanged += slotBeforeChangeHandler;
			slot.onAfterChanged += slotAfterChangeHandler;

			slotAfterChangeHandler ();

			Slot_Pair pairSlot = slot.GetSlotBehaviour<Slot_Pair> ();
			if (pairSlot != null)
			{
				pairSlot.pair.onBeforeChanged += slotBeforeChangeHandler;
				pairSlot.pair.onAfterChanged += slotAfterChangeHandler;
			}

			OnSlotChanged ();
		}

		public void Detatch ()
		{
			if (slot != null)
			{
				slot.onBeforeChanged -= slotBeforeChangeHandler;
				slot.onAfterChanged -= slotAfterChangeHandler;

				Slot_Pair pairSlot = slot.GetSlotBehaviour<Slot_Pair> ();
				if (pairSlot != null)
				{
					pairSlot.pair.onBeforeChanged -= slotBeforeChangeHandler;
					pairSlot.pair.onAfterChanged -= slotAfterChangeHandler;
				}

				slotRenderer.RenderEmpty ();
			}

			slot = null;
		}

		void OnSlotChanged ()
		{
			if (slot == null)
				return;

			slotRenderer.RenderSlot (slot);
		}

		public void Hover ()
		{
			if (Hovered)
				return;

			Hovered = true;
			CurrentlyHovered = this;
			if (onHovered != null)
			{
				onHovered ();
			}
		}

		public void Unhover ()
		{
			if (!Hovered)
				return;

			Hovered = false;

			if (CurrentlyHovered == this)
				CurrentlyHovered = null;

			if (onUnfocus != null)
				onUnfocus ();
		}

		public void OnPointerEnter (PointerEventData eventData)
		{
			Hover ();
		}

		public void OnPointerExit (PointerEventData eventData)
		{
			Unhover ();
		}

		public void OnSelect (BaseEventData eventData)
		{
			Hover ();
		}

		public void OnDeselect (BaseEventData eventData)
		{
			Unhover ();
		}

		public void OnPointerClick (PointerEventData eventData)
		{
			if (slot.Item == null)
				return;

			if (eventData.button == PointerEventData.InputButton.Right)
			{
				UIContextMenu.Instance.Display (
					(slot.IsActivatable) ? (IContextEntry)new ContextButton ("Use", slot.TryUse) : new ContextNone (),
					new ContextButton ("Drop", slot.Remove),
					new ContextButton ("Destroy", () =>
					{
						PopupMenu.Instance.Display ("Destory Item",
							new PopupButton ("Cancel", null),
							new PopupButton ("Destroy", slot.Remove)
						);
					})
				);
			}
		}
	}
}

