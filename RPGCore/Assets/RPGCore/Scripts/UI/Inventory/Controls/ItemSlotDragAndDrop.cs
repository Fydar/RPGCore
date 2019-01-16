using RPGCore.Audio;
using UnityEngine;

namespace RPGCore.Inventories
{
	public class ItemSlotDragAndDrop : MonoBehaviour
	{
		public static ItemSlotDragAndDrop instance;

		public enum DragPositioner
		{
			Fixed,
			Offset,
		}

		public enum DragState
		{
			None,
			Potential,
			Dragging
		}

		[Header ("Reference")]
		public ItemRenderer ghost;
		public CanvasMouse mouse;

		[Header ("Setup")]
		public float dragThreshhold = 6.0f;
		public DragPositioner positioner;
		public bool EnableClickDrag;

		[Header ("States")]
		public DragState dragState = DragState.None;
		public Vector3 dragStartPosition;

		private ItemSlotManager startSlot;
		private ItemSlotManager lastHoveringSlot;

		void Awake ()
		{
			instance = this;

			ghost.gameObject.SetActive (false);
		}

		void Update ()
		{
			transform.position = mouse.ScreenToCanvas (Input.mousePosition);

			if (dragState == DragState.None)
			{
				if (Input.GetMouseButtonDown (0) && ItemSlotManager.CurrentlyHovered != null &&
					ItemSlotManager.CurrentlyHovered.slot != null &&
					ItemSlotManager.CurrentlyHovered.slot.Item != null)
				{
					dragState = DragState.Potential;
					dragStartPosition = transform.position;

					startSlot = ItemSlotManager.CurrentlyHovered;
					startSlot.slotRenderer.Faded = true;
				}
			}

			if (dragState == DragState.Potential)
			{
				if (!EnableClickDrag && Input.GetMouseButtonUp (0))
				{
					dragState = DragState.None;
					ghost.gameObject.SetActive (false);
					ghost.RenderSlot (null);
					startSlot.slotRenderer.Faded = false;
					startSlot = null;
				}
				else if (Vector3.Distance (dragStartPosition, Input.mousePosition) > dragThreshhold)
				{
					dragState = DragState.Dragging;

					if (startSlot.slot.Item.template.StartDrag != null)
						AudioManager.Play (startSlot.slot.Item.template.StartDrag);

					if (positioner == DragPositioner.Offset)
					{
						Vector3 originalPosition = transform.position;
						transform.position = dragStartPosition;

						ghost.RenderSlot (startSlot.slot);
						ghost.transform.position = startSlot.transform.position;

						transform.position = originalPosition;
					}
					else if (positioner == DragPositioner.Fixed)
					{
						ghost.RenderSlot (startSlot.slot);
						ghost.transform.localPosition = Vector3.zero; ;
					}

					ghost.gameObject.SetActive (true);

					// TODO: Tell Item Slot to fade out
				}
				else
				{
					return;
				}
			}

			if (dragState == DragState.Dragging)
			{
				if (lastHoveringSlot != ItemSlotManager.CurrentlyHovered)
				{
					if (lastHoveringSlot != null)
						lastHoveringSlot.slotRenderer.Faded = false;

					lastHoveringSlot = ItemSlotManager.CurrentlyHovered;

					if (lastHoveringSlot != null)
					{
						if (lastHoveringSlot.slot.IsValid (startSlot.slot.Item))
							lastHoveringSlot.slotRenderer.Faded = true;
					}
				}

				if (Input.GetMouseButtonUp (0))
				{
					dragState = DragState.None;
					ghost.gameObject.SetActive (false);
					ghost.RenderSlot (null);

					if (startSlot.slot.Item.template.EndDrag != null)
						AudioManager.Play (startSlot.slot.Item.template.EndDrag);

					if (ItemSlotManager.CurrentlyHovered == null)
					{

					}
					else if (ItemSlotManager.CurrentlyHovered != null)
					{
						ItemSlotManager endSlot = ItemSlotManager.CurrentlyHovered;

						endSlot.slot.Swap (startSlot.slot);
					}

					startSlot.slotRenderer.Faded = false;
					startSlot = null;
				}
			}
		}
	}
}

