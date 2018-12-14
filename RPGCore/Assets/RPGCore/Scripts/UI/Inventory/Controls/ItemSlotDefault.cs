using UnityEngine;
using UnityEngine.EventSystems;

namespace RPGCore.Inventories
{
	public class ItemSlotDefault : MonoBehaviour, IPointerClickHandler
	{

		public void OnPointerClick (PointerEventData eventData)
		{
			GetComponentInParent<InventorySelector> ().ReturnSlot (null);
		}
	}
}

